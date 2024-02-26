using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Configurations.Common
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity<int>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
