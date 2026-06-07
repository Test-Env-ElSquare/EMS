using DAL.Models.Calculated.Historical;
using DAL.Models.Calculated.Views;
using DAL.Models.Configurations;
using DAL.Models.Definitions;
using DAL.Models.RealTime;
using DAL.Models.Threshold;
using Domain.Models.Definitions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL.Context
{
    public class EmsContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public EmsContext(DbContextOptions<EmsContext> options) : base(options)
        {
        }


        //Db Sets
        #region Threshold
        public DbSet<EnergyHeatmapThreshold> EnergyHeatmapThresholds { get; set; }
        #endregion
        #region Identity
        public DbSet<SystemClaim> SystemClaims { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }

        #endregion

        #region  RealTime   
        public DbSet<Energy> Energies { get; set; }
        public DbSet<Signal> Signals { get; set; }
        public DbSet<MachineLoad> MachineLoads { get; set; }
        #endregion
        #region  Definitions   
        public DbSet<Transformer> Transformers { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<LineTransformer> LineTransformers { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineShift> MachineShifts { get; set; }
        public DbSet<SKU> SKUs { get; set; }
        #endregion
        #region  Calculated   

        //Transformers
        public DbSet<VW_TransformerAnalysis> VW_TransformerAnalysis { get; set; }
        public DbSet<VW_TransformerHourlyAnalysis> VW_TransformerHourlyAnalysis { get; set; }
        public DbSet<TransformerAnalysis> TransformerAnalysis { get; set; }
        public DbSet<TransformerHourlyAnalysis> TransformerHourlyAnalysis { get; set; }

        //Zones
        public DbSet<VW_ZoneAnalysis> VW_ZoneAnalysis { get; set; }
        public DbSet<VW_ZoneHourlyAnalysis> VW_ZoneHourlyAnalysis { get; set; }
        public DbSet<ZoneAnalysis> ZoneAnalysis { get; set; }
        public DbSet<ZoneHourlyAnalysis> ZoneHourlyAnalysis { get; set; }

        //Lines
        public DbSet<VW_LineAnalysis> VW_LineAnalysis { get; set; }
        public DbSet<VW_LineHourlyAnalysis> VW_LineHourlyAnalysis { get; set; }
        public DbSet<LineAnalysis> LineAnalysis { get; set; }
        public DbSet<LineHourlyAnalysis> LineHourlyAnalysis { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.ApplyConfiguration(new LineTransfornerConfigurations());
            modelBuilder.ApplyConfiguration(new ZoneConfiguration());
            modelBuilder.ApplyConfiguration(new LineConfiguration());


            // any Views

            modelBuilder.Entity<VW_TransformerAnalysis>().HasNoKey().ToView("VW_TransformerAnalysis", schema: "Calculated");
            modelBuilder.Entity<VW_TransformerHourlyAnalysis>().HasNoKey().ToView("VW_TransformerHourlyAnalysis", schema: "Calculated");

            modelBuilder.Entity<VW_TransformerAnalysis>().HasNoKey().ToView("VW_ZoneAnalysis", schema: "Calculated");
            modelBuilder.Entity<VW_TransformerHourlyAnalysis>().HasNoKey().ToView("VW_ZoneHourlyAnalysis", schema: "Calculated");

            modelBuilder.Entity<VW_TransformerAnalysis>().HasNoKey().ToView("VW_LineAnalysis", schema: "Calculated");
            modelBuilder.Entity<VW_TransformerHourlyAnalysis>().HasNoKey().ToView("VW_LineHourlyAnalysis", schema: "Calculated");


            modelBuilder.Entity<TransformerHourlyAnalysis>().HasNoKey();
            modelBuilder.Entity<TransformerAnalysis>().HasNoKey();

            modelBuilder.Entity<ZoneHourlyAnalysis>().HasNoKey();
            modelBuilder.Entity<ZoneAnalysis>().HasNoKey();

            modelBuilder.Entity<LineHourlyAnalysis>().HasNoKey();
            modelBuilder.Entity<LineAnalysis>().HasNoKey();

            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }

    }


}
