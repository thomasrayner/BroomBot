using System.Collections.Generic;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BroomBot
{
    public static class BroomBotUtils
    {
        public static async Task<IList<GitPullRequest>> GetPullRequests(
            GitHttpClient gitClient,
            string project)
        {
            List<GitPullRequest> pullCollection = new List<GitPullRequest>();
            List<GitRepository> repos = await gitClient.GetRepositoriesAsync(project);

            foreach (GitRepository repo in repos)
            {
                var pulls = await gitClient.GetPullRequestsAsync(
                    project,
                    repo.Id,
                    new GitPullRequestSearchCriteria());

                foreach (var pull in pulls)
                {
                    pullCollection.Add(pull);
                }
            }

            return pullCollection;
        }

        public static async Task<IList<GitPullRequest>> CheckPullRequestFreshness(
            GitHttpClient gitClient,
            string project,
            IList<GitPullRequest> pullRequests,
            DateTime staleDate)
        {
            List<GitPullRequest> pullCollection = new List<GitPullRequest>();

            foreach (GitPullRequest pr in pullRequests)
            {
                IList<GitPullRequestCommentThread> threads = await gitClient.GetThreadsAsync(
                    project,
                    pr.Repository.Id,
                    pr.PullRequestId);

                GitPullRequestCommentThread lastUpdated = threads
                    .OrderBy(p => p.LastUpdatedDate)
                    .FirstOrDefault();

                if (lastUpdated == null || lastUpdated.LastUpdatedDate < staleDate)
                {
                    pullCollection.Add(pr);
                }
            }

            return pullCollection;
        }
    }
}
