using System;
using Judge.Model.Contests;

namespace Judge.Services.Converters.Contests;

internal sealed class ContestConverterFactory : IContestConverterFactory
{
    public IContestConverter Get(ContestRules rules) =>
        rules switch
        {
            ContestRules.Acm => AcmConverter.Instance,
            ContestRules.Points => PointsConverter.Instance,
            ContestRules.CheckPoint => CheckPointsConverter.Instance,
            _ => throw new ArgumentOutOfRangeException(nameof(rules), rules, null)
        };
}