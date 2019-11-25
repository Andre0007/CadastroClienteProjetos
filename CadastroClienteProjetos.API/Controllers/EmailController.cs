using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        /// <summary>
        /// Serviço de Envio de emails para qualquer necessidade de uso tipo(erro gerado, fale conosco, bem vindo usuario)
        /// http://www.macoratti.net/18/04/aspcoremvc_email1.htm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EnviaEmail(Email email)
        {
            var mensagemErro = "";

            if (!ModelState.IsValid)
                return BadRequest(email.MensagemErro);
            else
            {
                try
                {
                    _emailSender.SendEmailAsync(email);
                }
                catch (Exception)
                {
                    mensagemErro = "Email Falhou";
                }
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(email);
        }
    }
}