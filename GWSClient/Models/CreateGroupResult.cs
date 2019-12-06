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
    public class CreateGroupResult
    {
        internal IEnumerable<string> DetailMessages { get; set; }

        public CreateGroupState State { get; set; }
        public bool Ok => State == CreateGroupState.Ok;
        public UWGroup Group { get; set; }
        public IEnumerable<string> BadRequestDetails
        {
            get
            {
                if (State == CreateGroupState.BadRequest)
                {
                    return DetailMessages;
                }
                return new string[] { };
            }
        }

        internal CreateGroupResult(HttpStatusCode status, UWGroup group = null, IEnumerable<GWSError> errs = null)
        {
            if (!Enums.TryStrictParse<CreateGroupState>(status, out var state))
            {
                if (status != HttpStatusCode.Created)
                {
                    throw new NotSupportedException($"{status} is undocumented behavior.");
                }
                state = CreateGroupState.Ok;
            }
            State = state;
            Group = group;
            DetailMessages = errs?.SelectMany(e => e.detail);
        }

        public static implicit operator bool(CreateGroupResult r)
        {
            return r.Ok;
        }
    }

    public enum CreateGroupState
    {
        Ok = 200,
        BadRequest = 400,
        Disallowed = 401,
        Conflict = 412
    }
}
