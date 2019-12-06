using System.Collections.Generic;

namespace UW.Web.Services.GWSClient.Models
{
    class GWSErrorResponse
    {
        public List<string> schemas { get; set; }
        public GWSErrorMeta meta { get; set; }
        public List<GWSError> errors { get; set; }
    }

    class GWSErrorMeta
    {
        public string resourceType { get; set; }
        public string version { get; set; }
        public long timestamp { get; set; }
    }

    class GWSError
    {
        public int status { get; set; }
        public List<string> detail { get; set; }
        public List<string> notFound { get; set; }
    }
}
