using Domain.Entities.Common;

namespace Domain.Entities.Security
{
    public class Module : Entity<int>
    {
        public Module()
        {
            Permissions = new HashSet<Permission>();
        }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
