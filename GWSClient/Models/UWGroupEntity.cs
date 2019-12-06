using System;

namespace UW.Web.Services.GWSClient.Models
{
    /// <summary>
    /// Type of group entity
    /// </summary>
    public enum UWGroupEntityType
    {
        UWNetID = 1,
        Group = 2,
        DNS = 3,
        EPPN = 4,
        Set = 5
    }

    [Serializable]
    public class UWGroupEntity
    {
        /// <summary>
        /// Type of group entity
        /// </summary>
        public UWGroupEntityType EntityType { get; set; }

        public string Type
        {
            get
            {
                return EntityType.ToString().ToLower();
            }
        }

        /// <summary>
        /// Display name of entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the type is 'set' the id is:
        ///   * all: any entity
        ///   * none: no entity
        ///   * uw: any UW member entity
        ///   * member: any member of the group
        /// </summary>
        public string Id { get; set; }
    }
}
