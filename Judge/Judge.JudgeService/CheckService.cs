using Judge.Data;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService
{
    internal sealed class CheckService
    {
        private readonly IJudgeService _service;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public CheckService(IJudgeService service, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _service = service;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Check()
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork(transactionRequired: true))
            {
                var repository = unitOfWork.GetRepository<ISubmitResultRepository>();
                var submit = repository.DequeueUnchecked();

                var result = _service.Check(submit);

                submit.PassedTests = result.TestsPassedCount;
                submit.TotalBytes = result.PeakMemoryBytes;
                submit.TotalMilliseconds = result.TimeConsumedMilliseconds;
                submit.Status = result.GetStatus();
                submit.CompileOutput = result.CompileResult.Output;
                submit.RunDescription = result.Description;
                submit.RunOutput = result.Output;

                unitOfWork.Commit();
            }
        }
    }
}
