using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("RoteiroSelecaoItens")]
    public class RoteiroSelecaoItem
    {
        [Key]
        [Column("IdRoteiroSelecaoItem")]
        public int Id { get; set; }

        public int SelecaoId { get; set; }

        public int SelecionadoId { get; set; }

        public string Origem { get; set; }
    }
}
