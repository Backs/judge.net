using System.Threading.Tasks;
using Judge.Web.Client.Submits;
using SubmitsQuery = Judge.Services.Model.SubmitsQuery;

namespace Judge.Services;

public interface ISubmitsService
{
    public Task<SubmitResultsList> SearchAsync(SubmitsQuery query, long? currentUserId);

    public Task<long> SaveAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo);

    public Task<SubmitResultExtendedInfo?> GetResultAsync(long id, long? currentUserId);
    Task<SubmitResultExtendedInfo?> RejudgeAsync(long id, long? currentUserId);
}