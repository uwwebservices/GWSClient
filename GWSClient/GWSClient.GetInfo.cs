using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UW.Web.Services.GWSClient.Models;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient
{
    public partial class GWSClient
    {
        public async Task<IEnumerable<GetInfoResult>> GetInfo(GetInfosRequest req, CancellationToken cancellationToken = default)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req));
            }

            if (req.Groups == null || !req.Groups.Any())
            {
                throw new ArgumentNullException(nameof(req.Groups));
            }

            var results = new ConcurrentBag<GetInfoResult>();

            //Fanout to get all the groups
            var skip = 0;
            var groupsToGetInfo = req.Groups.Take(maxGWSBatchSize);
            while (groupsToGetInfo.Any())
            {
                var tasks = new List<Task>();

                foreach (var group in groupsToGetInfo)
                {
                    var process = Task.Run(async () =>
                    {
                        var result = await GetInfo(group, cancellationToken).ConfigureAwait(false);
                        results.Add(result);
                    }, cancellationToken);

                    tasks.Add(process);
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                skip += maxGWSBatchSize;
                groupsToGetInfo = req.Groups.Skip(skip).Take(maxGWSBatchSize);
            }

            return results;
        }

        public async Task<GetInfoResult> GetInfo(string group, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(nameof(group));
            }

            var uri = baseUri.Append($"group/{group}");

            var response = await transport.GetAsync(uri, cancellationToken).ConfigureAwait(false);

            return await MapGetInfoResult(group, response).ConfigureAwait(false);
        }


        async Task<GetInfoResult> MapGetInfoResult(string group, HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var infoResponse = JsonConvert.DeserializeObject<GWSInfoResponse>(responseString);
                return new GetInfoResult(response.StatusCode, infoResponse.data);
            }

            return new GetInfoResult(response.StatusCode, new UWGroup { Id = group });
        }
    }
}