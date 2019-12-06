namespace UW.Web.Services.GWSClient.Models
{
    class GWSCreateGroupResponse : GWSResponseBase
    {
        public GWSCreateGroupResponseData data { get; set; }
    }

    class GWSGetMembersResponse : GWSResponseBase
    {
        public GWSGroupMember[] data { get; set; }
    }

    class GWSGetEffectiveMembersResponse : GWSResponseBase
    {
        public GWSEffectiveGroupMember[] data { get; set; }
    }

    class GWSGetHistoryResponse : GWSResponseBase
    {
        public UWGroupHistory[] data { get; set; }
    }

    class GWSInfoResponse : GWSResponseBase
    {
        public UWGroup data { get; set; }
    }

    class GWSResponseBase
    {
        public string[] schemas { get; set; }
        public GWSResponseMeta meta { get; set; }
    }

    class GWSResponseMeta
    {
        public string resourceType { get; set; }
        public string version { get; set; }
        public string regid { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string selfRef { get; set; }
        public long timestamp { get; set; }
    }
}
