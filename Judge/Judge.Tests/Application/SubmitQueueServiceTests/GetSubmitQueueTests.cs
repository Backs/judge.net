using System.Linq;
using System.Security.Principal;
using Judge.Application;
using Judge.Data;
using Judge.Model;
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

            unitOfWork.Stub(o => o.SubmitResultRepository).Return(_submitResultRepository);
            unitOfWork.Stub(o => o.LanguageRepository).Return(languageRepository);
            unitOfWork.Stub(o => o.TaskNameRepository).Return(_taskRepository);
            unitOfWork.Stub(o => o.UserRepository).Return(_userRepository);

            unitOfWorkFactory.Stub(o => o.GetUnitOfWork()).Return(unitOfWork);

            var principal = MockRepository.GenerateMock<IPrincipal>();

            _service = new SubmitQueueService(unitOfWorkFactory, principal);
        }

        [Test]
        public void HidePassedTestsForAcceptedTest()
        {
            var submit = ProblemSubmit.Create();
            submit.ProblemId = 1;
            submit.UserId = 1;
            var submits = new[] { new SubmitResult(submit) { Status = SubmitStatus.Accepted, PassedTests = 10 } };

            _submitResultRepository.Stub(
                o => o.GetSubmits(Arg<ISpecification<SubmitResult>>.Is.Anything, Arg<int>.Is.Anything, Arg<int>.Is.Anything))
                .Return(submits);

            _taskRepository.Stub(o => o.GetTasks(Arg<long[]>.Is.Anything)).Return(new[] { new TaskName { Id = 1, Name = "A+B" } });

            _userRepository.Stub(o => o.Find(Arg<ISpecification<User>>.Is.Anything)).Return(new[] { new User { Id = 1 } });

            var model = _service.GetSubmitQueue(1, 1, 1);

            Assert.That(model.Submits.Select(o => o.PassedTests), Is.All.Null);
        }
    }
}
