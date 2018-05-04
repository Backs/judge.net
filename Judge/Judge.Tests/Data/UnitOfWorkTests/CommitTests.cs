using System;
using System.Transactions;
using NUnit.Framework;
using Judge.Data;
using SimpleInjector;

namespace Judge.Tests.Data.UnitOfWorkTests
{
    [TestFixture]
    public sealed class CommitTests
    {
        [Test]
        public void CommitTest()
        {
            using (var unitOfWork = new UnitOfWork(true, new Container(), new DataContext("asd")))
            {
                Assert.That(Transaction.Current, Is.Not.Null);
                unitOfWork.Commit();
            }

            Assert.That(Transaction.Current, Is.Null);
        }

        [Test]
        public void CommitWithoutTransactionTest()
        {
            using (var unitOfWork = new UnitOfWork(false, new Container(), new DataContext("asd")))
            {
                Assert.Throws<InvalidOperationException>(() => unitOfWork.Commit());
            }
        }
    }
}
