using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace UW.Web.Services.GWSClient
{
    public class GWSHttpTransport : HttpClient
    {
        public GWSHttpTransport(X509Certificate2 credential)
            : base(EWSSocketsHttpHandler.Get(credential))
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    static class EWSSocketsHttpHandler
    {
        public static void SetupServicePointManager()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.DefaultConnectionLimit = 300;
            ServicePointManager.SetTcpKeepAlive(true, 10000, 1000);
            ServicePointManager.EnableDnsRoundRobin = true;
        }

        public static SocketsHttpHandler Get(X509Certificate2 credential)
        {
            var handler = new SocketsHttpHandler()
            {
                SslOptions = new SslClientAuthenticationOptions()
            };
            handler.SslOptions.ClientCertificates = new X509CertificateCollection(new X509Certificate2[] { credential });
            handler.SslOptions.RemoteCertificateValidationCallback = (sender, cert, chain, errs) => true;
            handler.AllowAutoRedirect = true;

            SetupServicePointManager();

            return handler;
        }
    }
}
