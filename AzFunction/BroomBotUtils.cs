using System.Collections.Generic;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
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
                var pulls = await gitClient.GetPullRequestsAsync(project, repo.Id, new GitPullRequestSearchCriteria());

                foreach (var pull in pulls)
                {
                    pullCollection.Add(pull);
                }
            }

            return pullCollection;
        }

        public static async Task<Dictionary<GitPullRequest, bool>> CheckPullRequestFreshness(
            GitHttpClient gitClient,
            string project,
            IList<GitPullRequest> pullRequests,
            DateTime staleDate,
            string botName)
        {
            Dictionary<GitPullRequest, bool> pullCollection = new Dictionary<GitPullRequest, bool>();

            foreach (GitPullRequest pr in pullRequests)
            {
                IList<GitPullRequestCommentThread> threads = await gitClient.GetThreadsAsync(project, pr.Repository.Id, pr.PullRequestId);

                GitPullRequestCommentThread lastUpdated = threads.OrderBy(p => p.LastUpdatedDate).FirstOrDefault();

                // Stale PRs have never been updated, or haven't been updated since the staleDate
                if (lastUpdated == null || lastUpdated.LastUpdatedDate < staleDate)
                {
                    // Knowing whether or not the last comment was from the bot will tell us if we need to check for tags or not
                    bool commentByBot = lastUpdated != null && lastUpdated.Comments.Last().Author.Descriptor.Identifier == botName;
                    pullCollection.Add(pr, commentByBot);
                }
            }

            return pullCollection;
        }

        public static async Task<IList<GitPullRequest>> TagStalePRs(
            GitHttpClient gitClient,
            string project,
            Dictionary<GitPullRequest, bool> stalePRs,
            string warningPrefix,
            int warningCount)
        {
            List<GitPullRequest> pullCollection = new List<GitPullRequest>();

            foreach (KeyValuePair<GitPullRequest, bool> pr in stalePRs)
            {
                // Add a comment to the PR describing that it's stale
                Comment comment = new Comment
                {
                    Content = $"@{pr.Key.CreatedBy.Descriptor.Identifier} - This pull request is stale. Please update it or it risks being abandoned."
                };
                List<Comment> commentList = new List<Comment>
                {
                    comment
                };

                GitPullRequestCommentThread commentThread = new GitPullRequestCommentThread
                {
                    Comments = commentList
                };
                await gitClient.CreateThreadAsync(commentThread, pr.Key.Repository.Id, pr.Key.PullRequestId);

                // Handle the tag updating
                List<WebApiTagDefinition> allTags = await gitClient.GetPullRequestLabelsAsync(project, pr.Key.Repository.Id, pr.Key.PullRequestId);
                WebApiCreateTagRequestData newLabel = new WebApiCreateTagRequestData
                {
                    Name = string.Format("{0}: (UTC) {1}", warningPrefix, DateTime.UtcNow.ToString("g"))
                };

                if (!pr.Value)
                {
                    // PR has been updated since the last bot action, remove tags
                    List<WebApiTagDefinition> tagsToRemove = allTags.Where(t => t.Name.StartsWith(warningPrefix)).ToList();

                    foreach (WebApiTagDefinition tag in tagsToRemove)
                    {
                        await gitClient.DeletePullRequestLabelsAsync(project, pr.Key.Repository.Id, pr.Key.PullRequestId, tag.Id.ToString());
                    }
                }
                else
                {
                    // check how many tags are already on it, if it's more than the warning threshold, add it to the list of returned PRs
                    // no need to add another tag because this PR is going to be abandoned
                    int botTagCount = allTags.Where(t => t.Name.StartsWith(warningPrefix)).Count();

                    if (botTagCount >= warningCount) pullCollection.Add(pr.Key);
                    continue;
                }

                // add a tag
                await gitClient.CreatePullRequestLabelAsync(newLabel, pr.Key.Repository.Id, pr.Key.PullRequestId);
            }

            return pullCollection;
        }
    }
}
