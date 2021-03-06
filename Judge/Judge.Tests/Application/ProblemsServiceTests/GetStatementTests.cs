﻿using Judge.Application;
using Judge.Data;
using Judge.Model.CheckSolution;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Application.ProblemsServiceTests
{
    [TestFixture]
    public class GetStatementTests
    {
        private ITaskRepository _taskRepository;
        private ProblemsService _service;

        [SetUp]
        public void SetUp()
        {
            var unitOfWorkFactory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();

            _taskRepository = MockRepository.GenerateMock<ITaskRepository>();

            unitOfWork.Stub(o => o.TaskRepository).Return(_taskRepository);

            unitOfWorkFactory.Stub(o => o.GetUnitOfWork()).Return(unitOfWork);

            _service = new ProblemsService(unitOfWorkFactory);
        }

        [Test]
        public void StatementNotFoundTest()
        {
            var result = _service.GetStatement(1, false);
            Assert.IsNull(result);
        }

        [Test]
        public void OpenedTaskStatementTest()
        {
            var task = new Task
            {
                MemoryLimitBytes = 1024000,
                TimeLimitMilliseconds = 5000,
                Name = "Task",
                Statement = "*bb*",
                TestsFolder = "Folder",
                IsOpened = true
            };
            _taskRepository.Stub(o => o.Get(1)).Return(task);
            var result = _service.GetStatement(1, false);

            Assert.That(result.MemoryLimitBytes, Is.EqualTo(task.MemoryLimitBytes));
            Assert.That(result.TimeLimitMilliseconds, Is.EqualTo(task.TimeLimitMilliseconds));
            Assert.That(result.Statement, Is.EqualTo(task.Statement));
            Assert.That(result.Name, Is.EqualTo(task.Name));
        }

        [Test]
        public void ClosedTaskStatementTest()
        {
            var task = new Task
            {
                MemoryLimitBytes = 1024000,
                TimeLimitMilliseconds = 5000,
                Name = "Task",
                Statement = "*bb*",
                TestsFolder = "Folder",
                IsOpened = false
            };
            _taskRepository.Stub(o => o.Get(1)).Return(task);
            var statement = _service.GetStatement(1, false);
            Assert.IsNull(statement);
        }
    }
}