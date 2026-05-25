using Domain.Models.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Models.Configurations
{
    public class LineTransfornerConfigurations : IEntityTypeConfiguration<LineTransformer>
    {
        public void Configure(EntityTypeBuilder<LineTransformer> builder)
        {
            builder.HasKey(lt => new { lt.LineId, lt.TransformerId });

            builder.HasOne(lt => lt.Line)
                   .WithMany(l => l.LineTransformers)
                   .HasForeignKey(lt => lt.LineId);

            builder.HasOne(lt => lt.Transformer)
                   .WithMany(t => t.LineTransformers)
                   .HasForeignKey(lt => lt.TransformerId);

        }
    }
}
