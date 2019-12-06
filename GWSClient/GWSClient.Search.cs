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
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient
{
    public partial class GWSClient
    {
        public async Task<SearchResult> Search(
            SearchRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var query = request.QueryString();

            var uri = baseUri.Append("search").Params(query);

            var response = await transport.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var results = JsonConvert.DeserializeObject<GWSSearchResponse>(str);
                return new SearchResult(response.StatusCode, results.data?.Select(d => new UWGroupReference
                {
                    Description = d.description,
                    Id = d.id,
                    Name = d.name,
                    RegID = d.regid,
                    Type = d.type
                }));
            }

            return new SearchResult(response.StatusCode);
        }
    }
}
