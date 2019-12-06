using System.Collections.Generic;

namespace UW.Web.Services.GWSClient.Models
{
    class GWSSearchResponse : GWSResponseBase
    {
        public List<GWSGroupReference> data { get; set; }
    }

    class GWSGroupReference
    {
        public string id { get; set; }
        public string type { get; set; }
        public string regid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
