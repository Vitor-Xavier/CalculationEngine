using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Fisicos")]
    public class Fisico
    {
        [Key]
        [Column("IdFisico")]
        public int Id { get; set; }

        [Column("areaTerreno", TypeName = "decimal(9,2)")]
        public decimal AreaTerreno { get; set; }

        [Column("areaEdificada", TypeName = "decimal(9,2)")]
        public decimal AreaEdificada { get; set; }

        [Column("idFaceQuadra")]
        public int FaceQuadraId { get; set; }

        public virtual FaceQuadra FaceQuadra { get; set; }

        public virtual ICollection<RoteiroSelecaoItemFisico> RoteiroSelecaoItens { get; set; }
    }
}
