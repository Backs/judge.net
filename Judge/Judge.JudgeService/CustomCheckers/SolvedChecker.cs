using System.Collections.Generic;
using System.Linq;
using Judge.Checker;
using Judge.Data;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;

namespace Judge.JudgeService.CustomCheckers;

internal class SolvedChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PreExecutable;

    private IUnitOfWorkFactory unitOfWorkFactory;

    public SolvedChecker(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions)
    {
        if (problemSettings?.PreSolvedProblemId == null)
        {
            return null;
        }

        using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();

        var submits = unitOfWork.SubmitResults.SearchAsync(new UserSolvedProblemsSpecification(
            submitResult.Submit.UserId,
            [problemSettings.PreSolvedProblemId.Value]), 0, int.MaxValue).GetAwaiter().GetResult();

        if (submits.Any(o => o is { Submit: ProblemSubmit, Status: SubmitStatus.Accepted }))
        {
            return null;
        }

        return new[]
        {
            new SubmitRunResult
            {
                RunStatus = RunStatus.Success,
                CheckStatus = CheckStatus.NotSolvedYet
            }
        };
    }
}