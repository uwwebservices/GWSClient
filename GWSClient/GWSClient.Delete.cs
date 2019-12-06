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
        public async Task<DeleteGroupResult> Delete(string group, bool synchronized = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }

            var url = baseUri
                        .Append($"group/{group}")
                        .Params($"synchronized={synchronized.ToString().ToLower()}");

            var response = await transport.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            return new DeleteGroupResult(response.StatusCode, group);
        }

        public async Task<IEnumerable<DeleteGroupResult>> Delete(DeleteGroupsRequest req, CancellationToken cancellationToken = default)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req));
            }

            var results = new ConcurrentBag<DeleteGroupResult>();

            //Fanout to delete all the groups
            var skip = 0;
            var groupsToDelete = req.Groups.Take(maxGWSBatchSize);
            while (groupsToDelete.Any())
            {
                var tasks = new List<Task>();

                foreach (var group in groupsToDelete)
                {
                    var process = Task.Run(async () =>
                    {
                        var result = await Delete(group, req.Synchronized, cancellationToken).ConfigureAwait(false);
                        results.Add(result);
                    }, cancellationToken);

                    tasks.Add(process);
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                skip += maxGWSBatchSize;
                groupsToDelete = req.Groups.Skip(skip).Take(maxGWSBatchSize);
            }

            return results;
        }
    }
}