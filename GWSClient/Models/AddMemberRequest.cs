using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public class AddMemberRequest
    {
        string group;
        /// <summary>
        /// Group to add member to.
        /// </summary>
        public string Group
        {
            get => group;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                group = value;
            }
        }
        string member;
        /// <summary>
        /// Member to add to group.
        /// </summary>
        public string Member
        {
            get => member;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                member = value;
            }
        }
        /// <summary>
        /// Wait until the member has been added before returning? (default: false)
        /// </summary>
        public bool Synchronized { get; set; } = false;
    }
}
