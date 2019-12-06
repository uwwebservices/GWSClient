using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class DeleteGroupsRequest
    {
        public IEnumerable<string> Groups { get; }
        public bool Synchronized { get; set; } = false;

        public DeleteGroupsRequest(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentNullException(nameof(group));
            }
            Groups = new string[] { group };
        }

        public DeleteGroupsRequest(IEnumerable<string> groups)
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
