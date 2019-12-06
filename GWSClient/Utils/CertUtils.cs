using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text.RegularExpressions;

namespace UW.Web.Services.GWSClient.Utils
{
    public class OSAgnosticCertUtils
    {
        public static X509Certificate2 GetCertFromFile(string path, string passwd)
        {
            if (!File.Exists(path))
            {
                throw new GWSClientConfigurationException($"{path} could not be found.");
            }
            var bytes = File.ReadAllBytes(path);
            var cert = new X509Certificate2(bytes, passwd);
            if (!cert.HasPrivateKey || cert.PrivateKey == null)
            {
                throw new CertPrivateKeyInaccessibleException($"Certificate at {path} does not have an accessible private key.");
            }
            return cert;
        }
    }

    public class WindowsX509CertUtils
    {
        /// <summary>
        /// Get the X509 cert with a specified common name from the local machine's certificate store.
        /// </summary>
        /// <param name="certCommonName">the common name of the certificate to get</param>
        /// <returns>the first valid certificate, or null if not found</returns>
        /// <exception cref="CertPrivateKeyInaccessibleException">if the certificate private key was not accessible</exception>
        /// <exception cref="ArgumentException">if the cert common name is null or empty</exception>
        public static X509Certificate2 GetCertFromLocalMachine(string certCommonName)
        {
            if (string.IsNullOrEmpty(certCommonName))
            {
                throw new ArgumentException("Certificate common name was null or empty string.");
            }

            using (var store = new X509Store(StoreLocation.LocalMachine))
            {
                try
                {
                    // error here on non-Windows:
                    // System.Security.Cryptography.CryptographicException:
                    // System.Security.Cryptography.CryptographicException: Unix LocalMachine X509Store is limited to the Root and CertificateAuthority stores.
                    // ---> System.PlatformNotSupportedException
                    store.Open(OpenFlags.ReadOnly);

                    foreach (var cert in store.Certificates)
                    {
                        if (GetCertCommonName(cert.Subject).Equals(certCommonName) && IsCurrent(cert))
                        {
                            if (!cert.HasPrivateKey || cert.PrivateKey == null)
                            {
                                throw new CertPrivateKeyInaccessibleException();
                            }
                            return cert;
                        }
                    }

                    return null;
                }
                finally
                {
                    store?.Close();
                }
            }
        }

        /// <summary>
        /// Determine whether or not a certificate's certificate is valid.
        /// </summary>
        /// <param name="cert">the certificate to check</param>
        /// <returns>true if it is, false otherwise</returns>
        public static bool IsCurrent(X509Certificate2 cert)
        {
            DateTime now = DateTime.Now;
            // Current date/time must on or after the start validity date and on or before the validity
            // end date.
            return cert.NotBefore.CompareTo(now) <= 0 && now.CompareTo(cert.NotAfter) <= 0;
        }

        /// <summary>
        /// Parse the common name out of the certificate subject.
        /// </summary>
        /// <param name="certSubjectName">the certificate subject</param>
        /// <returns>the common name</returns>
        public static string GetCertCommonName(string certSubjectName)
        {
            string pattern = @"(\s|^)[Cc][Nn]\s*=\s*[^(\,)]*";
            string certCommonName = "";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(certSubjectName))
            {
                certSubjectName = regex.Match(certSubjectName).ToString();

                // Get everything but the 'cn ='
                certCommonName = certSubjectName.Substring(certSubjectName.IndexOf("=") + 1,
                    certSubjectName.Length - 1 - certSubjectName.IndexOf("="));
            }

            return certCommonName;
        }
    }

    /// <summary>
    /// Thrown if a certificate's private key is inaccessible. This is often a result of the user needing
    /// to access the X.509 client certificate having insufficient permissions. Give the user
    /// 'Full Control' access to the files containing the keys the WSE will need to retrieve in the
    /// following folder: C:\Documents and Settings\All Users\Application Data\Microsoft\Crypto\RSA\MachineKeys
    /// </summary>
    public class CertPrivateKeyInaccessibleException : Exception
    {
        public CertPrivateKeyInaccessibleException()
            : base(
                "No private key found. This is often a result of the user needing to access " +
                "the X.509 client certificate having insufficient permissions. Give the user 'Full Control' " +
                "access to the files containing the keys the WSE will need to retrieve in the " +
                "following folder: " +
                @"C:\Documents and Settings\All Users\Application Data\Microsoft\Crypto\RSA\MachineKeys"
                )
        { }

        public CertPrivateKeyInaccessibleException(string message)
            : base(message)
        {

        }
    }
}
