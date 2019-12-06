using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class CreateGroupRequest
    {
        public string Id { get; set; }
        public string Contact { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public GroupClassification? Classification { get; set; }
        public IEnumerable<UWGroupEntity> Admins { get; set; }
        public IEnumerable<UWGroupEntity> Readers { get; set; }
        public IEnumerable<UWGroupEntity> Updaters { get; set; }
        public IEnumerable<UWGroupEntity> Creators { get; set; }
        public IEnumerable<UWGroupEntity> Optins { get; set; }
        public IEnumerable<UWGroupEntity> Optouts { get; set; }
        public string DependsOn { get; set; }
        public bool Synchronized { get; set; } = true;
    }

    public enum GroupClassification : short
    {
        Unclassified,
        Restricted,
        Confidential,
    }
}
