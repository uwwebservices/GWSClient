using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UW.Web.Services.GWSClient.Models
{
    public sealed class SearchRequest
    {
        public SearchDepth Depth { get; set; } = SearchDepth.One;
        public string Name { get; set; }
        public string Stem { get; set; }
        public string Member { get; set; }
        public UWGroupMemberType? Type { get; set; }
        public string Owner { get; set; }
        public string Affiliate { get; set; }
        public string Instructor { get; set; }
        public bool BypassCache { get; set; }

        internal string QueryString()
        {
            var props = GetType().GetProperties();
            var kvps = new List<KeyValuePair<string, string>>();
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var name = prop.Name.ToLower();
                    var val = prop.GetValue(this, null) as string;
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        kvps.Add(new KeyValuePair<string, string>(name, val));
                    }
                }
            }
            kvps.Add(new KeyValuePair<string, string>("depth", Depth.ToString().ToLower()));
            if (Type.HasValue)
            {
                kvps.Add(new KeyValuePair<string, string>("type", Type.Value.ToString().ToLower()));
            }
            if (BypassCache)
            {
                kvps.Add(new KeyValuePair<string, string>("source", "registry"));
            }
            return string.Join('&', kvps.Select(p => $"{p.Key}={p.Value}"));
        }
    }

    public enum SearchDepth : byte
    {
        One = 1,
        All = 2
    }
}
