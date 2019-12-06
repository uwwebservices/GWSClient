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
        public async Task<GetMembersResult> GetMembers(GetMembersRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request?.Group?.Id))
            {
                throw new ArgumentException($"{nameof(request)} must not be null and have a Group with an Id.");
            }

            var uri = baseUri.Append($"group/{request.Group.Id}/member");
            if (request.BypassCache)
            {
                uri = uri.Params("source=registry");
            }

            var response = await transport.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
           
            if (response.IsSuccessStatusCode)
            {
                var memberResponse = JsonConvert.DeserializeObject<GWSGetMembersResponse>(responseString);
                return new GetMembersResult(response.StatusCode, memberResponse.data?.Select(d => new UWGroupMember(d)));
            }

            return new GetMembersResult(response.StatusCode);
        }

        public async Task<GetEffectiveMembersResult> GetEffectiveMembers(GetMembersRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request?.Group?.Id))
            {
                throw new ArgumentException($"{nameof(request)} must not be null and have a Group with an Id.");
            }

            var uri = baseUri.Append($"group/{request.Group.Id}/effective_member");
            if (request.BypassCache)
            {
                uri = uri.Params("source=registry");
            }

            var response = await transport.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var members = JsonConvert.DeserializeObject<GWSGetEffectiveMembersResponse>(responseString);
                return new GetEffectiveMembersResult(response.StatusCode, members.data?.Select(d => new UWEffectiveGroupMember(d)));
            }
            return new GetEffectiveMembersResult(response.StatusCode);
        }
    }
}