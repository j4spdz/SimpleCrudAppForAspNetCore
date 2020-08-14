using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence
{
  public class DataContext : IdentityDbContext<AppUser>
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }

    static void ConfigureDBEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseModel
    {
      var entity = modelBuilder.Entity<TEntity>();
      entity.Property(e => e.CreateTime).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      var entityTypes = builder.Model
        .GetEntityTypes()
        .Where(t => t.ClrType.IsSubclassOf(typeof(BaseModel)));

      var configureMethod = GetType().GetTypeInfo().DeclaredMethods
        .Single(m => m.Name == nameof(ConfigureDBEntity));
      var args = new object[] { builder };
      foreach (var entityType in entityTypes)
        configureMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, args);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
      foreach (var entry in ChangeTracker.Entries<BaseModel>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.Entity.CreateTime = DateTime.UtcNow;
            entry.Entity.UpdateTime = DateTime.UtcNow;
            break;

          case EntityState.Modified:
            entry.Entity.UpdateTime = DateTime.UtcNow;
            break;

          case EntityState.Deleted:
            entry.State = EntityState.Modified;
            entry.CurrentValues["IsDeleted"] = true;
            break;
        }
      }
      return await base.SaveChangesAsync(cancellationToken);
    }
  }
}