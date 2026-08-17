using FriendsDebt.Domain.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FriendsDebt.Persistence.Configurations;

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(expense => expense.Id);
        builder.Property(expense => expense.Id).ValueGeneratedNever();

        builder.Property(expense => expense.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(expense => expense.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(expense => expense.PlanId)
            .IsRequired();

        builder.Property(expense => expense.MemberId)
            .IsRequired();
    }
}
