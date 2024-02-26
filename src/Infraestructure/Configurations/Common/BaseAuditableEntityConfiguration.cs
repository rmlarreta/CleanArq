using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Common
{
    public abstract class BaseAuditableEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Auditable
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Set key for entity
            builder.HasKey(p => p.Id);

            builder.Property(t => t.CreatedBy).IsRequired().HasMaxLength(250);
            builder.Property(t => t.ModifiedBy).HasMaxLength(250);
            builder.Property(t => t.CreatedDate);
        }
    }
}
