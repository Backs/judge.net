using System.Data.Entity.ModelConfiguration;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Mappings
{
    internal sealed class ContestTaskSubmitMapping : EntityTypeConfiguration<ContestTaskSubmit>
    {
        private const byte SubmitType = 2;
        public ContestTaskSubmitMapping()
        {
            Map(map =>
                {
                    map.Requires(@"SubmitType").HasValue(SubmitType);
                }
            );
        }
    }
}
