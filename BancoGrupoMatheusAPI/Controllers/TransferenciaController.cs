using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BancoGrupoMatheusAPI.Models;
using BancoGrupoMatheusAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransferenciaController : ControllerBase
    {
        private readonly IServiceTransferencia _transactionService;
        private IMapper _mapper;

        public TransferenciaController(IServiceTransferencia transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Fazer um Deposito")]
        public IActionResult Depositar(int id, string NumeroConta, decimal Valor, string TransactionPin, string OrigemTransferencia, string DestinoTransferencia)
        {
            return Ok(_transactionService.Depositar(id, NumeroConta, Valor, TransactionPin, OrigemTransferencia, DestinoTransferencia));
        }
        [HttpPost]
        [Route("Fazer uma Transferencia")]
        public IActionResult MakeFundsTransferencia(int id, string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia)
        {
            if (ContaOrigem.Equals(ContaDestino)) return BadRequest("Você não pode realizar uma transferência para você mesmo");

            return Ok(_transactionService.FazerTransferencia(id, ContaOrigem, ContaDestino, Valor, TransactionPin, OrigemDestino, DestinoTransferencia));
        }

        [HttpPost]
        [Route("Fazer um Saque")]
        public IActionResult Saque(int id, string NumeroConta, decimal Valor, string TransactionPin)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.Saque(id, NumeroConta, Valor, TransactionPin));

        }

        [HttpPost]
        [Route("Compra no Debito")]
        public IActionResult CompraDebito(int id, string NumeroConta, decimal Valor, string TransactionPin)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.CompraDebito(id, NumeroConta, Valor, TransactionPin));

        }

        [HttpPost]
        [Route("Compra no Credito")]
        public IActionResult CompraCredito(int id, string NumeroConta, decimal Valor, string PinTransferencia, string Fatura)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.CompraCredito(id, NumeroConta, Valor, PinTransferencia, Fatura));

        }


    }
}
