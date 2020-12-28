using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("FacesQuadra")]
    public class FaceQuadra
    {
        [Key]
        [Column("idFaceQuadra")]
        public int Id { get; set; }

        public string Inscricao { get; set; }

        public string Descricao { get; set; }

        public string Face { get; set; }

        public virtual ICollection<Fisico> Fisicos { get; set; }
    }
}
