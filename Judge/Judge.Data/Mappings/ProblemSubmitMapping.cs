using System.Data.Entity.ModelConfiguration;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Mappings
{
    internal sealed class ProblemSubmitMapping : EntityTypeConfiguration<ProblemSubmit>
    {
        private const byte SubmitType = 1;

        public ProblemSubmitMapping()
        {
            Map(map =>
                {
                    map.Requires(@"SubmitType").HasValue(SubmitType);
                }
            );
        }
    }
}
