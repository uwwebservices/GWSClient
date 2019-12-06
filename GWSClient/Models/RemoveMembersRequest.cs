using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class RemoveMembersRequest
    {
        public string Group { get; }
        public IEnumerable<string> Remove { get; }
        public bool Synchronized { get; set; } = true;

        public RemoveMembersRequest(string group, IEnumerable<string> remove)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (remove == null)
            {
                throw new ArgumentNullException(nameof(remove));
            }
            if (!remove.Any())
            {
                throw new ArgumentException($"{remove} must not be empty.");
            }
            Group = group;
            Remove = remove;
        }

        public RemoveMembersRequest(string group, IEnumerable<UWGroupMember> remove)
            : this(group, remove?.Select(r => r.Id))
        {

        }

        public RemoveMembersRequest(string group, string remove)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (string.IsNullOrWhiteSpace(remove))
            {
                throw new ArgumentNullException(nameof(remove));
            }
            Group = group;
            Remove = new string[] { remove };
        }
    }
}
