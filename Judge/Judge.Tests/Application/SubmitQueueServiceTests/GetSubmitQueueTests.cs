using System.Linq;
using Judge.Application;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
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
        private ITaskNameRepository _taskRepository;
        private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            var unitOfWorkFactory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            _submitResultRepository = MockRepository.GenerateMock<ISubmitResultRepository>();
            var languageRepository = MockRepository.GenerateMock<ILanguageRepository>();
            languageRepository.Stub(o => o.GetLanguages()).Return(new[] { new Language() });

            _taskRepository = MockRepository.GenerateMock<ITaskNameRepository>();
            _userRepository = MockRepository.GenerateMock<IUserRepository>();

            unitOfWork.Stub(o => o.GetRepository<ISubmitResultRepository>()).Return(_submitResultRepository);
            unitOfWork.Stub(o => o.GetRepository<ILanguageRepository>()).Return(languageRepository);
            unitOfWork.Stub(o => o.GetRepository<ITaskNameRepository>()).Return(_taskRepository);
            unitOfWork.Stub(o => o.GetRepository<IUserRepository>()).Return(_userRepository);

            unitOfWorkFactory.Stub(o => o.GetUnitOfWork(Arg<bool>.Is.Anything)).Return(unitOfWork);

            _service = new SubmitQueueService(unitOfWorkFactory);
        }

        [Test]
        public void HidePassedTestsForAcceptedTest()
        {
            var submit = Submit.Create();
            submit.ProblemId = 1;
            submit.UserId = 2;
            var submits = new[] { new SubmitResult(submit) { Status = SubmitStatus.Accepted, PassedTests = 10 } };

            _submitResultRepository.Stub(
                o => o.GetLastSubmits(Arg<long?>.Is.Anything, Arg<long?>.Is.Anything, Arg<int>.Is.Anything))
                .Return(submits);

            _taskRepository.Stub(o => o.GetTasks(Arg<long[]>.Is.Anything)).Return(new[] { new TaskName { Id = 1, Name = "A+B" } });

            _userRepository.Stub(o => o.GetUsers(Arg<long[]>.Is.Anything)).Return(new[] { new User { Id = 1 } });

            var model = _service.GetSubmitQueue(1, 1);

            Assert.That(model.Submits.Select(o => o.PassedTests), Is.All.Null);
        }
    }
}
