![BroomBot Logo](img/broombot-text.png)

# Contributing

👍🎉 First off, thanks for taking the time to contribute! 🎉👍

The following is a set of guidelines for contributing to **BroomBot**. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request.

## Table of Contents

* [Code of Conduct](#code-of-conduct)
* [How Can I Contribute?](#how-can-i-contribute)
  * [Reporting Bugs](#reporting-bugs)
  * [Suggesting Enhancements](#suggesting-enhancements)
  * [Pull Requests](#pull-requests)
    * [What should I know before I get started?](#what-should-i-know-before-i-get-started)
      * [Design](#design)
      * [Local Debugging](#local-debugging)

## Code of Conduct

This project and everyone participating in it is governed by our adopted [Microsoft Open Source Code of Conduct](https://microsoft.github.io/codeofconduct/). By participating, you are expected to uphold this code. Please report unacceptable behavior to thmsrynr@outlook.com.

## How Can I Contribute

### Reporting Bugs

**BroomBot** is maintained primarily by a single person with the occasional help of others. Please be patient with bug report responses and engagements.

This section guides you through submitting a bug report for **BroomBot**. Following these guidelines helps maintainers and the community understand your report 📝, reproduce the behavior 💻 💻, and find related reports 🔎.

Before creating bug reports, please check the issues on this repo as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible. 

#### Before Submitting A Bug Report

* Check if you can reproduce the problem reliably
* Perform a cursory search to see if the problem has already been reported. If it has and the issue is still open, add a comment to the existing issue instead of opening a new one.

##### How Do I Submit A (Good) Bug Report?

Bugs are tracked as GitHub issues. Create an issue and provide the following information by providing as much useful information as possible.

Explain the problem and include additional details to help maintainers reproduce the problem:

* Use a clear and descriptive title for the issue to identify the problem.
* Describe the exact steps which reproduce the problem in as many details as possible. When listing steps, don't just say what you did, but explain how you did it.
* Provide specific examples to demonstrate the steps. Include links to files or GitHub projects, or copy/pasteable snippets, which you use in those examples. If you're providing snippets in the issue, use Markdown code blocks.
* Describe the behavior you observed after following the steps and point out what exactly is the problem with that behavior.
* Explain which behavior you expected to see instead and why.
* Include screenshots and animated GIFs (if possible) which show you following the described steps and clearly demonstrate the problem.
* If the problem is related to performance or memory, include a CPU profile capture with your report.
* If the problem wasn't triggered by a specific action or configuration, describe what you were doing before the problem happened and share more information using the guidelines below.

Provide more context by answering these questions:

* Did the problem start happening recently or was this always a problem?
* If the problem started happening recently, can you reproduce the problem in an older version of **BroomBot**? What's the most recent version in which the problem doesn't happen?
* Can you reliably reproduce the issue? If not, provide details about how often the problem happens and under which conditions it normally happens.

### Suggesting Enhancements

**BroomBot** is maintained primarily by a single person with the occasional help of others. Please be patient with enhancement suggestion responses and engagements.

This section guides you through submitting an enhancement suggestion for **BroomBot**, including completely new features and minor improvements to existing functionality. Following these guidelines helps maintainers and the community understand your suggestion 📝 and find related suggestions 🔎.

Before creating enhancement suggestions, please check this list and this repo's issues as you might find out that you don't need to create one. When you are creating an enhancement suggestion, please include as many details as possible.

#### Before Submitting An Enhancement Suggestion

* Check if you're using the latest version of **BroomBot** and if you can get the desired behavior by changing **BroomBot**'s config settings.
* Perform a cursory search to see if the enhancement has already been suggested. If it has, add a comment to the existing issue instead of opening a new one.

#### How Do I Submit A (Good) Enhancement Suggestion?

Enhancement suggestions are tracked as GitHub issues. Create an issue on this repository and provide the following information:

* Use a clear and descriptive title for the issue to identify the suggestion.
* Provide a step-by-step description of the suggested enhancement in as many details as possible.
* Provide specific examples to demonstrate the steps. Include copy/pasteable snippets which you use in those examples, as Markdown code blocks.
* Describe the current behavior and explain which behavior you expected to see instead and why.
* Include screenshots and animated GIFs which help you demonstrate the steps or point out the part of Atom which the suggestion is related to.
* Explain why this enhancement would be useful to most **BroomBot** users.

### Pull Requests

**BroomBot** is maintained primarily by a single person with the occasional help of others. Please be patient with code reviews and engagements.

#### What Should I Know Before I Get Started

##### Design

**BroomBot** is made up of only a few components and dependencies.

**BroomBot** is an Azure Function that interacts with Azure DevOps, which means that for proper testing, it is recommended that you setup a testing Azure Subscription and a testing Azure DevOps organization. Trials of both products are available, and personal use credits are included with MSDN, among other programs. Please be careful if you intend to use your employer's or other production subscriptions and/or organizations for testing and development.

See the [Deployment Guide](/deployment.md) for more information about setting what you need in Azure for **BroomBot**.

**BroomBot** is intended for development in Visual Studio Code.

##### Local Debugging

Local debugging is not only possible, but encouraged. Microsoft's published guide on [local Azure Function debugging](https://docs.microsoft.com/en-us/azure/developer/javascript/tutorial/vscode-function-app-http-trigger/tutorial-vscode-serverless-node-test-local#:~:text=In%20Visual%20Studio%20Code%2C%20press%20F5%20to%20launch,to%20open%20the%20browser%20and%20run%20the%20function%3A) covers this concept in detail.

#### `everything-is-stale-test` branch

**BroomBot** maintains a [`everything-is-stale-test` branch](https://github.com/thomasrayner/BroomBot/tree/everything-is-stale-test) on this repo whose settings are configured to sweep every minute, and to treat every pull request it finds as stale. This is obviously not what you would want in a production deployment, but may be helpful when testing and debugging **BroomBot**.
