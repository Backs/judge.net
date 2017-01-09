namespace Judge.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork(bool transactionRequired);
    }
}
