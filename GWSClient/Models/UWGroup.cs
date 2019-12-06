using System;
using System.Collections.Generic;
using System.Linq;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    [Serializable]
    public class UWGroup
    {
        /// <summary>
        /// Unique, opaque idenfier for the group
        /// </summary>
        public string RegID { get; set; }

        /// <summary>
        /// ID of the group 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Descriptive name of the group
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Group's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Create timestamp (Unix Timestamp in milliseconds)
        /// </summary>
        public long Created { get; set; }

        /// <summary>
        /// Create DateTime
        /// </summary>
        public DateTime CreatedDateTime
        {
            get
            {
                return ClientUtilities.GetDateTimeFromUnixTimestamp(Created);
            }
        }

        /// <summary>
        /// Last modified timestamp (Unix Timestamp in milliseconds)
        /// </summary>
        public long LastModified { get; set; }

        /// <summary>
        /// Last Modified DateTime
        /// </summary>
        public DateTime LastModifiedDateTime
        {
            get
            {
                return ClientUtilities.GetDateTimeFromUnixTimestamp(LastModified);
            }
        }

        /// <summary>
        /// Last member modified timestamp (Unix Timestamp in milliseconds)
        /// </summary>
        public long LastMemberModified { get; set; }

        /// <summary>
        /// Last Member Modified DateTime
        /// </summary>
        public DateTime LastMemberModifiedDateTime
        {
            get
            {
                return ClientUtilities.GetDateTimeFromUnixTimestamp(LastMemberModified);
            }
        }

        /// <summary>
        /// Contact person (uwnetid) for the group
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Multi-factor authn required
        /// </summary>
        public int AuthnFactor { get; set; }

        /// <summary>
        /// Classification of membership. u=unclassified, r=restricted, c=confidential, missing=no classification
        /// </summary>
        public GroupClassification? Classification { get; set; }

        /// <summary>
        /// Membership dependency group
        /// </summary>
        public string DependsOn { get; set; }

        /// <summary>
        /// Unique int
        /// </summary>
        public int Gid { get; set; }

        /// <summary>
        /// Affiliations for the group
        /// </summary>
        public IEnumerable<Affiliate> Affiliates { get; set; }

        /// <summary>
        /// Entities with full group access
        /// </summary>
        public IEnumerable<UWGroupEntity> Admins { get; set; }

        /// <summary>
        /// Entities who can edit membership
        /// </summary>
        public IEnumerable<UWGroupEntity> Updaters { get; set; }

        /// <summary>
        /// Entities who can create sub-groups
        /// </summary>
        public IEnumerable<UWGroupEntity> Creators { get; set; }

        /// <summary>
        /// Entities who can read group membership
        /// </summary>
        public IEnumerable<UWGroupEntity> Readers { get; set; }

        /// <summary>
        /// Entities who can opt in to membership
        /// </summary>
        public IEnumerable<UWGroupEntity> Optins { get; set; }

        /// <summary>
        /// Entities who can opt out of membership
        /// </summary>
        public IEnumerable<UWGroupEntity> Optouts { get; set; }

        internal static IEnumerable<UWGroupEntity> Map(IEnumerable<GWSGroupEntity> ge)
        {
            if (ge == null) return null;
            return ge.Select(e => new UWGroupEntity { Id = e.id, Name = e.name, EntityType = Enum.Parse<UWGroupEntityType>(e.type, ignoreCase: true) });
        }
    }

    public class Affiliate
    {
        /// <summary>
        /// Affiliate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Activation status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Authorized senders
        /// </summary>
        public UWGroupEntity[] Senders { get; set; }
    }
}
