using System;

namespace UW.Web.Services.GWSClient.Models
{
    [Serializable]
    public class UWGroupReference
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Group RegID
        /// </summary>
        public string RegID { get; set; }

        /// <summary>
        /// Group Display Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }
    }
}
