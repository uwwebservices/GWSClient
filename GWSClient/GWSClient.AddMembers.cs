using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using UW.Web.Services.GWSClient.Models;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient
{
    public partial class GWSClient
    {
        public Task<AddMembersResult> AddMembers(AddMembersRequest req, CancellationToken cancellationToken = default)
        {
            return AddMemberImpl(req.Group, string.Join(',', req.Members), "AddMembers", req.Synchronized, cancellationToken);
        }

        public Task<AddMembersResult> AddMember(AddMemberRequest req, CancellationToken cancellationToken = default)
        {
            return AddMemberImpl(req.Group, req.Member, "AddMember", req.Synchronized, cancellationToken);
        }

        async Task<AddMembersResult> AddMemberImpl(string group, string member, string action, bool synchronized, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (string.IsNullOrWhiteSpace(member))
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentNullException(nameof(action));
            }

            var uri = baseUri
                .Append($"group/{group}/member/{member}")
                .Params($"synchronized={synchronized.ToString().ToLower()}");

            var response = await transport.PutAsync(uri, GetPayload(), cancellationToken).ConfigureAwait(false);
            return await MapPutMembersResult(response, action).ConfigureAwait(false);
        }

        async Task<AddMembersResult> MapPutMembersResult(HttpResponseMessage response, string action)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.NotFound:
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var errs = JsonConvert.DeserializeObject<GWSErrorResponse>(responseString);
                    return new AddMembersResult(response.StatusCode, errs.errors.FirstOrDefault());
                case HttpStatusCode.InternalServerError:
                    throw new GWSException($"GWS Internal Server Error.");
                default:
                    throw new GWSException($"{response.StatusCode} is undocumented behavior for the {action} action.");
            }
        }
    }
}