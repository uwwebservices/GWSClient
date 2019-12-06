using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    public class RemoveMembersResult
    {
        public RemoveMembersState State { get; set; }
        public bool Ok => State == RemoveMembersState.Ok;

        internal RemoveMembersResult(HttpStatusCode status)
        {
            if (!Enums.TryStrictParse<RemoveMembersState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
        }

        public static implicit operator bool(RemoveMembersResult r)
        {
            return r.Ok;
        }
    }

    public enum RemoveMembersState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401
    }
}
