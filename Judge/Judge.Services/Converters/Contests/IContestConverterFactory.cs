using Judge.Model.Contests;

namespace Judge.Services.Converters.Contests;

public interface IContestConverterFactory
{
    IContestConverter Get(ContestRules rules);
}