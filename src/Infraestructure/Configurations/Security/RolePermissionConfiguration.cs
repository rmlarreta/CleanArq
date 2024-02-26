using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Security
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolesPermissions", "Security");

            // Primary Key
            builder.HasKey(p => new { p.RoleId, p.PermissionId });

            // Relationships
            builder.HasOne(p => p.Role)
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Permission)
                   .WithMany(x => x.RolePermissions)
                   .HasForeignKey(x => x.PermissionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
