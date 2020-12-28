using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Contribuintes")]
    public class Contribuinte
    {
        [Key]
        [Column("crc")]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string CPFCNPJ { get; set; }

        public TipoPessoa Tipo { get; set; }

        public DateTime DataNascimento { get; set; }

        public virtual ICollection<RoteiroSelecaoItemContribuinte> RoteiroSelecaoItens { get; set; }
    }
}
