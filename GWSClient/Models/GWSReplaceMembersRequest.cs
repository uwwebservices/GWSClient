using System.Collections.Generic;

namespace UW.Web.Services.GWSClient.Models
{
    class GWSReplaceMembersRequest
    {
        public GWSReplaceMembersRequest(IEnumerable<UWGroupMember> members)
        {
            data = new List<GWSReplaceMembersBody>();

            foreach (var member in members)
            {
                data.Add(new GWSReplaceMembersBody() { type = member.Type, id = member.Id });
            }
        }

        public List<GWSReplaceMembersBody> data { get; set; }
    }

    class GWSReplaceMembersBody
    {
        public string type { get; set; }
        public string id { get; set; }
    }
}
