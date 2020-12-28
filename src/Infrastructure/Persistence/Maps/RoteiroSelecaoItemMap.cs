using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Infrastructure.Persistence.Maps
{
    public class RoteiroSelecaoItemMap : IEntityTypeConfiguration<RoteiroSelecaoItem>
    {
        public void Configure(EntityTypeBuilder<RoteiroSelecaoItem> builder)
        {
            builder.HasDiscriminator(e => e.Origem)
                .HasValue<RoteiroSelecaoItemContribuinte>("Contribuinte")
                .HasValue<RoteiroSelecaoItemFisico>("Fisico");
        }
    }
}
