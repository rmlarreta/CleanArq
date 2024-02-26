using System.Data;

namespace Domain.Repositories
{
    public interface IBaseUnitOfWork
    {
        Task<T> TransactionallyDo<T>(Func<Task<T>> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task TransactionallyDo(Func<Task> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<int> CommitAsync();
        Task<T> TransactionallyDo<T>(Func<Task<T>> asyncAction, bool useExecutionStrategy, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task TransactionallyDo(Func<Task> asyncAction, bool useExecutionStrategy, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void ClearContext();
    }
}
