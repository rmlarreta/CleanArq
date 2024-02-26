using Domain.Entities.Security;
using Infraestructure.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Security
{
    public class UserConfiguration : BaseAuditableEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            // Mapping for table
            builder.ToTable("Users", "Security");

            builder.Property(t => t.FullName).IsRequired().HasMaxLength(256);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(256);

            builder.Ignore(t => t.Permissions);

            builder.HasIndex(p => new { p.Email, p.DeletedDate })
                   .IsUnique()
                   .HasDatabaseName("UX_User_Email");
        }
    }
}
