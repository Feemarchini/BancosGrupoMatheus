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
        public IActionResult Depositar(string NumeroConta, decimal Valor, string TransactionPin, string OrigemTransferencia, string DestinoTransferencia)
        {
            return Ok(_transactionService.Depositar(NumeroConta, Valor, TransactionPin, OrigemTransferencia, DestinoTransferencia));
        }
        [HttpPost]
        [Route("Fazer uma Transferencia")]
        public IActionResult MakeFundsTransferencia(string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia)
        {
            if (ContaOrigem.Equals(ContaDestino)) return BadRequest("Você não pode realizar uma transferência para você mesmo");

            return Ok(_transactionService.FazerTransferencia(ContaOrigem, ContaDestino, Valor, TransactionPin, OrigemDestino, DestinoTransferencia));
        }

        [HttpPost]
        [Route("Fazer um Saque")]
        public IActionResult Saque(string NumeroConta, decimal Valor, string TransactionPin)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.Saque(NumeroConta, Valor, TransactionPin));

        }

        [HttpPost]
        [Route("Compra no Debito")]
        public IActionResult CompraDebito(string NumeroConta, decimal Valor, string TransactionPin)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.CompraDebito(NumeroConta, Valor, TransactionPin));

        }

        [HttpPost]
        [Route("Compra no Credito")]
        public IActionResult CompraCredito(string NumeroConta, decimal Valor, string PinTransferencia, string Fatura)
        {
            //try check validity of NumeroConta
            if (!Regex.IsMatch(NumeroConta, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Seu número de conta pode conter apenas 10 digitos");

            return Ok(_transactionService.CompraCredito(NumeroConta, Valor, PinTransferencia, Fatura));

        }


    }
}
