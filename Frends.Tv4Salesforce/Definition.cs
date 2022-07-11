#pragma warning disable 1591

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MTV.Tv4Salesforce
{
    /// <summary>
    /// Parameters class usually contains parameters that are required.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Directory of CSV file to be sent to Tv4Salesforce
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        public string DirectoryCSV { get; set; }

        /// <summary>
        /// Base domain URL used to send requests to Tv4Salesforce
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        public string BaseDomainURL { get; set; } 

        /// <summary>
        /// Authorization token 
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Body describing job settings
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        public string CreateJobBody { get; set; }
    }

    public class JobResult
    {
        public int StatusCode { get; set; }
        public string HttpResultBody { get; set; }
    }
}
