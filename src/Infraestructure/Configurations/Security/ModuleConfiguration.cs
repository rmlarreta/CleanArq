using Domain.Entities.Security;
using Infraestructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Security
{
    public class ModuleConfiguration : BaseEntityConfiguration<Module>
    {
        public override void Configure(EntityTypeBuilder<Module> builder)
        {
            base.Configure(builder);

            // Mapping for table
            builder.ToTable("Modules", "Security");

            builder.Property(t => t.Code).HasMaxLength(50);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(255);
        }
    }
}
