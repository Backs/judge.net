﻿using System.Collections.Generic;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService.CustomCheckers;

internal interface ICustomChecker
{
    ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult);
}