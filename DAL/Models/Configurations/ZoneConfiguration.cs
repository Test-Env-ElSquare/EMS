using DAL.Models.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Models.Configurations
{
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder
           .HasOne(z => z.Factory)
           .WithMany(f => f.Zones)
           .HasForeignKey(z => z.FactoryId)
           .OnDelete(DeleteBehavior.Restrict);

            builder
           .HasOne(z => z.Transformer)
           .WithMany(t => t.Zones)
           .HasForeignKey(z => z.TransformerId)
           .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
