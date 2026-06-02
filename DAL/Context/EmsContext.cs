using DAL.Models.Auth;
using DAL.Models.Calculated.Historical;
using DAL.Models.Calculated.Views;
using DAL.Models.Configurations;
using DAL.Models.Definitions;
using DAL.Models.RealTime;
using Domain.Models.Definitions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class EmsContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public EmsContext(DbContextOptions<EmsContext> options) : base(options)
        {
        }


        //Db Sets
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

        public DbSet<VW_TransformerAnalysis> VW_TransformerAnalysis { get; set; }
        public DbSet<VW_TransformerHourlyAnalysis> VW_TransformerHourlyAnalysis { get; set; }
        public DbSet<TransformerAnalysis> TransformerAnalysis { get; set; }
        public DbSet<TransformerHourlyAnalysis> TransformerHourlyAnalysis { get; set; }


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
            modelBuilder.Entity<TransformerHourlyAnalysis>().HasNoKey();
            modelBuilder.Entity<TransformerAnalysis>().HasNoKey();
        }

    }


}
