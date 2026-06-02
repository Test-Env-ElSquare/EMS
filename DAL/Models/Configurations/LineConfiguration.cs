using Domain.Models.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Models.Configurations
{
    // LineConfiguration.cs
    public class LineConfiguration : IEntityTypeConfiguration<Line>
    {
        public void Configure(EntityTypeBuilder<Line> builder)
        {
            builder
                .HasOne(l => l.Zone)
                .WithMany(z => z.Lines)
                .HasForeignKey(l => l.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
