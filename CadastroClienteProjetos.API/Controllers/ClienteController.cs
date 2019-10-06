using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteProjetos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        readonly IRepository<Cliente> _repository;
        public ClienteController(IRepository<Cliente> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByOne(int? id)
        {
            Cliente cliente = null;
            var mensagemErro = "";

            if (id == null)
                return BadRequest();

            try
            {
                cliente = _repository.GetById(id);
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost("{id}")]
        public ActionResult<IEnumerable<Cliente>> GetDrop()
        {
            IEnumerable<Cliente> cliente = null;
            var mensagemErro = "";

            try
            {
                cliente = _repository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (cliente == null)
                return NotFound();

            return cliente.ToArray();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Cliente> ListaCliente = null;
            var mensagemErro = "";

            try
            {
                ListaCliente = _repository.GetAll().OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            if (ListaCliente == null)
                return NotFound();

            return Ok(ListaCliente);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Cliente cliente)
        {
            var mensagemErro = "";
            var resultCNPJ = _repository.GetAll().Count(x => x.cnpj == cliente.cnpj) == 0 ? true : false;

            if (!ModelState.IsValid || !resultCNPJ)
            {
                mensagemErro = !resultCNPJ ? @"CNPJ já existe" : cliente.MensagemErro;
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    cliente.dataInscricao = DateTime.Now;
                    _repository.Insert(cliente);
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

            return Ok( CreatedAtAction(nameof(Get), new { cliente.id }, cliente) );
        }

        [HttpPut]
        public ActionResult<Cliente> Put(Cliente cliente)
        {
            var mensagemErro = ""; var resultCNPJ = false;

            try
            {
                resultCNPJ = _repository.GetAll().Count(x => x.cnpj == cliente.cnpj && x.id == cliente.id) == 1 ? true : false;

                if (!resultCNPJ)
                    resultCNPJ = _repository.GetAll().Count(x => x.cnpj == cliente.cnpj) == 0 ? true : false;
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (!ModelState.IsValid || !resultCNPJ)
            {
                mensagemErro = (!resultCNPJ ? "CNPJ já existe" : cliente.MensagemErro);
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    cliente.dataInscricao = DateTime.Now;
                    _repository.Update(cliente);
                    var result = _repository.Save();

                    if (result <= 0)
                        mensagemErro = "Erro ao alterar o cliente";
                }
                catch (Exception ex)
                {
                    mensagemErro = ex.Message;
                }
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(CreatedAtAction(nameof(Get), new { cliente.id }, cliente));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? ID)
        {
            var mensagemErro = "";
            var cliente = new Cliente();

            if (ID == null)
                return BadRequest();

            try
            {
                cliente = _repository.GetById(ID);

                if (cliente == null)
                    return NotFound();

                _repository.Delete(cliente.id);
                var result = _repository.Save();
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (mensagemErro != "")
                return BadRequest(mensagemErro);

            return Ok(cliente);
        }

    }
}