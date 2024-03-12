using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crea_CaSdkComercial;
using SdkCreaComercial.Code.Business;

namespace SdkCreaComercial.Code.Data
{
    public class SdkDbContext : DbContext
    {
        public SdkDbContext() : base(Settings.Default.ConnectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogError>().ToTable("log_Error")
                .HasKey<int>(e => e.ErrorId);

            modelBuilder.Entity<LogActivity>().ToTable("log_Activity")
                .HasKey<int>(e => e.ActivityId);
        }

        public DbSet<LogError> LogErrors { get; set; }
        public DbSet<LogActivity> LogActivities { get; set; }
    }
}
