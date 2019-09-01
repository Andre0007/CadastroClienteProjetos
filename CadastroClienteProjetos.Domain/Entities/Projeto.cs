using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroClienteProjetos.Domain.Entities
{
    [Table("tb_projeto")]
    public class Projeto : IValidatableObject
    {
        [Key, Column("idProjeto")]
        public int? id { get; set; }

        [Column("strNomeProjeto")]
        public string nomeProjeto { get; set; }

        [Column("dtDataInscricao")]
        public DateTime dataInscricao { get; set; }

        public int? idCliente { get; set; }

        [ForeignKey("idCliente")]
        public virtual Cliente Cliente { get; set; }

        [NotMapped]
        public string MensagemErro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (nomeProjeto == "")
                yield return new ValidationResult(MensagemErro = "Nome do projeto é um campo obrigatorio");
            else if (idCliente == null)
                yield return new ValidationResult(MensagemErro = "DropDown Cliente é um campo obrigatorio");
        }
    }
}