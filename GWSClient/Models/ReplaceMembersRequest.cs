using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class ReplaceMembersRequest
    {
        /// <summary>
        /// Group to modify membership.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// New membership.
        /// </summary>
        public IEnumerable<UWGroupMember> Members { get; }

        public static ReplaceMembersRequest Empty(string group)
        {
            return new ReplaceMembersRequest(group, new UWGroupMember[] { });
        }

        public ReplaceMembersRequest(string group, IEnumerable<string> memberIds, UWGroupMemberType memberType)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (memberIds == null)
            {
                throw new ArgumentNullException(nameof(memberIds));
            }
            Group = group;
            Members = memberIds.Select(m => new UWGroupMember { Id = m, MemberType = memberType });
        }

        public ReplaceMembersRequest(string group, IEnumerable<UWGroupMember> members)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (members == null)
            {
                throw new ArgumentNullException(nameof(members));
            }
            Group = group;
            Members = members;
        }
    }
}
