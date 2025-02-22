using System.Threading.Tasks;
using Judge.Web.Client.Contests;
using Judge.Web.Client.Problems;
using Contest = Judge.Web.Client.Contests.Contest;

namespace Judge.Services;

public interface IContestsService
{
    Task<ContestsInfoList> SearchAsync(ContestsQuery query, bool openedOnly);
    Task<Contest?> GetAsync(int id, long? userId);
    Task<EditContest?> GetEditableAsync(int id);
    Task<ContestResult?> GetResultAsync(int id);
    Task<EditContest?> SaveAsync(EditContest editContest);
    Task<Problem?> GetProblemAsync(int contestId, string label);
}