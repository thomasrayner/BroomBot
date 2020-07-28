using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace Microsoft.Hackathon
{
    public static class ADOTest
    {
        [FunctionName("ADOTest")]
        public static async void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
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

            string collectionUri = $"https://dev.azure.com/{organization}";
            VssCredentials creds = new VssBasicCredential(string.Empty, PAT);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(collectionUri), creds);

            var pullCollection = new List<int>();

            // Get a GitHttpClient to talk to the Git endpoints
            using (GitHttpClient gitClient = connection.GetClient<GitHttpClient>())
            {
                // Get data about a specific repository
                var repos = await gitClient.GetRepositoriesAsync(project);
                foreach (var repo in repos)
                {
                    var pulls = await gitClient.GetPullRequestsAsync(project, repo.Id, new GitPullRequestSearchCriteria());
                    foreach (var pull in pulls)
                    {
                        pullCollection.Add(pull.PullRequestId);
                    }
                }
            }

            log.LogInformation(string.Join(", ", pullCollection));
        }
    }
}
