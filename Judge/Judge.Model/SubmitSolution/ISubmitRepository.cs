using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.SubmitSolution;

public interface ISubmitRepository
{
    void Add(SubmitBase item);

    Task<IReadOnlyCollection<SubmitBase>> SearchAsync(ISpecification<SubmitBase> specification);
}