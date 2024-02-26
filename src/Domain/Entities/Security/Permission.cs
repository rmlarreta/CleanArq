using Domain.Entities.Common;

namespace Domain.Entities.Security
{
    public class Permission : Entity<int>
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; } = null!;
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
