namespace DAL.Context.Confegrations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //builder.ToTable("AspNetRoles", "Definitions");

            builder.Property(r => r.Name)
                   .HasMaxLength(256);

            builder.Property(r => r.NormalizedName)
                   .HasMaxLength(256);

            // Relationship: ApplicationRole ↔ RoleZone (Many-to-Many)
            //builder.HasMany(r => r.RoleZones)
            //       .WithOne(rz => rz.Role)
            //       .HasForeignKey(rz => rz.RoleId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
