# BroomBot

Azure DevOps Pull Request Minder. Sweep up forgotten and forlorn Azure DevOps Pull Requests.

## About

GitHub has a rich ecosystem of bots and helper tools that (among other things) empower repo owners to automatically remind pull request creators to stay on top of their PRs, and clean up stale PRs when they are unattended too long. There are not as many great solutions for this same problem in Azure DevOps, and so **BroomBot** fills that gap.

**BroomBot** periodically checks your Azure DevOps repos for pull requests that haven't been updated in a while and reminds the person who opened it to attend to their PR. After a few reminders, **BroomBot** will abandon the stale PR.
