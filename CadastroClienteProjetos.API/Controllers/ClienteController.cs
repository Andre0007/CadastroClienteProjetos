using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Domain.Interfaces;
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
        IRepository<Cliente> _repository;
        public ClienteController(IRepository<Cliente> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByOne(int? id)
        {
            Cliente cliente = null;
            string mensagemErro = "";

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
            string mensagemErro = "";

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
            string mensagemErro = "";

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
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            string mensagemErro = "";
            bool resultCNPJ = _repository.GetAll().Where(x => x.cnpj == cliente.cnpj).Count() == 0 ? true : false;

            if (!ModelState.IsValid || resultCNPJ == false)
            {
                mensagemErro = (resultCNPJ == false ? "CNPJ já existe" : cliente.MensagemErro);
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    cliente.dataInscricao = DateTime.Now;
                    _repository.Insert(cliente);
                    int result = _repository.Save();

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

            return Ok( CreatedAtAction("Get", new { id = cliente.id }, cliente) );
        }

        [HttpPut]
        public ActionResult<Cliente> Put(Cliente cliente)
        {
            string mensagemErro = ""; bool resultCNPJ = false;

            try
            {
                resultCNPJ = _repository.GetAll().Where(x => x.cnpj == cliente.cnpj && x.id == cliente.id).Count() == 1 ? true : false;

                if (resultCNPJ == false)
                    resultCNPJ = _repository.GetAll().Where(x => x.cnpj == cliente.cnpj).Count() == 0 ? true : false;
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
            }

            if (!ModelState.IsValid || resultCNPJ == false)
            {
                mensagemErro = (resultCNPJ == false ? "CNPJ já existe" : cliente.MensagemErro);
                return BadRequest(mensagemErro);
            }
            else
            {
                try
                {
                    cliente.dataInscricao = DateTime.Now;
                    _repository.Update(cliente);
                    int result = _repository.Save();

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

            return Ok(CreatedAtAction("Get", new { id = cliente.id }, cliente));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? ID)
        {
            string mensagemErro = "";
            Cliente cliente = new Cliente();

            if (ID == null)
                return BadRequest();

            try
            {
                cliente = _repository.GetById(ID);

                if (cliente == null)
                    return NotFound();

                _repository.Delete(cliente.id);
                int result = _repository.Save();
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