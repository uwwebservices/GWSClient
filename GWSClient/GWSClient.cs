using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UW.Web.Services.GWSClient.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace UW.Web.Services.GWSClient
{
    public partial class GWSClient : IGWSClient
    {
        readonly Uri baseUri;
        readonly int maxGWSBatchSize;
        readonly GWSHttpTransport transport;

        public GWSClient(IOptions<GWSClientOptions> opts, GWSHttpTransport transport)
            : this(transport, opts.Value.Url, opts.Value.MaxGWSBatchSize)
        {

        }

        public GWSClient(GWSHttpTransport transport, Uri uri, int maxGWSBatchSize)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (uri.Scheme != "https")
            {
                throw new ArgumentException($"{nameof(uri)} must be a valid https Uri.");
            }
            if (maxGWSBatchSize < 1)
            {
                throw new ArgumentException($"{nameof(maxGWSBatchSize)} must be a positive integer.");
            }
            this.transport = transport;
            baseUri = uri;
            this.maxGWSBatchSize = maxGWSBatchSize;
        }

        HttpContent GetPayload(object payload = null, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            if (payload != null)
            {
                var body = JsonConvert.SerializeObject(payload, settings);
                return new StringContent(body, Encoding.UTF8, "application/json");
            }
            return new StringContent(string.Empty, Encoding.UTF8, "application/json");
        }
    }

    /// <summary>
    /// The root GWSException, all exceptions custom to the GWSClient derive from this.
    /// </summary>
    public class GWSException : Exception
    {
        public GWSException()
        {
        }

        public GWSException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// An exception that indicates we tried calling GWS several times but could
    /// not get a response back.
    /// </summary>
    public class GWSUnreachableException : GWSException
    {
        public GWSUnreachableException()
        {
        }

        public GWSUnreachableException(string message) : base(message)
        {
        }
    }
}
