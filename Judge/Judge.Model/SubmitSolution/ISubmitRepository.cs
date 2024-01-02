using System.Threading.Tasks;

namespace Judge.Model.SubmitSolution
{
    using System.Collections.Generic;

    public interface ISubmitRepository
    {
        void Add(SubmitBase item);

        SubmitBase Get(long submitId);
        
        IEnumerable<SubmitBase> Get(ISpecification<SubmitBase> specification);
        Task<IReadOnlyCollection<SubmitBase>> SearchAsync(ISpecification<SubmitBase> specification);
    }
}
