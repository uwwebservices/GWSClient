using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    public class GetHistoryResult : IEnumerable<UWGroupHistory>
    {
        public GetHistoryState State { get; set; }
        public bool Ok => State == GetHistoryState.Ok;

        public IEnumerable<UWGroupHistory> History { get; set; }

        internal GetHistoryResult(HttpStatusCode status)
        {
            if (!Enums.TryStrictParse<GetHistoryState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
        }

        /// <summary>
        /// Enable truthy comparisons.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator bool(GetHistoryResult r)
        {
            return r.Ok;
        }

        public IEnumerator<UWGroupHistory> GetEnumerator()
        {
            return History.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return History.GetEnumerator();
        }
    }

    public enum GetHistoryState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401
    }
}
