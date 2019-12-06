using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    public class GetInfoResult
    {
        public GetInfoState State { get; set; }

        public UWGroup Group { get; set; }

        public bool Ok => State == GetInfoState.Ok;

        internal GetInfoResult(HttpStatusCode status, UWGroup group)
        {
            if (!Enums.TryStrictParse<GetInfoState>(status, out var state))
            {
                throw new NotSupportedException($"{status} is undocumented behavior.");
            }
            State = state;
            Group = group;
        }

        public static implicit operator bool(GetInfoResult r)
        {
            return r.Ok;
        }
    }

    public enum GetInfoState
    {
        Ok = 200,
        NotFound = 404,
    }
}