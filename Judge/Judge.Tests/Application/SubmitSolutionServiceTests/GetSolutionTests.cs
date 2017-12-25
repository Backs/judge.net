using System.Security.Authentication;
using System.Security.Principal;
using Judge.Application;
using Judge.Application.Interfaces;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Application.SubmitSolutionServiceTests
{
    [TestFixture]
    internal sealed class GetSolutionTests
    {
        private ISubmitSolutionService _service;
        private ITaskNameRepository _taskRepository;
        private ISubmitResultRepository _submitResultRepository;

        [SetUp]
        public void SetUp()
        {
            var factory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            factory.Stub(o => o.GetUnitOfWork(Arg<bool>.Is.Anything)).Return(unitOfWork);

            _taskRepository = MockRepository.GenerateMock<ITaskNameRepository>();
            _submitResultRepository = MockRepository.GenerateMock<ISubmitResultRepository>();
            unitOfWork.Stub(o => o.GetRepository<ITaskNameRepository>()).Return(_taskRepository);
            unitOfWork.Stub(o => o.GetRepository<ISubmitResultRepository>()).Return(_submitResultRepository);

            var principal = MockRepository.GenerateMock<IPrincipal>();
            _service = new SubmitSolutionService(factory, principal);
        }

        [Test]
        public void GetSolutionTest()
        {
            const long submitId = 1;
            const long userId = 2;
            const string sourceCode = "qwe";

            var submitResult = new SubmitResult(new ProblemSubmit
            {
                FileName = "main.cpp",
                Id = submitId,
                LanguageId = 4,
                ProblemId = 6,
                SourceCode = sourceCode,
                UserId = userId,
            })
            {
                Id = submitId
            };

            const string taskName = "task";

            var task = new TaskName
            {
                IsOpened = true,
                Name = taskName
            };

            _submitResultRepository.Stub(o => o.Get(submitId)).Return(submitResult);
            _taskRepository.Stub(o => o.Get(6)).Return(task);

            var result = _service.GetSolution(submitId, userId);

            Assert.NotNull(result);
            Assert.That(result.SourceCode, Is.EqualTo(sourceCode));
            Assert.That(result.ProblemName, Is.EqualTo(taskName));
        }

        [Test]
        public void GetAnotherUserSolutionTest()
        {
            const long submitId = 1;
            const long userId = 2;
            const string sourceCode = "qwe";

            var submitResult = new SubmitResult(new ProblemSubmit
            {
                FileName = "main.cpp",
                Id = submitId,
                LanguageId = 4,
                ProblemId = 6,
                SourceCode = sourceCode,
                UserId = userId
            })
            {
                Id = submitId
            };

            var task = new TaskName
            {
                IsOpened = false,
                Name = "task"
            };

            _submitResultRepository.Stub(o => o.Get(submitId)).Return(submitResult);
            _taskRepository.Stub(o => o.Get(6)).Return(task);

            Assert.Throws<AuthenticationException>(() => _service.GetSolution(submitId, 3));
        }
    }
}
