using Infraestructure.Interfaces;
using Infraestructure.Repositories;

namespace Infraestructure.Data
{
    //private IUserRepository _users;
    public class UnitOfWork(DefaultContext context) : BaseUnitOfWork(context), IUnitOfWork
    {
        // public IUserRepository Users => _users ??= new UserRepository(_context);

    }
}
