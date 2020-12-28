using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Infrastructure.Persistence.Maps
{
    public class RoteiroSelecaoItemContribuinteMap : IEntityTypeConfiguration<RoteiroSelecaoItemContribuinte>
    {
        public void Configure(EntityTypeBuilder<RoteiroSelecaoItemContribuinte> builder)
        {
            builder.HasOne(e => e.Contribuinte)
                .WithMany(e => e.RoteiroSelecaoItens)
                .HasForeignKey(e => e.SelecionadoId)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
