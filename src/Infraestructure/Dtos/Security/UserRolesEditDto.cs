namespace Infraestructure.Dtos.Security
{
    public class UserRolesEditDto
    {
        public int UserId { get; set; }

        public int[]? RolesId { get; set; }
    }
}
