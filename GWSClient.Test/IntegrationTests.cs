using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UW.Web.Services.GWSClient.Models;
using UW.Web.Services.GWSClient.Utils;
using System;

namespace UW.Web.Services.GWSClient.Test
{
    [TestClass]
    public class IntegrationTests
    {
        private static IGWSClient client;

        [ClassInitialize]
        public static void Startup(TestContext _)
        {
            var opts = Options.Create(new GWSClientOptions
            {
                CertificateName = "aisdev.cac.washington.edu",
                Url = new Uri("https://groups.uw.edu/group_sws/v3"),
                MaxGWSBatchSize = 10
            });
            var transport = new GWSHttpTransport(WindowsX509CertUtils.GetCertFromLocalMachine(opts.Value.CertificateName));
            client = new GWSClient(opts, transport);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetMembers()
        {
            var myGroup = new UWGroup() { Id = "uw_ais_sm_ews" };

            var getMembersRequest = new GetMembersRequest()
            {
                Group = myGroup,
                BypassCache = false
            };

            var memberResponse = await client.GetMembers(getMembersRequest);

            Assert.IsNotNull(memberResponse);
            Assert.IsTrue(memberResponse);
            Assert.IsTrue(memberResponse.Any());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetEffectiveMembers()
        {
            var myGroup = new UWGroup() { Id = "uw_ais_sm_ews" };

            var getMembersRequest = new GetMembersRequest()
            {
                Group = myGroup,
                BypassCache = false
            };

            var memberResponse = await client.GetEffectiveMembers(getMembersRequest);

            Assert.IsNotNull(memberResponse);
            Assert.IsTrue(memberResponse);
            Assert.IsTrue(memberResponse.Any());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetMembers_GroupNotFound()
        {
            var myGroup = new UWGroup() { Id = "thisisnotarealgroupid" };

            var getMembersRequest = new GetMembersRequest()
            {
                Group = myGroup,
                BypassCache = false
            };

            var memberResponse = await client.GetMembers(getMembersRequest);

            Assert.IsNotNull(memberResponse);
            Assert.IsFalse(memberResponse);
            Assert.AreEqual(GetMembersState.NotFound, memberResponse.State);
        }


        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetHistory()
        {
            var myGroup = new UWGroup() { Id = "uw_ais_sm_ews" };

            var getHistoryResult = await client.GetHistory(myGroup);

            Assert.IsNotNull(getHistoryResult);
            Assert.IsTrue(getHistoryResult);
            Assert.IsTrue(getHistoryResult.Any());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetHistory_NotFound()
        {
            var myGroup = new UWGroup() { Id = "thisisnotarealgroupid" };

            var getHistoryResult = await client.GetHistory(myGroup);

            Assert.IsNotNull(getHistoryResult);
            Assert.IsFalse(getHistoryResult);
            Assert.IsNull(getHistoryResult.History);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetInfo_Single()
        {
            var infoResult = await client.GetInfo("uw_ais_sm_ews");

            Assert.IsNotNull(infoResult);
            Assert.IsTrue(infoResult.Ok);
            Assert.IsNotNull(infoResult.Group);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetInfo()
        {
            var groups = new List<string>
            {
                "uw_ais_sm_ews",
                "uw_ais_sm_ews_hrp",
                "uw_ais_sm_ews_sws"
            };

            var req = new GetInfosRequest(groups);

            var results = await client.GetInfo(req);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.All(x=> x.Ok));
            Assert.AreEqual(groups.Count, results.Count());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetInfo_Partial()
        {
            var groups = new List<string>
            {
                "uw_ais_sm_ews",
                "blahblahblah"
            };

            var req = new GetInfosRequest(groups);

            var results = await client.GetInfo(req);

            Assert.IsNotNull(results);
            Assert.IsFalse(results.All(x => x.Ok));
            Assert.AreEqual(groups.Count, results.Count());
            Assert.AreEqual(1, results.Where(x=>x.State == GetInfoState.NotFound).Count());
            Assert.AreEqual(1, results.Where(x => x.State == GetInfoState.Ok).Count());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_GetInfo_NotFound()
        {
            var groups = new List<string>
            {
                "blahblahblah"
            };

            var req = new GetInfosRequest(groups);

            var results = await client.GetInfo(req);

            Assert.IsNotNull(results);
            Assert.AreEqual(GetInfoState.NotFound, results.FirstOrDefault().State);
            Assert.IsFalse(results.FirstOrDefault().Ok);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_Search()
        {
            var request = new SearchRequest
            {
                Stem = "uw_ais_sm"
            };

            var result = await client.Search(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_Create()
        {
            var group = "uw_ais_sm_ews_ews-gwsclient-test-group";
            var displayName = "EWS GWS Client test group";
            var description = "blah";
            var admins = new List<UWGroupEntity>()
            {
                new UWGroupEntity()
                {
                    Id = "ssonger",
                    EntityType = UWGroupEntityType.UWNetID,
                    Name = ""
                }
            };

            var req = new CreateGroupRequest
            {
                Id = group,
                DisplayName = displayName,
                Description = description,
                Admins = admins
            };

            var result = await client.Create(req);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_Delete()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";

            var result = await client.Delete(groupId);

            Assert.IsTrue(result);
            Assert.AreEqual(groupId, result.Group);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_AddMembers()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";
            var member1 = "pprestin";
            var member2 = "mannkind";
            var memberIds = new List<string>() { member1, member2 };

            var success = await client.AddMembers(new AddMembersRequest(groupId, memberIds));

            Assert.IsTrue(success);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_RemoveMembers()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";
            var member1 = "pprestin";
            var member2 = "mannkind";
            var memberIds = new List<string>() { member1, member2 };

            var result = await client.RemoveMembers(new RemoveMembersRequest(groupId, memberIds));

            Assert.IsTrue(result);
            Assert.AreEqual(RemoveMembersState.Ok, result.State);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_ReplaceMembers()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";
            var member1 = "pprestin";
            var member2 = "mannkind";
            var memberIds = new List<string>() { member1, member2 };

            var success = await client.ReplaceMembers(new ReplaceMembersRequest(groupId, memberIds, UWGroupMemberType.UWNetID));

            Assert.IsTrue(success);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_ReplaceMembersStronglyTyped()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";
            var members = new List<UWGroupMember>
            {
                new UWGroupMember() { Id = "cspital", MemberType = UWGroupMemberType.UWNetID },
                new UWGroupMember() { Id = "ssonger", MemberType = UWGroupMemberType.UWNetID }
            };

            var success = await client.ReplaceMembers(new ReplaceMembersRequest(groupId, members));

            Assert.IsTrue(success);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GWSClient_ClearOutGroup()
        {
            var groupId = "uw_ais_sm_ews_ews-gwsclient-test-group";

            var success = await client.ReplaceMembers(ReplaceMembersRequest.Empty(groupId));

            Assert.IsTrue(success);
        }
    }
}
