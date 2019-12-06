using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace UW.Web.Services.GWSClient.Models
{
    public class AddMembersResult
    {
        public PutMembersState State { get; set; }
        public List<string> NotSaved { get; set; }

        public bool Ok => State == PutMembersState.Ok;

        internal AddMembersResult(HttpStatusCode status, GWSError err)
        {
            NotSaved = err?.notFound ?? new List<string>();
            switch (status)
            {
                case HttpStatusCode.OK:
                    if (!NotSaved.Any())
                    {
                        State = PutMembersState.Ok;
                        return;
                    }
                    State = PutMembersState.Partial;
                    break;
                case HttpStatusCode.NotFound:
                    State = PutMembersState.NotFound;
                    break;
                case HttpStatusCode.Unauthorized:
                    State = PutMembersState.Disallowed;
                    break;
                default:
                    throw new NotSupportedException($"{status} is undocumented behavior.");
            }
        }

        public static implicit operator bool(AddMembersResult r)
        {
            return r.Ok;
        }
    }

    public enum PutMembersState
    {
        Ok,
        Partial,
        NotFound,
        Disallowed
    }
}