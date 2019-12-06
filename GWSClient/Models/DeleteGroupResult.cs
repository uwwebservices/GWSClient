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
    public class DeleteGroupResult
    {
        public string Group { get; }
        public DeleteGroupState State { get; }
        public bool Ok => State == DeleteGroupState.Ok;

        internal DeleteGroupResult(HttpStatusCode status, string group)
        {
            if (!Enums.TryStrictParse<DeleteGroupState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
            Group = group;
        }

        /// <summary>
        /// Enable truthy comparisons.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(DeleteGroupResult r)
        {
            return r.Ok;
        }
    }

    public enum DeleteGroupState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401
    }
}
