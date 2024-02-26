using Domain.Entities.Security;
using Infraestructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Security
{
    public class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            // Mapping for table
            builder.ToTable("Roles", "Security");

            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Description).HasMaxLength(256);
        }
    }
}
