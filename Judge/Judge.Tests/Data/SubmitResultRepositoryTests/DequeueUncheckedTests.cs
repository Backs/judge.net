using System.Configuration;
using System.Transactions;
using Judge.Data;
using Judge.Model.SubmitSolution;
using NUnit.Framework;
using SimpleInjector;

namespace Judge.Tests.Data.SubmitResultRepositoryTests
{
    [TestFixture]
    [Category("Database")]
    public class DequeueUncheckedTests
    {
        private Container _container;
        private TransactionScope _transaction;

        [SetUp]
        public void SetUp()
        {
            _container = new Container();
            var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

            new DataContainerExtension(connectionString, Lifestyle.Singleton).Configure(_container);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
            _transaction.Dispose();
        }

        [Test]
        public void DequeueTest()
        {
            var factory = _container.GetInstance<IUnitOfWorkFactory>();
            long submitId;
            using (var uow = factory.GetUnitOfWork())
            {
                var submitRepository = uow.Submits;
                var submit = ProblemSubmit.Create();
                submit.FileName = "main.cpp";
                submit.LanguageId = 1;
                submit.ProblemId = 1;
                submit.SourceCode = "123";
                submit.UserId = 1;
                submitRepository.Add(submit);
                uow.Commit();
                submitId = submit.Id;
            }

            using (var uow = factory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResults;
                var submit = submitResultRepository.DequeueUnchecked();

                Assert.NotNull(submit);
                Assert.That(submit.Submit.Id, Is.EqualTo(submitId));
            }
        }
    }
}
