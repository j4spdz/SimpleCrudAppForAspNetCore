using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
  {
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(DbContextOptions options, ICurrentUserService currentUserService) : base(options)
    {
      _currentUserService = currentUserService;
    }

    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
      foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.Entity.CreatedBy = _currentUserService.GetCurrentUsername() ?? "system";
            entry.Entity.Created = DateTime.UtcNow;
            break;

          case EntityState.Modified:
            entry.Entity.LastModifiedBy = _currentUserService.GetCurrentUsername() ?? "system";
            entry.Entity.LastModified = DateTime.UtcNow;
            break;
        }
      }
      return await base.SaveChangesAsync(cancellationToken);
    }
  }
}