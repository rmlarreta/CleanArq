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
    }
}
