using System;
using System.Security.Claims;
using Judge.Application;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Data;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Application.ContestsServiceTests
{
    [TestFixture]
    public class GetStatementTests
    {
        private IContestsService _service;
        private IContestTaskRepository _contestTaskRepository;
        private ISubmitRepository _submitRepository;
        private IContestsRepository _contestsRepository;

        [SetUp]
        public void SetUp()
        {
            var factory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();

            _contestTaskRepository = MockRepository.GenerateMock<IContestTaskRepository>();
            _submitRepository = MockRepository.GenerateMock<ISubmitRepository>();
            _contestsRepository = MockRepository.GenerateMock<IContestsRepository>();
            unitOfWork.Stub(o => o.ContestTaskRepository).Return(_contestTaskRepository);
            unitOfWork.Stub(o => o.SubmitRepository).Return(_submitRepository);
            unitOfWork.Stub(o => o.ContestsRepository).Return(_contestsRepository);

            factory.Stub(o => o.GetUnitOfWork()).Return(unitOfWork);

            _service = new ContestsService(factory, new ClaimsPrincipal());
        }

        [Test]
        public void TaskNotFoundTest()
        {
            var contest = new Contest();
            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            var result = _service.GetStatement(1, "A");

            Assert.IsNull(result);
        }

        [Test]
        public void ContestNotStartedTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.UtcNow.AddDays(2),
                FinishTime = DateTime.UtcNow.AddDays(3)
            };

            var task = new ContestTask
            {
                Task = new Model.CheckSolution.Task()
            };

            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            _contestTaskRepository.Stub(o => o.Get(1, "A")).Return(task);

            var result = _service.GetStatement(1, "A");

            Assert.That(result.Contest.IsNotStarted, Is.True);
            Assert.That(result.Contest.IsFinished, Is.False);
            Assert.That(result.Contest.Status, Is.EqualTo(ContestStatus.Planned));
        }

        [Test]
        public void ContestFinishedTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.UtcNow.AddDays(-2),
                FinishTime = DateTime.UtcNow.AddDays(-1)
            };

            var task = new ContestTask
            {
                Task = new Model.CheckSolution.Task()
            };

            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            _contestTaskRepository.Stub(o => o.Get(1, "A")).Return(task);

            var result = _service.GetStatement(1, "A");

            Assert.That(result.Contest.IsNotStarted, Is.False);
            Assert.That(result.Contest.IsFinished, Is.True);
            Assert.That(result.Contest.Status, Is.EqualTo(ContestStatus.Finished));
        }

        [Test]
        public void ContestStartedTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.UtcNow.AddDays(-2),
                FinishTime = DateTime.UtcNow.AddDays(1)
            };

            var task = new ContestTask
            {
                Task = new Model.CheckSolution.Task()
            };

            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            _contestTaskRepository.Stub(o => o.Get(1, "A")).Return(task);

            var result = _service.GetStatement(1, "A");

            Assert.That(result.Contest.IsNotStarted, Is.False);
            Assert.That(result.Contest.IsFinished, Is.False);
            Assert.That(result.Contest.Status, Is.EqualTo(ContestStatus.Started));
        }
    }
}