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
        public async Task<CreateGroupResult> Create(
            CreateGroupRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var body = new GWSCreateGroupRequest
            {
                data = new GWSCreateGroupData
                {
                    id = request.Id,
                    contact = request.Contact,
                    displayName = request.DisplayName,
                    description = request.Description,
                    classification = GWSCreateGroupData.Classify(request.Classification),
                    admins = GWSCreateGroupData.Map(request.Admins),
                    readers = GWSCreateGroupData.Map(request.Readers),
                    updaters = GWSCreateGroupData.Map(request.Updaters),
                    creators = GWSCreateGroupData.Map(request.Creators),
                    optins = GWSCreateGroupData.Map(request.Optins),
                    optouts = GWSCreateGroupData.Map(request.Optouts),
                    dependsOn = request.DependsOn
                }
            };
            var content = GetPayload(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var url = baseUri
                .Append($"group/{request.Id}")
                .Params($"synchronized={request.Synchronized.ToString().ToLower()}");

            var response = await transport.PutAsync(url, content, cancellationToken).ConfigureAwait(false);
            var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<GWSCreateGroupResponse>(str).data;
                var group = new UWGroup
                {
                    RegID = data.regid,
                    Id = data.id,
                    DisplayName = data.displayName,
                    Description = data.description,
                    Contact = data.contact,
                    Created = data.created,
                    LastModified = data.lastModified,
                    LastMemberModified = data.lastMemberModified,
                    AuthnFactor = data.authnfactor,
                    Classification = GWSCreateGroupData.Classify(data),
                    DependsOn = data.dependsOn,
                    Gid = data.gid,
                    Affiliates = data.affiliates?.Select(a => new Affiliate { Name = a.name, Status = a.status }),
                    Admins = UWGroup.Map(data.admins),
                    Updaters = UWGroup.Map(data.updaters),
                    Creators = UWGroup.Map(data.creators),
                    Readers = UWGroup.Map(data.readers),
                    Optins = UWGroup.Map(data.optins),
                    Optouts = UWGroup.Map(data.optouts),
                };

                return new CreateGroupResult(response.StatusCode, group);
            }

            var err = JsonConvert.DeserializeObject<GWSErrorResponse>(str).errors;
            return new CreateGroupResult(response.StatusCode, null, err);
        }
    }
}