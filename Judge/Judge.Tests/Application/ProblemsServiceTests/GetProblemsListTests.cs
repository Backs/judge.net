using Judge.Application;
using Judge.Application.Interfaces;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;
using Judge.Model;

namespace Judge.Tests.Application.ProblemsServiceTests
{
    [TestFixture]
    internal sealed class GetProblemsListTests
    {
        private IProblemsService _service;
        private ITaskNameRepository _taskRepository;
        private ISubmitResultRepository _submitResultRepository;

        [SetUp]
        public void SetUp()
        {
            var unitOfWorkFactory = MockRepository.GenerateMock<IUnitOfWorkFactory>();
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();

            _taskRepository = MockRepository.GenerateMock<ITaskNameRepository>();
            _submitResultRepository = MockRepository.GenerateMock<ISubmitResultRepository>();

            unitOfWork.Stub(o => o.TaskNameRepository).Return(_taskRepository);
            unitOfWork.Stub(o => o.SubmitResultRepository).Return(_submitResultRepository);

            unitOfWorkFactory.Stub(o => o.GetUnitOfWork()).Return(unitOfWork);

            _service = new ProblemsService(unitOfWorkFactory);
        }

        [Test]
        public void FirstPageTest()
        {
            const int pageSize = 100;
            const int page = 1;

            var tasks = new[] { new TaskName { Id = 1, Name = "A" }, new TaskName { Id = 2, Name = "B" } };
            _taskRepository.Stub(o => o.GetTasks(AllTasksSpecification.Instance, page, pageSize)).Return(tasks);
            _taskRepository.Stub(o => o.Count()).Return(tasks.Length);

            var model = _service.GetProblemsList(page, pageSize, null, true);

            Assert.That(model.ProblemsCount, Is.EqualTo(2));
            Assert.That(model.Pagination.PageSize, Is.EqualTo(pageSize));
            Assert.That(model.Pagination.CurrentPage, Is.EqualTo(page));
            Assert.That(model.Pagination.TotalPages, Is.EqualTo(1));

            CollectionAssert.AreEqual(tasks.Select(o => new { o.Id, o.Name }), model.Problems.Select(o => new { o.Id, o.Name }));
        }

        [Test]
        public void TotalPagesTest()
        {
            var tasks = new[]
            {
                new TaskName { Id = 1, Name = "A" },
                new TaskName { Id = 2, Name = "B" },
                new TaskName { Id = 3, Name = "C" }
            };
            _taskRepository.Stub(o => o.GetTasks(AllTasksSpecification.Instance, 1, 2)).Return(tasks.Take(2));
            _taskRepository.Stub(o => o.Count()).Return(tasks.Length);

            var model = _service.GetProblemsList(1, 2, null, true);

            Assert.That(model.Pagination.CurrentPage, Is.EqualTo(1));
            Assert.That(model.Pagination.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void SecondPageTest()
        {
            var tasks = new[]
            {
                new TaskName { Id = 1, Name = "A" },
                new TaskName { Id = 2, Name = "B" },
                new TaskName { Id = 3, Name = "C" },
                new TaskName { Id = 4, Name = "D" },
                new TaskName { Id = 5, Name = "E" },
            };
            _taskRepository.Stub(o => o.GetTasks(AllTasksSpecification.Instance, 2, 2)).Return(tasks.Skip(2).Take(2));
            _taskRepository.Stub(o => o.Count()).Return(tasks.Length);

            var model = _service.GetProblemsList(2, 2, null, true);

            Assert.That(model.Pagination.CurrentPage, Is.EqualTo(2));
            Assert.That(model.Pagination.TotalPages, Is.EqualTo(3));

            CollectionAssert.AreEqual(tasks.Skip(2).Take(2).Select(o => new { o.Id, o.Name }), model.Problems.Select(o => new { o.Id, o.Name }));
        }

        [Test]
        public void SolvedProblemsTest()
        {
            const long userId = 5;
            const long solvedTaskId = 1;

            var tasks = new[]
            {
                new TaskName { Id = solvedTaskId, Name = "A" },
                new TaskName { Id = 2, Name = "B" },
                new TaskName { Id = 3, Name = "C" }
            };

            _taskRepository.Stub(o => o.GetTasks(AllTasksSpecification.Instance, 1, 2)).Return(tasks);
            _taskRepository.Stub(o => o.Count()).Return(tasks.Length);

            _submitResultRepository.Stub(o => o.GetSolvedProblems(Arg<ISpecification<SubmitResult>>.Is.Anything)).Return(new[] { solvedTaskId });

            var model = _service.GetProblemsList(1, 2, userId, true);

            CollectionAssert.AreEqual(tasks.Select(o => new { o.Id, o.Name, Solved = o.Id == solvedTaskId }), model.Problems.Select(o => new { o.Id, o.Name, o.Solved }));
        }
    }
}
