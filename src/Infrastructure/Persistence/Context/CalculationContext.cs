using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Infrastructure.Persistence.Context
{
    public class CalculationContext : DbContext
    {
        public CalculationContext(DbContextOptions<CalculationContext> options) : base(options)  { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        public DbSet<RoteiroSelecaoItem> RoteiroSelecaoItens { get; set; }

        public DbSet<RoteiroSelecaoItemContribuinte> RoteiroSelecaoItemContribuintes { get; set; }

        public DbSet<RoteiroSelecaoItemFisico> RoteiroSelecaoItemFisicos { get; set; }

        public DbSet<Contribuinte> Contribuintes { get; set; }

        public DbSet<Fisico> Fisicos { get; set; }

        public DbSet<FaceQuadra> FacesQuadra { get; set; }
    }
}
