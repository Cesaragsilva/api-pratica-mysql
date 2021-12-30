using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Transacao.API.Configuration;
using Transacao.API.Data.Entities;
using Transacao.API.Entities;
using Transacao.API.Repositories.Dapper;
using Transacao.API.Repositories.Interfaces;

namespace Transacao.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly ILogger<TransactionController> _logger;
        private readonly IDapperDB _dapperDB;
        private static int dado = 0;

        public TransactionController(ITransactionRepository repository, ILogger<TransactionController> logger, IDapperDB dapperDB)
        {
            _repository = repository;
            _logger = logger;
            _dapperDB = dapperDB;
        }

        [HttpGet("GetCidades")]
        [ProducesResponseType(typeof(IEnumerable<Cidade>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidades()
        {
            var cidades = _dapperDB.Search("SELECT Id,Id_GLB_UF,Nome FROM glb_cidade ORDER BY Nome ASC LIMIT 10");
            Console.WriteLine(dado++);
            return Ok(cidades);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Transaction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var data = await _repository.GetTransactions();
            _logger.LogInformation($"[GetTransactions] retornou {data.Count()} transações.");

            return Ok(data);
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Transaction>> GetTransactionById(string id)
        {
            var data = await _repository.GetTransaction(id);

            if (data == null)
            {
                _logger.LogInformation($"[GetTransactionById] {Mensagens.TransacaoNaoEncontrada}: {id}.");
                return NotFound();
            }
            else
                _logger.LogInformation($"[GetTransactionById] encontrou a transação {id}.");

            return Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                await _repository.CreateTransaction(transaction);
                _logger.LogInformation($"[CreateTransaction] {Mensagens.TransacaoCriada} com sucesso!");
            }
            catch(Exception ex)
            {
                _logger.LogError($"[CreateTransaction] {Mensagens.ErroCriarTransacao} - {ex.Message}");
            }
            return CreatedAtRoute("CreateTransaction", new { id = transaction.Id }, transaction);
        }

        [HttpDelete("{id}", Name = "DeleteTransaction")]        
        [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            if (await _repository.DeleteTransaction(id))
            {
                _logger.LogInformation($"[DeleteTransaction] - {Mensagens.TransacaoRemovida}.");
                return Ok();
            }
            
            _logger.LogInformation($"[DeleteTransaction] - {Mensagens.TransacaoNaoEncontrada}");
            return NotFound(Mensagens.TransacaoNaoEncontrada);
        }
    }
}
