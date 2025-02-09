#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitResultRepository
    {
        Task<IReadOnlyCollection<SubmitResult>> SearchAsync(ISpecification<SubmitResult> specification, int skip, int take);

        Task<IReadOnlyCollection<long>> GetSolvedProblemsAsync(ISpecification<SubmitResult> specification);

        SubmitResult? DequeueUnchecked();
        Task<int> CountAsync(ISpecification<SubmitResult> specification);
        Task<SubmitResult?> GetAsync(long id);
        Task<SubmitResult> SaveAsync(SubmitResult submitResult);
    }
}
