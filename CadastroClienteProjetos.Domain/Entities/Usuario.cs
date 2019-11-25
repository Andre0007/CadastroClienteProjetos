using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroClienteProjetos.Domain.Entities
{
    [Table("tb_Usuario")]
    public class Usuario : IValidatableObject
    {
        [Key, Column("idUsuario")]
        public int? id { get; set; }

        [Column("strNomeUsuario")]
        public string nomeUsuario { get; set; }

        [Column("strEmail")]
        public string email { get; set; }

        [Column("pwSenha")]
        public string senha { get; set; }

        [Column("strCPF")]
        public string cpf { get; set; }

        [Column("strPis")]
        public string pis { get; set; }

        [Column("dtDataInscricao")]
        public DateTime dataInscricao { get; set; }

        [NotMapped]
        public string MensagemErro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(nomeUsuario))
                yield return new ValidationResult(MensagemErro = "Nome de usuario é um campo obrigatorio");
            else if (!string.IsNullOrEmpty(email))
                yield return new ValidationResult(MensagemErro = "Email é um campo obrigatorio");
            else if (!Util.Validacao.IsEmail(email))
                yield return new ValidationResult(MensagemErro = "Email Inválido!!");
            else if (!string.IsNullOrEmpty(senha))
                yield return new ValidationResult(MensagemErro = "Senha é um campo obrigatorio");
            else if (!string.IsNullOrEmpty(cpf))
                yield return new ValidationResult(MensagemErro = "CPF é um campo obrigatorio");
            else if (!Util.Validacao.IsCPF(cpf))
                yield return new ValidationResult(MensagemErro = "CPF Inválido!!");
            else if (!string.IsNullOrEmpty(pis))
                yield return new ValidationResult(MensagemErro = "PIS é um campo obrigatorio");
            else if (!Util.Validacao.IsPis(pis))
                yield return new ValidationResult(MensagemErro = "PIS Inválido!!");
        }
    }
}