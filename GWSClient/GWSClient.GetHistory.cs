using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UW.Web.Services.GWSClient.Models;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient
{
    public partial class GWSClient
    {
        public async Task<GetHistoryResult> GetHistory(UWGroup group, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(group?.Id))
            {
                throw new ArgumentException($"Group must not be null and must have an Id.");
            }

            var uri = baseUri
                .Append($"group/{group.Id}/history")
                .Params($"activity=membership&order=a&start=0");

            var response = await transport.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = new GetHistoryResult(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                var historyResponse = JsonConvert.DeserializeObject<GWSGetHistoryResponse>(responseString);
                result.History = historyResponse.data;
            }

            return result;
        }
    }
}