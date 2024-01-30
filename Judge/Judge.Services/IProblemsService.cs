using System.Threading.Tasks;
using Judge.Web.Client.Problems;

namespace Judge.Services;

public interface IProblemsService
{
    Task<ProblemsList> SearchAsync(long? userId, ProblemsQuery query);
    Task<Problem?> GetAsync(long id);
    Task<EditProblem?> SaveAsync(EditProblem problem);
}