using Judge.Data;
using Judge.Model.SubmitSolution;
using NLog;

namespace Judge.JudgeService
{
    internal sealed class CheckService
    {
        private readonly IJudgeService _service;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILogger logger;

        public CheckService(IJudgeService service, IUnitOfWorkFactory unitOfWorkFactory, ILogger logger)
        {
            _service = service;
            _unitOfWorkFactory = unitOfWorkFactory;
            this.logger = logger;
        }

        public void Check()
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork(true))
            {
                var repository = unitOfWork.SubmitResultRepository;
                var submit = repository.DequeueUnchecked();
                if (submit == null)
                    return;

                using (NestedDiagnosticsLogicalContext.Push($"Submit-{submit.Id}"))
                {
                    this.logger.Info($"Dequeued submit");

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
}
