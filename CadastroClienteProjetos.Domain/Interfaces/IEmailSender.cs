using CadastroClienteProjetos.Domain.Entities;
using System.Threading.Tasks;

namespace CadastroClienteProjetos.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Email email);
    }
}