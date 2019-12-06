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
        public async Task<AddMembersResult> ReplaceMembers(
            ReplaceMembersRequest req,
            CancellationToken cancellationToken = default)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req));
            }

            var uri = baseUri.Append($"group/{req.Group}/member");

            var request = new GWSReplaceMembersRequest(req.Members);
            var content = GetPayload(request);
            var response = await transport.PutAsync(uri, content, cancellationToken).ConfigureAwait(false);

            return await MapPutMembersResult(response, "ReplaceMember").ConfigureAwait(false);
        }
    }
}