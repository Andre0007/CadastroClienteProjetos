using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadastroClienteProjetos.Domain.Entities
{
    public class Email : IValidatableObject
    {
        public string Destino { get; set; }

        public string Assunto { get; set; }

        public string Mensagem { get; set; }

        public string MensagemErro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Destino))
                yield return new ValidationResult(MensagemErro = "O e-mail de Destino é um campo obrigatorio");
            else if (!string.IsNullOrEmpty(Assunto))
                yield return new ValidationResult(MensagemErro = "Assunto é um campo obrigatorio");
            else if (!string.IsNullOrEmpty(Mensagem))
                yield return new ValidationResult(MensagemErro = "Mensagem é um campo obrigatorio");
            else if (!Util.Validacao.IsEmail(Destino))
                yield return new ValidationResult(MensagemErro = "E-mail Inválido!!");
        }
    }
}
