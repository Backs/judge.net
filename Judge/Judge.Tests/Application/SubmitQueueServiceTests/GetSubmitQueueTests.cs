using System.Linq;
using Judge.Application;
using Judge.Data;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Application.SubmitQueueServiceTests
{
    [TestFixture]
    public sealed class GetSubmitQueueTests
    {
        private SubmitQueueService _service;
        private ISubmitResultRepository _submitResultRepository;

        [SetUp]
        public void SetUp()
        {
            var unitOfWorkFactory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            _submitResultRepository = MockRepository.GenerateMock<ISubmitResultRepository>();
            var languageRepository = MockRepository.GenerateMock<ILanguageRepository>();
            languageRepository.Stub(o => o.GetLanguages()).Return(new[] { new Language() });

            unitOfWork.Stub(o => o.GetRepository<ISubmitResultRepository>()).Return(_submitResultRepository);
            unitOfWork.Stub(o => o.GetRepository<ILanguageRepository>()).Return(languageRepository);

            unitOfWorkFactory.Stub(o => o.GetUnitOfWork(Arg<bool>.Is.Anything)).Return(unitOfWork);

            _service = new SubmitQueueService(unitOfWorkFactory);
        }

        [Test]
        public void HidePassedTestsForAcceptedTest()
        {
            var submits = new[] { new SubmitResult(Submit.Create()) { Status = SubmitStatus.Accepted, PassedTests = 10 } };

            _submitResultRepository.Stub(
                o => o.GetLastSubmits(Arg<long?>.Is.Anything, Arg<long?>.Is.Anything, Arg<int>.Is.Anything))
                .Return(submits);

            var model = _service.GetSubmitQueue(1, 1);

            Assert.That(model.Submits.Select(o => o.PassedTests), Is.All.Null);
        }
    }
}
