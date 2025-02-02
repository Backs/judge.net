using System.Threading.Tasks;
using Judge.Web.Client.Admin;
using Judge.Web.Client.Problems;

namespace Judge.Services;

public interface IProblemsService
{
    Task<ProblemsList> SearchAsync(long? userId, ProblemsQuery query);
    Task<AllProblemsList> GetAllAsync(int skip, int take);
    Task<Problem?> GetAsync(long id);
    Task<EditProblem?> GetEditableAsync(long id);
    Task<EditProblem?> SaveAsync(EditProblem problem);
}