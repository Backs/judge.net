using System.Threading.Tasks;
using Judge.Web.Client.Problems;

namespace Judge.Services
{
    public interface IProblemsService
    {
        Task<ProblemsList> GetProblemsAsync(ProblemsQuery query);
    }
}