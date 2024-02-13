using System.Data;

namespace Infraestructure.Interfaces
{
    public interface IBaseUnitOfWork
    {
        Task TransactionallyDo(Func<Task> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task<T?> TransactionallyDo<T>(Func<Task<T>> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task TransactionallyDo(Func<Task> asyncAction, bool useExecutionStrategy, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void ClearContext();

        Task<int> CommitAsync();
    }
}
