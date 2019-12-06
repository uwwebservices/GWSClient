using System;
using System.Collections.Generic;
using System.Linq;

namespace UW.Web.Services.GWSClient.Models
{
    class GWSCreateGroupRequest
    {
        public GWSCreateGroupData data { get; set; }
    }

    class GWSCreateGroupData
    {
        public string id { get; set; }
        public string contact { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public IEnumerable<GWSGroupEntity> admins { get; set; }
        public IEnumerable<GWSGroupEntity> readers { get; set; }
        public IEnumerable<GWSGroupEntity> updaters { get; set; }
        public IEnumerable<GWSGroupEntity> creators { get; set; }
        public IEnumerable<GWSGroupEntity> optins { get; set; }
        public IEnumerable<GWSGroupEntity> optouts { get; set; }
        public string dependsOn { get; set; }

        internal static IEnumerable<GWSGroupEntity> Map(IEnumerable<UWGroupEntity> uw)
        {
            if (uw == null) return null;
            return uw
                .Where(u => u != null)
                .Select(u => new GWSGroupEntity { id = u.Id, name = u.Name, type = u.Type });
        }

        internal static string Classify(GroupClassification? gc)
        {
            if (!gc.HasValue)
            {
                return null;
            }
            switch(gc.Value)
            {
                case GroupClassification.Unclassified:
                    return "u";
                case GroupClassification.Restricted:
                    return "r";
                case GroupClassification.Confidential:
                    return "c";
                default:
                    throw new NotSupportedException($"{gc.Value} is an invalid {typeof(GroupClassification)}");
            }
        }

        internal static GroupClassification? Classify(GWSCreateGroupResponseData c)
        {
            if (string.IsNullOrWhiteSpace(c?.classification))
            {
                return null;
            }
            if (Enum.TryParse<GroupClassification>(c.classification, out var cls))
            {
                return cls;
            }
            return null;
        }
    }

    class GWSGroupEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }
}
