using System.Threading.Tasks;
using Judge.Web.Client.Submits;
using SubmitsQuery = Judge.Services.Model.SubmitsQuery;

namespace Judge.Services;

public interface ISubmitsService
{
    public Task<SubmitsList> SearchAsync(SubmitsQuery query);

    public Task SaveAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo);

    public Task<SubmitResultExtendedInfo?> GetResultAsync(long id);
    Task<SubmitResultExtendedInfo?> RejudgeAsync(long id);
}