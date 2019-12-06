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
        public async Task<RemoveMembersResult> RemoveMembers(RemoveMembersRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var members = string.Join(',', request.Remove);
            var url = baseUri
                .Append($"group/{request.Group}/member/{members}")
                .Params($"synchronized={request.Synchronized.ToString().ToLower()}");

            var response = await transport.DeleteAsync(url, cancellationToken).ConfigureAwait(false);
            return new RemoveMembersResult(response.StatusCode);
        }
    }
}