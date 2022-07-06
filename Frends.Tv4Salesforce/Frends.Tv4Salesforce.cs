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
    public static class BulkInsert
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// This is task
        /// Documentation: https://github.com/GlazowskiHiQ/Frends.Tv4Salesforce
        /// </summary>
        public static Result InsertSales(Parameters input, CancellationToken cancellationToken)
        {
            byte[] fileBytes = ReadBytesFrom(input.DirectoryCSV);
            var freshJob = CreateJob(input.AuthToken, input.BaseDomainURL, input.CreateJobBody);
            return null;
        }

        private static byte[] ReadBytesFrom(string directory)
        {
            return File.ReadAllBytes(directory);
        }

        private static async Task<JToken> CreateJob(string authToken, string domain, string creationBody)
        {
            client.DefaultRequestHeaders.Add("Authorization", authToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(domain),
                Content = new StringContent(creationBody, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JToken.Parse(responseBody);
        }

    }
}
