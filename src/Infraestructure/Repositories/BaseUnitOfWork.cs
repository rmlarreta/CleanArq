using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.SqlClient;

namespace Infraestructure.Repositories
{
    public abstract class BaseUnitOfWork(DbContext context) : IBaseUnitOfWork, IDisposable
    {
        protected readonly DbContext _context = context;
        private IDbContextTransaction? _currentTransaction;

        public async Task<int> CommitAsync()
        {
            try
            {

                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                if (sqlEx.Number == 4060 || sqlEx.Number == 10928)
                {
                    Thread.Sleep(10000);
                    return await _context.SaveChangesAsync();
                }
                else
                {
                    throw;
                }
            }
        }

        public void ClearContext()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public async Task<T?> TransactionallyDo<T>(Func<Task<T>> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var initializedTransaction = false;
            if (_currentTransaction is null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel);
                initializedTransaction = true;
            }
            T? result;
            try
            {
                result = await asyncAction();
                if (initializedTransaction)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                if (initializedTransaction)
                {
                    await _currentTransaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (initializedTransaction)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }

            return result;
        }

        public async Task TransactionallyDo(Func<Task> asyncAction, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var initializedTransaction = false;
            if (this._currentTransaction == null)
            {
                this._currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel);
                initializedTransaction = true;
            }
            try
            {
                await asyncAction();
                await CommitAsync();
            }
            catch (Exception)
            {
                if (initializedTransaction)
                {
                    await _currentTransaction.RollbackAsync();
                    _currentTransaction = null;
                }
                throw;
            }

            if (initializedTransaction)
            {
                await _currentTransaction.CommitAsync();
                _currentTransaction = null;
            }
        }

        public async Task TransactionallyDo(Func<Task> asyncAction, bool useExecutionStrategy, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (useExecutionStrategy)
            {
                var executionStrategy = _context.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
                    try
                    {
                        await asyncAction();
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            else
            {
                await TransactionallyDo(asyncAction, isolationLevel);
            }
        }

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}