using System;
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
        private ITaskRepository _taskRepository;
        private ISubmitRepository _submitRepository;

        [SetUp]
        public void SetUp()
        {
            var factory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            factory.Stub(o => o.GetUnitOfWork(Arg<bool>.Is.Anything)).Return(unitOfWork);

            _taskRepository = MockRepository.GenerateMock<ITaskRepository>();
            _submitRepository = MockRepository.GenerateMock<ISubmitRepository>();
            unitOfWork.Stub(o => o.GetRepository<ITaskRepository>()).Return(_taskRepository);
            unitOfWork.Stub(o => o.GetRepository<ISubmitRepository>()).Return(_submitRepository);

            var principal = MockRepository.GenerateMock<IPrincipal>();
            _service = new SubmitSolutionService(factory, principal);
        }

        [Test]
        public void GetSolutionTest()
        {
            const long submitId = 1;
            const long userId = 2;
            const string sourceCode = "qwe";

            var submit = new ProblemSubmit
            {
                FileName = "main.cpp",
                Id = submitId,
                LanguageId = 4,
                ProblemId = 6,
                SourceCode = sourceCode,
                UserId = userId
            };

            const string taskName = "task";

            var task = new Task
            {
                IsOpened = true,
                Name = taskName
            };

            _submitRepository.Stub(o => o.Get(submitId)).Return(submit);
            _taskRepository.Stub(o => o.Get(6)).Return(task);

            var result = _service.GetSolution(submitId, userId);

            Assert.NotNull(result);
            Assert.That(result.SourceCode, Is.EqualTo(sourceCode));
            Assert.That(result.ProblemName, Is.EqualTo(taskName));
        }

        [Test]
        public void GetClosedTaskSolutionTest()
        {
            const long submitId = 1;
            const long userId = 2;
            const string sourceCode = "qwe";

            var submit = new ProblemSubmit
            {
                FileName = "main.cpp",
                Id = submitId,
                LanguageId = 4,
                ProblemId = 6,
                SourceCode = sourceCode,
                UserId = userId
            };

            var task = new Task
            {
                IsOpened = false,
                Name = "task"
            };

            _submitRepository.Stub(o => o.Get(submitId)).Return(submit);
            _taskRepository.Stub(o => o.Get(6)).Return(task);

            Assert.Throws<InvalidOperationException>(() => _service.GetSolution(submitId, userId));
        }

        [Test]
        public void GetAnotherUserSolutionTest()
        {
            const long submitId = 1;
            const long userId = 2;
            const string sourceCode = "qwe";

            var submit = new ProblemSubmit
            {
                FileName = "main.cpp",
                Id = submitId,
                LanguageId = 4,
                ProblemId = 6,
                SourceCode = sourceCode,
                UserId = userId
            };

            var task = new Task
            {
                IsOpened = false,
                Name = "task"
            };

            _submitRepository.Stub(o => o.Get(submitId)).Return(submit);
            _taskRepository.Stub(o => o.Get(6)).Return(task);

            Assert.Throws<AuthenticationException>(() => _service.GetSolution(submitId, 3));
        }
    }
}
