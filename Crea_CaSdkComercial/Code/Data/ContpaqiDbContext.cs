using System.Data.Entity;
using Crea_CaSdkComercial.Code.Contpaqi;

namespace Crea_CaSdkComercial.Code.Data
{
    class ContpaqiDbContext: DbContext
    {
        public ContpaqiDbContext(string baseDatos) : base(Settings.Default.ConnectionStringContpaq.Replace("#BASEDATOS#", baseDatos))
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmFoliosDigitales>().ToTable("admFoliosDigitales")
                .HasKey<int>(e => e.CIDFOLDIG);

            modelBuilder.Entity<AdmClientes>().ToTable("admClientes")
                .HasKey<int>(e => e.CIDCLIENTEPROVEEDOR);

            modelBuilder.Entity<AdmCuentasBancarias>().ToTable("admCuentasBancarias")
                .HasKey<int>(e => e.CIDCUENTA);
        }

        public DbSet<AdmFoliosDigitales> FoliosDigitaleses { get; set; }
        public DbSet<AdmClientes> Clientes { get; set; }
        public DbSet<AdmCuentasBancarias> CuentasBancarias { get; set; }
    }
}
