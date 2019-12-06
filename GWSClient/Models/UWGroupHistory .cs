using System;
using UW.Web.Services.GWSClient.Utils;

namespace UW.Web.Services.GWSClient.Models
{
    [Serializable]
    public class UWGroupHistory
    {
        /// <summary>
        /// User who performed the action
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Act-as user, if user acting as another
        /// </summary>
        public string ActAs { get; set; }

        /// <summary>
        /// Activity name
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// Details of the event
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unix Timestamp in milliseconds
        /// </summary>
        public long Timestamp { get; set; }

        public DateTime HistoryDate
        {
            get
            {
                return ClientUtilities.GetDateTimeFromUnixTimestamp(Timestamp);
            }
        }
    }
}
