using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public abstract class AuditableEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : AuditableEntity
  {
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
      builder.Property(e => e.Created).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
      builder.Property(e => e.CreatedBy).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
  }
}