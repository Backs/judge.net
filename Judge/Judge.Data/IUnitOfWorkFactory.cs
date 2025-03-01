namespace Judge.Data;

public interface IUnitOfWorkFactory
{
    IUnitOfWork GetUnitOfWork();

    IUnitOfWork GetUnitOfWork(bool startTransaction);
}