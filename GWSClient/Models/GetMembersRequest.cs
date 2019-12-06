namespace UW.Web.Services.GWSClient.Models
{
    public class GetMembersRequest
    {
        public GetMembersRequest()
        {
            BypassCache = false;
        }

        /// <summary>
        /// UW Group 
        /// </summary>
        public UWGroup Group { get; set; }

        /// <summary>
        /// Handle request via the registry.
        /// Default is false (handled by fast response cache)
        /// </summary>
        public bool BypassCache { get; set; }
    }
}
