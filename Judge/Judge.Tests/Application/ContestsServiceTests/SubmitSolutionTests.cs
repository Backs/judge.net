using System;
using Judge.Application;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels;
using Judge.Data;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Application.ContestsServiceTests
{
    [TestFixture]
    public class SubmitSolutionTests
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
            unitOfWork.Stub(o => o.GetRepository<IContestTaskRepository>()).Return(_contestTaskRepository);
            unitOfWork.Stub(o => o.GetRepository<ISubmitRepository>()).Return(_submitRepository);
            unitOfWork.Stub(o => o.GetRepository<IContestsRepository>()).Return(_contestsRepository);

            factory.Stub(o => o.GetUnitOfWork(Arg<bool>.Is.Anything)).Return(unitOfWork);

            _service = new ContestsService(factory);
        }

        [Test]
        public void TaskNotFoundTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.Now.AddDays(-1),
                FinishTime = DateTime.Now.AddDays(1)
            };
            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            var userInfo = new UserInfo(9, null, null);
            var ex = Assert.Throws<InvalidOperationException>(() => _service.SubmitSolution(1, "A", 2, new FakeHttpPostedFileBase(), userInfo));

            Assert.That(ex.Message, Is.EqualTo("Task not found"));
        }

        [Test]
        public void ContestNotStartedTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.Now.AddDays(1),
                FinishTime = DateTime.Now.AddDays(2)
            };
            _contestsRepository.Stub(o => o.Get(1)).Return(contest);

            var userInfo = new UserInfo(9, null, null);
            var ex = Assert.Throws<InvalidOperationException>(() => _service.SubmitSolution(1, "A", 2, new FakeHttpPostedFileBase(), userInfo));

            Assert.That(ex.Message, Is.EqualTo("Contest not started"));
        }

        [Test]
        public void ContestFinishedTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.Now.AddDays(-2),
                FinishTime = DateTime.Now.AddDays(-1)
            };
            _contestsRepository.Stub(o => o.Get(1)).Return(contest);

            var userInfo = new UserInfo(9, null, null);
            var ex = Assert.Throws<InvalidOperationException>(() => _service.SubmitSolution(1, "A", 2, new FakeHttpPostedFileBase(), userInfo));

            Assert.That(ex.Message, Is.EqualTo("Contest finished"));
        }

        [Test]
        public void SubmitTest()
        {
            var contest = new Contest
            {
                StartTime = DateTime.Now.AddDays(-2),
                FinishTime = DateTime.Now.AddDays(2)
            };
            _contestsRepository.Stub(o => o.Get(1)).Return(contest);
            var contestTask = new ContestTask
            {
                Task = new Model.CheckSolution.Task()
            };
            _contestTaskRepository.Stub(o => o.Get(1, "A")).Return(contestTask);

            var userInfo = new UserInfo(9, null, null);
            _service.SubmitSolution(1, "A", 2, new FakeHttpPostedFileBase(), userInfo);

            _submitRepository.AssertWasCalled(o => o.Add(Arg<ContestTaskSubmit>.Is.Anything));
        }
    }
}
