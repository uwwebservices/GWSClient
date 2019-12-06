using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using System.Collections;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    public class GetMembersResult : IEnumerable<UWGroupMember>
    {
        public GetMembersState State { get; set; }
        public bool Ok => State == GetMembersState.Ok;
        public IEnumerable<UWGroupMember> Members { get; set; }

        internal GetMembersResult(HttpStatusCode status, IEnumerable<UWGroupMember> members = null)
        {
            if (!Enums.TryStrictParse<GetMembersState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
            Members = members ?? new List<UWGroupMember>();
        }

        /// <summary>
        /// Enable truthy comparisons.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(GetMembersResult r)
        {
            return r.Ok;
        }

        public IEnumerator<UWGroupMember> GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }
    }

    public enum GetMembersState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401,
        NotSupported = 403
    }
}
