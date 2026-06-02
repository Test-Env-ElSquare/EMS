namespace DAL.Context.Confegrations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Configure base Identity properties if needed
            builder.Property(u => u.FullName)
                .HasMaxLength(250);

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.DeletedAt)
                .IsRequired(false);

            // Relationships

            //builder.HasMany(u => u.PerformedVisits)
            //       .WithOne(tv => tv.PerformedByUser)
            //       .HasForeignKey(tv => tv.PerformedByUserId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(u => u.AssignedTickets)
            //       .WithOne(t => t.AssignedToUser)
            //       .HasForeignKey(t => t.AssignedToUserId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(u => u.OpenedTickets)
            //       .WithOne(t => t.OpenedByUser)
            //       .HasForeignKey(t => t.OpenedByUserId)
            //       .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
