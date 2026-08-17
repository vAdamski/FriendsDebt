using FriendsDebt.Domain.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FriendsDebt.Persistence.Configurations;

public sealed class PlanMemberConfiguration : IEntityTypeConfiguration<PlanMember>
{
    public void Configure(EntityTypeBuilder<PlanMember> builder)
    {
        builder.HasKey(member => member.Id);
        builder.Property(member => member.Id).ValueGeneratedNever();

        builder.Property(member => member.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(member => member.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property<Guid>("PlanId")
            .IsRequired();

        builder.HasMany<Expense>()
            .WithOne(expense => expense.Member)
            .HasForeignKey(expense => expense.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
