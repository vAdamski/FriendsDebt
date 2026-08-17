using FriendsDebt.Domain.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FriendsDebt.Persistence.Configurations;

public sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasKey(plan => plan.Id);
        builder.Property(plan => plan.Id).ValueGeneratedNever();

        builder.Property(plan => plan.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasMany(plan => plan.Expenses)
            .WithOne(expense => expense.Plan)
            .HasForeignKey(expense => expense.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(plan => plan.Members)
            .WithOne()
            .HasForeignKey("PlanId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(plan => plan.Expenses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(plan => plan.Members)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
