using System;
using System.Transactions;
using NUnit.Framework;
using Judge.Data;
using Unity;

namespace Judge.Tests.Data.UnitOfWorkTests
{
    [TestFixture]
    public sealed class CommitTests
    {
        [Test]
        public void CommitTest()
        {
            using (var unitOfWork = new UnitOfWork(true, new UnityContainer(),
                () => new DataContext(string.Empty)))
            {
                Assert.That(Transaction.Current, Is.Not.Null);
                unitOfWork.Commit();
            }

            Assert.That(Transaction.Current, Is.Null);
        }

        [Test]
        public void CommitWithoutTransactionTest()
        {
            using (var unitOfWork = new UnitOfWork(false, new UnityContainer(),
                () => new DataContext(string.Empty)))
            {
                Assert.Throws<InvalidOperationException>(() => unitOfWork.Commit());
            }
        }
    }
}
