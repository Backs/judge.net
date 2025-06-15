using System;
using System.Collections.Generic;
using System.IO;
using Judge.Checker;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class CurrentTimeChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PostExecutable;

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions)
    {
        if (!problemSettings.UseCurrentTimeChecker)
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

        var text = File.ReadAllText(fileOptions.OutputFileName).Trim();

        if (!TimeOnly.TryParseExact(text, "hh:mm:ss", out var timeOnly))
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

        var currentTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        if (currentTime.Hour != timeOnly.Hour ||
            currentTime.Minute != timeOnly.Minute ||
            currentTime.Second != timeOnly.Second)
        {
            return
            [
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.WA
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