using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroClienteProjetos.Domain.Entities
{
    [Table("tb_Cliente")]
    public class Cliente : IValidatableObject
    {
        [Key, Column("idCliente")]
        public int? id { get; set; }

        [Column("strRazaoSocial")]
        public string razaoSocial { get; set; }

        [Column("strCNPJ")]
        public string cnpj { get; set; }

        [Column("dtDataInscricao")]
        public DateTime dataInscricao { get; set; }

        [NotMapped]
        public string MensagemErro { get; set; }

        public virtual IEnumerable<Projeto> Projeto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (razaoSocial == "")
                yield return new ValidationResult(MensagemErro = "Razao Social é um campo obrigatorio");
            else if(cnpj == "")
                yield return new ValidationResult(MensagemErro = "cnpj é um campo obrigatorio");
            else if(Util.Validacao.CNPJ(cnpj) == false)
                yield return new ValidationResult(MensagemErro = "CNPJ Invalido.");
        }

    }
}