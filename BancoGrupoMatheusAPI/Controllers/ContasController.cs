using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BancoGrupoMatheusAPI.Models;
using BancoGrupoMatheusAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class contasController : ControllerBase
    {
        private IMapper _mapper;
        private IUserService _userService;

        public contasController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        [HttpPost]
        [Route("Registar Nova Conta")]
        public IActionResult CriarConta([FromBody] RegistrarNovaConta novaConta)
        {
            if (!ModelState.IsValid) return BadRequest(novaConta);
            //map
            var conta = _mapper.Map<Contas>(novaConta);
            return Ok(_userService.CriarConta(conta, novaConta.Pin, novaConta.ConfirmPin));
        }

        [HttpPut]
        [Route("Atualizar conta")]
        public IActionResult AtualizarConta(string primeiroNome, string Sobrenome, string Email, string numeroConta, string Pin, string cnpj, string cpf, string tipoDeConta, string numeroDeTelefone)
        {
            return Ok(_userService.AtualizarConta(primeiroNome, Sobrenome, Email, numeroConta, Pin, cnpj, cpf, tipoDeConta, numeroDeTelefone));
        }

        [HttpDelete]
        [Route("Deletar Conta")]
        public IActionResult DeletarConta(string NumeroConta, string Pin)
        {
            if (!ModelState.IsValid) return BadRequest(NumeroConta);
            //map
            return Ok(_userService.DeletarConta(NumeroConta, Pin));
        }


        [HttpGet]

        [Route("Buscar Todas Contas")]
        public IActionResult BuscarTodasAsContas()
        {
            var todasContas = _userService.BuscarTodasAsContas();
            var getTodasContas = _mapper.Map<IList<GetContasModel>>(todasContas);
            return Ok(getTodasContas);
        }

        [HttpPost]
        [Route("Autenticacao")]
        public IActionResult Autenticacao([FromBody] AutenticacaoModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var authResult = _userService.Autenticacao(model.NumeroConta, model.Pin);
            if (authResult == null) return Unauthorized("Credenciais Invalidas");
            return Ok(authResult);
        }
    }
}
