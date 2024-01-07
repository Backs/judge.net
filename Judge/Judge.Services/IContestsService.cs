using System.Threading.Tasks;
using Judge.Web.Client.Contests;

namespace Judge.Services;

public interface IContestsService
{
    Task<ContestsInfoList> SearchAsync(ContestsQuery query);

    Task<Contest?> GetAsync(int id, long? userId);
}