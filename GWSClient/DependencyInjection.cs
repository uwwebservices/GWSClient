using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient
{
    /// <summary>
    /// Extension so that it's easy to add the GWSClient to your project's Startup class:
    /// Install.GWSClient();
    /// </summary>
    public static class Install
    {
        public static bool GWSClient(IServiceCollection services, IConfiguration config)
        {
            var gwsClientSection = config.GetSection(GWSClientOptions.Section);
            if (!gwsClientSection.Exists())
            {
                return false;
            }

            var certData = GetCertificateData(gwsClientSection);

            var root = gwsClientSection.GetValue<string>(nameof(GWSClientOptions.Url));
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.Url)} is required.");
            }
            if (!Uri.TryCreate(root, UriKind.Absolute, out var uri) || uri.Scheme != "https")
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.Url)} must be a valid https URI.");
            }

            var maxBatch = gwsClientSection.GetValue<int>(nameof(GWSClientOptions.MaxGWSBatchSize));
            if (maxBatch < 1) // need a max for safety?
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.MaxGWSBatchSize)} must be a positive integer.");
            }

            // setup the options
            services.Configure<GWSClientOptions>(opts =>
            {
                opts.CertificateName = certData.CertName;
                opts.CertificateFile = certData.CertFile;
                opts.CertificateFilePassword = certData.CertPassword;
                opts.CertificateFilePasswordEnvKey = certData.CertPasswordEnvKey;
                opts.Url = uri;
                opts.MaxGWSBatchSize = maxBatch;
            });

            var cert = GetX509Certificate2(certData);
            services.AddSingleton<GWSHttpTransport>(new GWSHttpTransport(cert));
            services.AddSingleton<IGWSClient, GWSClient>();

            return true;
        }

        static CertificateData GetCertificateData(IConfigurationSection gws)
        {
            var certName = gws.GetValue<string>(nameof(GWSClientOptions.CertificateName));
            var certPath = gws.GetValue<string>(nameof(GWSClientOptions.CertificateFile));

            if (string.IsNullOrWhiteSpace(certName) ^ string.IsNullOrWhiteSpace(certPath))
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.CertificateName)} and {GWSClientOptions.Section}:{nameof(GWSClientOptions.CertificateFile)} are mutually exclusive, provide one or the other.");
            }

            if (!string.IsNullOrWhiteSpace(certName))
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    throw new GWSClientConfigurationException($"Certificate store is not available on non-Windows platforms, please provide certificate file and password instead.");
                }
                return new CertificateData
                {
                    CertificateSource = CertificateSource.Store,
                    CertName = certName
                };
            }

            var certPass = gws.GetValue<string>(nameof(GWSClientOptions.CertificateFilePassword));
            var certPassKey = gws.GetValue<string>(nameof(GWSClientOptions.CertificateFilePasswordEnvKey));
            if (string.IsNullOrWhiteSpace(certPass) && string.IsNullOrWhiteSpace(certPassKey))
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.CertificateFilePassword)} or {GWSClientOptions.Section}:{nameof(GWSClientOptions.CertificateFilePasswordEnvKey)} are required.");
            }

            if (!string.IsNullOrWhiteSpace(certPass))
            {
                return new CertificateData
                {
                    CertificateSource = CertificateSource.File,
                    CertFile = certPath,
                    CertPassword = certPass
                };
            }

            certPass = Environment.GetEnvironmentVariable(certPassKey);
            if (string.IsNullOrWhiteSpace(certPass))
            {
                throw new GWSClientConfigurationException($"{GWSClientOptions.Section}:{nameof(GWSClientOptions.CertificateFilePasswordEnvKey)} value not found.");
            }

            return new CertificateData
            {
                CertificateSource = CertificateSource.File,
                CertFile = certPath,
                CertPassword = certPass,
                CertPasswordEnvKey = certPassKey
            };
        }

        static X509Certificate2 GetX509Certificate2(CertificateData data)
        {
            switch (data.CertificateSource)
            {
                case CertificateSource.Store:
                    return WindowsX509CertUtils.GetCertFromLocalMachine(data.CertName);
                case CertificateSource.File:
                    return OSAgnosticCertUtils.GetCertFromFile(data.CertFile, data.CertPassword);
                default:
                    throw new NotSupportedException($"{data.CertificateSource} is unsupported.");
            }
        }

        struct CertificateData
        {
            public CertificateSource CertificateSource { get; set; }
            public string CertName { get; set; }
            public string CertFile { get; set; }
            public string CertPassword { get; set; }
            public string CertPasswordEnvKey { get; set; }
        }

        enum CertificateSource
        {
            Store,
            File
        }
    }

    public static class GWSClientExtensions
    {
        public static bool TryInstallGWSClient(this IServiceCollection services, IConfiguration config)
        {
            if (config == null) return false;

            return Install.GWSClient(services, config);
        }
    }

    public class GWSClientConfigurationException : GWSException
    {
        public GWSClientConfigurationException()
        {
        }

        public GWSClientConfigurationException(string message) : base(message)
        {
        }
    }
}
