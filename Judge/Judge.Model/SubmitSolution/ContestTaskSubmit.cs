using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge.Model.SubmitSolution
{
    public sealed class ContestTaskSubmit : SubmitBase
    {
        public static ContestTaskSubmit Create()
        {
            var submit = new ContestTaskSubmit();
            submit.Results.Add(new SubmitResult(submit));
            return submit;
        }

        public override long GetProblemId()
        {
            throw new NotImplementedException();
        }
    }
}
