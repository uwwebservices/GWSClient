using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Collections;

namespace UW.Web.Services.GWSClient.Models
{
    public class GetEffectiveMembersResult : IEnumerable<UWEffectiveGroupMember>
    {
        public GetEffectiveMembersState State { get; set; }
        public bool Ok => State == GetEffectiveMembersState.Ok;
        public IEnumerable<UWEffectiveGroupMember> Members { get; set; }

        internal GetEffectiveMembersResult(HttpStatusCode status, IEnumerable<UWEffectiveGroupMember> members = null)
        {
            if (!Enum.TryParse<GetEffectiveMembersState>(((int)status).ToString(), out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
            Members = members ?? new List<UWEffectiveGroupMember>();
        }

        /// <summary>
        /// Enable truthy comparisons.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(GetEffectiveMembersResult r)
        {
            return r.Ok;
        }

        public IEnumerator<UWEffectiveGroupMember> GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }
    }

    public enum GetEffectiveMembersState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401,
    }
}
