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

namespace MTV.Tv4Salesforce
{
    public static class BulkJob
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// This task handles Tv4Salesforce bulk jobs. Compatible with both insert and upsert. 
        /// Documentation: https://github.com/GlazowskiHiQ/Frends.Tv4Salesforce\
        /// Task creates new job, inserts CSV byte content, than closes job and returns summary of whole operation.
        /// </summary>
        public static async Task<string> SendBulkDataAsBytes(Parameters input, CancellationToken cancellationToken)
        {
            client.DefaultRequestHeaders.Add("Authorization", input.AuthToken);

            byte[] fileBytes = ReadBytesFrom(input.DirectoryCSV);

            HttpRequestMessage creation = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($@"{input.BaseDomainURL}/services/data/v55.0/jobs/ingest/"),
                Content = new StringContent(input.CreateJobBody, Encoding.UTF8, "application/json")
            };

            JobResult creationResult = await JobOperation(creation);

            JToken jobInformation = JToken.Parse(creationResult.HttpResultBody);

            HttpRequestMessage insertion = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($@"{input.BaseDomainURL}/{jobInformation["contentUrl"]}"),
                Content = new ByteArrayContent(fileBytes)
            };

            await JobOperation(insertion);

            HttpRequestMessage closure = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri($@"{input.BaseDomainURL}/{jobInformation["contentUrl"]}"),
                Content = new StringContent("{ \"state\":\"UploadComplete\" }", Encoding.UTF8, "application/json")
            };

            await JobOperation(closure);

            HttpRequestMessage getJobInformation = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($@"{input.BaseDomainURL}/{jobInformation["contentUrl"]}")
            };

            JobResult afterJobInformation = await JobOperation(getJobInformation);

            return JToken.Parse(afterJobInformation.HttpResultBody).ToString();
        }

        private static byte[] ReadBytesFrom(string directory)
        {
            return File.ReadAllBytes(directory);
        }

        private static async Task<JobResult> JobOperation(HttpRequestMessage request)
        {

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
