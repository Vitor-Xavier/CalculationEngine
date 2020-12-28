using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Infrastructure.Persistence.Maps
{
    public class RoteiroSelecaoItemFisicoMap : IEntityTypeConfiguration<RoteiroSelecaoItemFisico>
    {
        public void Configure(EntityTypeBuilder<RoteiroSelecaoItemFisico> builder)
        {
            builder.HasOne(e => e.Fisico)
                .WithMany(e => e.RoteiroSelecaoItens)
                .HasForeignKey(e => e.SelecionadoId)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
