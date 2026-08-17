using FriendsDebt.Domain.Plans;
using Microsoft.EntityFrameworkCore;

namespace FriendsDebt.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<Plan> Plans { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(
        string userEmail,
        CancellationToken cancellationToken = default);
}
