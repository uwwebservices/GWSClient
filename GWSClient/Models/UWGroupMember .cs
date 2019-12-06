using System;

namespace UW.Web.Services.GWSClient.Models
{
    /// <summary>
    /// Type of group member
    /// </summary>
    public enum UWGroupMemberType
    {
        UWNetID = 1,
        Group = 2 ,
        DNS = 3,
        EPPN = 4,
        UWWI = 5
    }

    [Serializable]
    public class UWGroupMember
    {
        /// <summary>
        /// Type of group member
        /// </summary>
        public UWGroupMemberType MemberType { get; set; }

        public string Type
        {
            get
            {
                return MemberType.ToString().ToLower();
            }
        }

        /// <summary>
        /// Entity id of member
        /// </summary>
        public string Id { get; set; }


        public UWGroupMember()
        {

        }

        internal UWGroupMember(GWSGroupMember gm)
        {
            MemberType = Enum.Parse<UWGroupMemberType>(gm.type, ignoreCase: true);
            Id = gm.id;
        }
    }

    [Serializable]
    class GWSGroupMember
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    [Serializable]
    public class UWEffectiveGroupMember
    {
        /// <summary>
        /// Type of group member
        /// </summary>
        public UWGroupMemberType MemberType { get; set; }

        public string Type
        {
            get
            {
                return MemberType.ToString().ToLower();
            }
        }

        /// <summary>
        /// Entity id of member
        /// </summary>
        public string Id { get; set; }

        public UWGroupMembershipType MembershipType { get; set; }

        public string MType
        {
            get
            {
                return MembershipType.ToString().ToLower();
            }
        }

        public string Source { get; set; }

        public UWEffectiveGroupMember()
        {

        }

        internal UWEffectiveGroupMember(GWSEffectiveGroupMember egm)
        {
            MemberType = Enum.Parse<UWGroupMemberType>(egm.type, ignoreCase: true);
            Id = egm.id;
            MembershipType = Enum.Parse<UWGroupMembershipType>(egm.mtype, ignoreCase: true);
            Source = egm.source;
        }
    }

    [Serializable]
    class GWSEffectiveGroupMember : GWSGroupMember
    {
        public string mtype { get; set; }
        public string source { get; set; }
    }

    public enum UWGroupMembershipType
    {
        Direct,
        Indirect
    }
}
