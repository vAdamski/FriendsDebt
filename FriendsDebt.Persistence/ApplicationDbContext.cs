using System.Reflection;
using FriendsDebt.Application.Common.Interfaces;
using FriendsDebt.Application.Common.Interfaces.Persistence;
using FriendsDebt.Domain.Common;
using FriendsDebt.Domain.Enums;
using FriendsDebt.Domain.Plans;
using FriendsDebt.Domain.UserAccounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FriendsDebt.Persistence;

public sealed class ApplicationDbContext
    : IdentityDbContext<UserAccount, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    private readonly TimeProvider _timeProvider;
    private readonly ICurrentUserService? _currentUserService;

    public DbSet<Plan> Plans => Set<Plan>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        _timeProvider = TimeProvider.System;
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        TimeProvider timeProvider,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _timeProvider = timeProvider;
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<UserAccount>(user =>
        {
            user.Property(account => account.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

            user.Property(account => account.CreatedAtUtc)
                .IsRequired();
        });
    }

    public Task<int> SaveChangesAsync(
        string userEmail,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        UpdateAuditableEntities(userEmail.Trim());

        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (!HasPendingAuditableChanges())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        var userEmail = _currentUserService?.Email;
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            throw new InvalidOperationException(
                "An email is required to save auditable entities. " +
                "For a guest user call SaveChangesAsync(userEmail, cancellationToken).");
        }

        UpdateAuditableEntities(userEmail.Trim());

        return base.SaveChangesAsync(cancellationToken);
    }

    private bool HasPendingAuditableChanges() =>
        ChangeTracker
            .Entries<AuditableEntity>()
            .Any(entry => entry.State is
                EntityState.Added or
                EntityState.Modified or
                EntityState.Deleted);

    private void UpdateAuditableEntities(string userEmail)
    {
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated(userEmail, now);
                    break;

                case EntityState.Modified:
                    entry.Property(entity => entity.CreatedBy).IsModified = false;
                    entry.Property(entity => entity.Created).IsModified = false;
                    entry.Entity.SetModified(userEmail, now);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Property(entity => entity.CreatedBy).IsModified = false;
                    entry.Property(entity => entity.Created).IsModified = false;
                    entry.Entity.SetInactivated(userEmail, now);
                    break;
            }
        }
    }
}
