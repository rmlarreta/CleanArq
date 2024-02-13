using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Providers
{
    public interface IContextProvider
    {
        string Username { get; }
        string Email { get; }
        int UserId { get; }
        int[] RolesId { get; }
        int[] PermissionsId { get; }
        int MinutesFromUtc { get; }
        int? AccountId { get; }
        int? SelectedAccountId { get; }
        int? OperationAccountId { get; }
    }
}
