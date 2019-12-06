using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    class GWSCreateGroupResponseData : GWSCreateGroupData
    {
        public string regid { get; set; }
        public long created { get; set; }
        public long lastModified { get; set; }
        public long lastMemberModified { get; set; }
        public int gid { get; set; }
        public IEnumerable<GWSAffiliate> affiliates { get; set; }
        public int authnfactor { get; set; }
    }

    class GWSAffiliate
    {
        public string name { get; set; }
        public string status { get; set; }
    }
}
