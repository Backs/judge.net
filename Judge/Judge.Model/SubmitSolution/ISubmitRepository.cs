using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitRepository
    {
        void Add(SubmitBase item);
        SubmitBase Get(long submitId);
        IEnumerable<SubmitBase> Get(ISpecification<SubmitBase> specification);
    }
}
