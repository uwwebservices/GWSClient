using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class AddMembersRequest
    {
        public string Group { get; }
        public IEnumerable<string> Members { get; }
        public bool Synchronized { get; set; } = false;

        public AddMembersRequest(string group, IEnumerable<UWGroupMember> members)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (members == null)
            {
                throw new ArgumentNullException(nameof(members));
            }
            if (!members.Any())
            {
                throw new ArgumentException($"{nameof(members)} must not be empty.");
            }
            Group = group;
            Members = members.Select(m => m.Id);
        }

        public AddMembersRequest(string group, IEnumerable<string> members)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (members == null)
            {
                throw new ArgumentNullException(nameof(members));
            }
            if (!members.Any())
            {
                throw new ArgumentException($"{nameof(members)} must not be empty.");
            }
            Group = group;
            Members = members;
        }
    }
}
