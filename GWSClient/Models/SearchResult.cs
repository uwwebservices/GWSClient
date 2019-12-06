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
    public class SearchResult : IEnumerable<UWGroupReference>
    {
        public SearchState State { get; set; }
        public IEnumerable<UWGroupReference> Refs { get; set; }
        public bool Ok => State == SearchState.Ok;

        internal SearchResult(HttpStatusCode status, IEnumerable<UWGroupReference> refs = null)
        {
            if (!Enums.TryStrictParse<SearchState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }

            State = state;
            Refs = refs ?? new List<UWGroupReference>();
        }

        public static implicit operator bool(SearchResult r)
        {
            return r.Ok;
        }

        public IEnumerator<UWGroupReference> GetEnumerator()
        {
            return Refs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Refs.GetEnumerator();
        }
    }

    public enum SearchState
    {
        Ok = 200,
        NotFound = 404,
        Disallowed = 401
    }
}
