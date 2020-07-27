using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Hackathon
{
    public static class ADOTest
    {
        [FunctionName("TimerTriggerCSharp")]
        public static async void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Behind schedule");
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Get the environment vars
            string PAT = Environment.GetEnvironmentVariable("PAT", EnvironmentVariableTarget.Process);
            string organization = Environment.GetEnvironmentVariable("Organization", EnvironmentVariableTarget.Process);
            string project = Environment.GetEnvironmentVariable("Project", EnvironmentVariableTarget.Process);

            // Construct the auth header content
            string b64PAT = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", PAT)));

            // The API endpoint to hit
            string getPRsUri = $"https://dev.azure.com/{organization}/{project}/_apis/git/pullrequests?api-version=5.1&searchCriteria.status=active";

            // Make the request
            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", b64PAT);

                using HttpResponseMessage response = await client.GetAsync(getPRsUri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                log.LogInformation(responseBody);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }
        }
    }
}
