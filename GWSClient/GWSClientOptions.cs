using System;

namespace UW.Web.Services.GWSClient
{
    public class GWSClientOptions
    {
        public const string Section = "GWSClient";

        /// <summary>
        /// The name of the X509 client certificate 
        /// to use when calling the Group Web Service.
        /// Only use this if running on Windows and the
        /// certificate is installed on your machine.
        /// Otherwise use the CertificatePath/CertificatePathPassword combination.
        /// </summary>
        public string CertificateName { get; set; }

        /// <summary>
        /// The fully-qualified path of the X509 client certificate
        /// to use when calling the Group Web Service.
        /// Use this if running in non-Windows environments
        /// or the certificate is not installed on your Windows machine.
        /// </summary>
        public string CertificateFile { get; set; }

        /// <summary>
        /// The X509 client certificate's password.
        /// Only use this if your repository is private.
        /// </summary>
        public string CertificateFilePassword { get; set; }

        /// <summary>
        /// The environment variable containing your
        /// X509 client certificate's password.
        /// Prefer this.
        /// </summary>
        public string CertificateFilePasswordEnvKey { get; set; }

        /// <summary>
        /// The base url for the Group Web Service.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// The maximum number of calls to send to the
        /// Group Web Service as part of a single action,
        /// like deleting or fetching many groups.
        /// </summary>
        public int MaxGWSBatchSize { get; set; }
    }
}
