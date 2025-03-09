using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner;
using Octokit;
using CheckStatus = Judge.Checker.CheckStatus;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class PullRequestChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PostExecutable;
    private static readonly Regex NumberRegex = new Regex(@"#(\d+)", RegexOptions.Compiled);

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions)
    {
        if (!problemSettings.UserPullRequestChecker)
            return null;

        if (!File.Exists(fileOptions.OutputFileName))
        {
            return
            [
                new SubmitRunResult
                {
                    RunStatus = RunStatus.RuntimeError
                }
            ];
        }

        var text = File.ReadAllText(fileOptions.OutputFileName);

        var match = NumberRegex.Match(text);

        if (!match.Success)
        {
            return
            [
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.PE
                }
            ];
        }

        var number = match.Groups[1].Value;

        var github = new GitHubClient(new ProductHeaderValue("Backs"));
        var pullRequests = github.PullRequest.GetAllForRepository("Backs", "judge.net").GetAwaiter().GetResult();

        var pr = pullRequests.FirstOrDefault(o => o.Number.ToString() == number);

        if (pr == null)
        {
            return
            [
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.PullRequestNotFound
                }
            ];
        }

        var login = submitResult.Submit.User.UserName;

        if (!pr.Body.Contains(login, StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.LoginNotFound
                }
            ];
        }

        return
        [
            new SubmitRunResult
            {
                RunStatus = RunStatus.Success,
                CheckStatus = CheckStatus.OK
            }
        ];
    }
}