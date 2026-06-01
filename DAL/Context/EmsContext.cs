using DAL.Models.Identity;
using DAL.Models.Calculated.Historical;
using DAL.Models.Calculated.Views;
using DAL.Models.Definitions;
using DAL.Models.RealTime;
using Domain.Models.Definitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class EmsContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole,IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>>
    {
        public EmsContext(DbContextOptions<EmsContext> options) : base(options)
        {
        }


        //Db Sets
        #region Identity
        public DbSet<SystemClaim> SystemClaims { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }

        #endregion

        #region  RealTime   
        public DbSet<Energy> Energies { get; set; }
        #endregion
        #region  Definitions   
        public DbSet<Transformer> Transformers { get; set; }
        public DbSet<LineTransformer> LineTransformers { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Machine> Machines { get; set; }
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

            // any Views

            modelBuilder.Entity<VW_TransformerAnalysis>().HasNoKey().ToView("VW_TransformerAnalysis", schema: "Calculated");
            modelBuilder.Entity<VW_TransformerHourlyAnalysis>().HasNoKey().ToView("VW_TransformerHourlyAnalysis", schema: "Calculated");
            modelBuilder.Entity<TransformerHourlyAnalysis>().HasNoKey();
            modelBuilder.Entity<TransformerAnalysis>().HasNoKey();
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
