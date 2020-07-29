using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Linq;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BroomBot
{
    public static class SweepBroom
    {
        [FunctionName("SweepBroom")]
        public static async void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Behind schedule");
            }

            log.LogInformation($"Started sweep: {DateTime.Now}");

            // Get the environment vars
            string PAT = Environment.GetEnvironmentVariable("PAT", EnvironmentVariableTarget.Process);
            string organization = Environment.GetEnvironmentVariable("Organization", EnvironmentVariableTarget.Process);
            string project = Environment.GetEnvironmentVariable("Project", EnvironmentVariableTarget.Process);

            // Set up the user strings
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\SweepBroom\UserStrings.json");
            BroomBotStrings userStrings = new BroomBotStrings(configPath);
            int staleAge = Convert.ToInt32(userStrings.StaleAge);
            int warningCount = Convert.ToInt32(userStrings.WarningCount);
            string collectionUri = $"https://dev.azure.com/{organization}";

            // Connect to Azure DevOps Services
            VssCredentials creds = new VssBasicCredential(string.Empty, PAT);
            VssConnection connection = new VssConnection(new Uri(collectionUri), creds);
            string botId = connection.AuthorizedIdentity.Id.ToString();

            // Get a GitHttpClient to talk to the Git endpoints
            using (GitHttpClient gitClient = connection.GetClient<GitHttpClient>())
            {
                // Get all the PRs and filter out the ones that were created since the stale date
                IList<GitPullRequest> allPRs = await BroomBotUtils.GetPullRequests(gitClient, project);
                log.LogInformation($"Found {allPRs.Count} pull requests in {project}");

                if (allPRs.Count == 0)
                {
                    log.LogInformation($"Finished sweep: {DateTime.Now}");
                    return;
                }

                // Find PRs that were created before the stale date, otherwise they're too new to be relevant
                // The stale date is today minus the user configured number of hours it takes something to get stale
                DateTime staleDate = DateTime.Now.AddHours(-staleAge).ToUniversalTime();
                IList<GitPullRequest> createdBeforeStaleDate = allPRs.Where(p => p.CreationDate < staleDate).ToList();
                log.LogInformation($"Found {createdBeforeStaleDate.Count} pull requests created before {staleDate}");

                if (createdBeforeStaleDate.Count == 0)
                {
                    log.LogInformation($"Finished sweep: {DateTime.Now}");
                    return;
                }

                // which PRs haven't had a comment since staledate, and if the last comment is from the bot
                Dictionary<GitPullRequest, bool> stalePRs = await BroomBotUtils.CheckPullRequestFreshness(
                    gitClient, project, createdBeforeStaleDate, staleDate, botId);
                log.LogInformation($"Found {stalePRs.Count} pull requests with no updates since {staleDate}");

                if (stalePRs.Count == 0)
                {
                    log.LogInformation($"Finished sweep: {DateTime.Now}");
                    return;
                }

                // tag & update stale PRs and return candidates for abandonment
                IList<GitPullRequest> abandonmentCandidates = await BroomBotUtils.TagStalePullRequests(gitClient, project, stalePRs, userStrings.WarningPrefix, warningCount, userStrings.PullRequestIsStale);
                log.LogInformation($"Found {abandonmentCandidates.Count} pull requests that are due to be abandoned");

                // PRs that need to be abandoned
                if (abandonmentCandidates.Count == 0)
                {
                    log.LogInformation($"Finished sweep: {DateTime.Now}");
                    return;
                }

                bool abandonmentSuccess = await BroomBotUtils.AbandonPullRequests(gitClient, abandonmentCandidates);
                if (!abandonmentSuccess) log.LogError("Did not correctly abandon all relevant PRs");
            }

            log.LogInformation($"Finished sweep: {DateTime.Now}");
        }
    }
}
