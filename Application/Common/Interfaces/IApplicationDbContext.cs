using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces
{
  public interface IApplicationDbContext
  {
    DbSet<AppUser> Users { get; set; }
    DbSet<Activity> Activities { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Entry(object entity);
    EntityEntry Remove(object entity);
  }
}