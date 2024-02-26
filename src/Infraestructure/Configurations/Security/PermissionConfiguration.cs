using Domain.Entities.Security;
using Infraestructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Security
{
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);

            // Mapping for table
            builder.ToTable("Permissions", "Security");

            builder.Property(t => t.Name).IsRequired().HasMaxLength(256);

            // Relationships
            builder.HasOne(p => p.Module)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
