using System;
using System.Collections.Generic;
using System.Linq;

namespace UW.Web.Services.GWSClient.Models
{
    public class GetInfosRequest
    {
        public IEnumerable<string> Groups { get; }

        public GetInfosRequest(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            Groups = new string[] { group };
        }

        public GetInfosRequest(IEnumerable<string> groups)
        {
            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }
            if (!groups.Any())
            {
                throw new ArgumentException($"{nameof(groups)} must not be empty.");
            }
            Groups = groups.Where(g => !string.IsNullOrWhiteSpace(g));
        }
    }
}
