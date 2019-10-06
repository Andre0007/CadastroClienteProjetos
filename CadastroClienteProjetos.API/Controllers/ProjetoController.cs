using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        readonly IRepository<Projeto> _repository;
        public ProjetoController(IRepository<Projeto> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Projeto> GetByOne(int id)
        {
            Projeto projeto = null;
            var mensagemErro = "";

            try
            {
                projeto = _repository.GetById(id);
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (projeto == null)
                return NotFound();

            return projeto;
        }

        [HttpPost("{id}")]
        public ActionResult<IEnumerable<Projeto>> GetDrop()
        {
            IEnumerable<Projeto> projeto = null;
            var mensagemErro = "";

            try
            {
                projeto = _repository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (projeto == null)
                return NotFound();

            return projeto.ToArray();
        }

        [HttpGet]
        public ActionResult<List<Projeto>> Get()
        {
            return _repository.GetAll().ToList();
        }

        [HttpPost]
        public ActionResult<Projeto> Post(Projeto projeto)
        {
            var mensagemErro = "";

            if (!ModelState.IsValid)
            {
                mensagemErro = projeto.MensagemErro;
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    _repository.Insert(projeto);
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

            return Ok(CreatedAtAction(nameof(Get), new { projeto.id }, projeto));
        }

        [HttpPut]
        public ActionResult<Projeto> Put(Projeto projeto)
        {
            var mensagemErro = "";

            if (!ModelState.IsValid)
            {
                mensagemErro = projeto.MensagemErro;
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    _repository.Update(projeto);
                    var result = _repository.Save();
                }
                catch (Exception ex)
                {
                    mensagemErro = ex.Message;
                }
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(CreatedAtAction(nameof(Get), new { projeto.id }, projeto));
        }

        [HttpDelete("{id}")]
        public ActionResult<Projeto> Delete(int ID)
        {
            var mensagemErro = "";
            var projeto = new Projeto();

            try
            {
                projeto = _repository.GetById(ID);

                if (projeto == null)
                    return NotFound();

                _repository.Update(projeto);
                var result = _repository.Save();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(projeto);
        }

    }
}