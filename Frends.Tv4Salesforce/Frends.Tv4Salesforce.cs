using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp; // You can remove this if you don't need dynamic type in .NET Standard frends Tasks
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace Frends.Tv4Salesforce
{
    public static class BulkJob
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// This is task
        /// Documentation: https://github.com/GlazowskiHiQ/Frends.Tv4Salesforce
        /// </summary>
        public static async Task<Result> Sales(Parameters input, CancellationToken cancellationToken)
        {
            byte[] fileBytes = ReadBytesFrom(input.DirectoryCSV);
            await JobOperation(OperationType.CREATE_JOB, input.AuthToken, input.BaseDomainURL, input.CreateJobBody);
            return null;
        }

        private static byte[] ReadBytesFrom(string directory)
        {
            return File.ReadAllBytes(directory);
        }

        public enum OperationType
        {
            CREATE_JOB, PUT_CSV, CLOSE_JOB, GET_JOB_INFO
        }

        private static async Task<JobResult> JobOperation
            (OperationType operation, 
            string authToken, 
            string domain, 
            string creationBody = null, 
            string contentUrl = null, 
            byte[] byteContent = null, 
            string jobId = null)
        {
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var request = new HttpRequestMessage { };

            switch (operation)
            {
                case OperationType.CREATE_JOB:
                    request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($@"{domain}/services/data/v55.0/jobs/ingest/"),
                        Content = new StringContent(creationBody, Encoding.UTF8, "application/json")
                    };
                    break;
                case OperationType.PUT_CSV:
                    request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri($@"{domain}/{contentUrl}"),
                        Content = new ByteArrayContent(byteContent)
                    };
                    break;
                case OperationType.CLOSE_JOB:
                    request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("PATCH"),
                        RequestUri = new Uri($@"{domain}/{contentUrl}"),
                        Content = new StringContent("{ \"state\":\"UploadComplete\" }", Encoding.UTF8, "application/json")
                    };
                    break;
                case OperationType.GET_JOB_INFO:
                    request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($@"{domain}//services/data/v55.0/jobs/ingest/{jobId}")
                    };
                    break;
            };

            var response = await client.SendAsync(request).ConfigureAwait(false);

            string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            JobResult result = new JobResult
            {
                StatusCode = (int)response.StatusCode,
                HttpResultBody = responseBody
            };
            return result;
        }
    }
}
