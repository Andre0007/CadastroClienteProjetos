using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepository<Usuario> _repository;
        public UsuarioController(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Usuario> GetByOne(int id)
        {
            Usuario usuario = null;
            var mensagemErro = "";

            try
            {
                usuario = _repository.GetById(id);
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            return _repository.GetAll().ToList();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Usuario> Post(Usuario usuario)
        {
            var mensagemErro = "";

            if (!ModelState.IsValid)
            {
                mensagemErro = usuario.MensagemErro;
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    usuario.senha = Domain.Util.Security.EncodePassword(usuario.senha.Trim());
                    _repository.Insert(usuario);
                    var result = _repository.Save();

                    if (result <= 0)
                        mensagemErro = "Erro ao salvar o cliente";
                }
                catch (Exception ex)
                {
                    mensagemErro = ex.Message;
                }
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(CreatedAtAction(nameof(Get), new { usuario.id }, usuario));
        }

        [Authorize]
        [HttpPut]
        public ActionResult<Usuario> Put(Usuario usuario)
        {
            var mensagemErro = "";

            if (!ModelState.IsValid)
            {
                mensagemErro = usuario.MensagemErro;
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    _repository.Update(usuario);
                    var result = _repository.Save();
                }
                catch (Exception ex)
                {
                    mensagemErro = ex.Message;
                }
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(CreatedAtAction(nameof(Get), new { usuario.id }, usuario));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<Usuario> Delete(int ID)
        {
            var mensagemErro = "";
            var usuario = new Usuario();

            try
            {
                usuario = _repository.GetById(ID);

                if (usuario == null)
                    return NotFound();

                _repository.Delete(usuario.id);
                var result = _repository.Save();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(usuario);
        }

    }
}
