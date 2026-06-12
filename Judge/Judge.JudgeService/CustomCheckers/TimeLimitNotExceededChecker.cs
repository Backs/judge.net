using System.Collections.Generic;
using System.IO;
using Judge.Checker;
using Judge.Data;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class TimeLimitNotExceededChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PostExecutable;

    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public TimeLimitNotExceededChecker(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions, IRunResult runResult)
    {
        if (!problemSettings.UseTimeLimitNotExceededChecker)
            return null;

        using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var problem = unitOfWork.Tasks.Get(submitResult.Submit.ProblemId)!;
        if (runResult.TimeConsumedMilliseconds <= problem.TimeLimitMilliseconds)
        {
            return
            [
                new SubmitRunResult
                {
                    CheckStatus = CheckStatus.TimeLimitNotExceeded,
                    RunStatus = RunStatus.Success,
                }
            ];
        }

        var output = File.ReadAllText(fileOptions.OutputFileName).Trim();
        var answer = File.ReadAllText(fileOptions.AnswerFileName).Trim();

        if (output == answer)
        {
            return
            [
                new SubmitRunResult
                {
                    CheckStatus = CheckStatus.OK,
                    RunStatus = RunStatus.Success,
                }
            ];
        }

        return
        [
            new SubmitRunResult
            {
                CheckStatus = CheckStatus.WA,
                RunStatus = RunStatus.Success,
            }
        ];
    }
}