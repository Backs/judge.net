using System.Threading.Tasks;
using Judge.Web.Client.Submits;
using SubmitsQuery = Judge.Services.Model.SubmitsQuery;

namespace Judge.Services;

public interface ISubmitsService
{
    public Task<SubmitResultsList> SearchAsync(SubmitsQuery query);

    public Task<long> SaveAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo);

    public Task<SubmitResultExtendedInfo?> GetResultAsync(long id);
    Task<SubmitResultExtendedInfo?> RejudgeAsync(long id);
}