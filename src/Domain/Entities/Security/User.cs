using Domain.Entities.Common;

namespace Domain.Entities.Security
{
    public class User : Auditable
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            Permissions = Enumerable.Empty<Permission>();
        }

        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public int MinutesFromUtc { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
