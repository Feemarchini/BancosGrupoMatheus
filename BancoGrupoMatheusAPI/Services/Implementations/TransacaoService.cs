﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BancoGrupoMatheusAPI.DAL;
using BancoGrupoMatheusAPI.Models;
using BancoGrupoMatheusAPI.Services.Interfaces;
using BancoGrupoMatheusAPI.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Services.Implementations
{
    public class TransacaoService : IServiceTransferencia
    {
        private DBContextBancoGrupoMatheus _dbContext;

        private ILogger<TransacaoService> _logger;
        private IUserService _userService;
        private AppSettings _settings;
        private static string contaPrincipalSelecionada;
        private static string saldoContaDestino;
        private static string totalFatura;

        public TransacaoService(DBContextBancoGrupoMatheus dbContext, ILogger<TransacaoService> logger, IUserService userService, IOptions<AppSettings> settings)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userService = userService;
            _settings = settings.Value;

            contaPrincipalSelecionada = _settings.contaPrincipalSelecionada;

        }

        public Response Depositar(int id, string NumeroConta, decimal Valor, string PinTransaction, string OrigemTransferencia, string DestinoTransferencia)
        {
            Response response = new Response();
            Contas conta; //our Bank Settlement conta
            Contas contaDestino; //individual
            Transacoes transacoes = new Transacoes();

            var AutenticacaoUser = _userService.Autenticacao(NumeroConta, PinTransaction);
            if (AutenticacaoUser == null)
            {
                throw new ApplicationException("Credenciais Inválidas");
            }

            try
            {
                conta = _userService.BuscarNumeroConta(contaPrincipalSelecionada);
                contaDestino = _userService.BuscarNumeroConta(NumeroConta);

                var saldo = conta.Saldo;
                var saldoDecimal = Convert.ToDecimal(saldo);

                var saldoDestino = Convert.ToDecimal(contaDestino.Saldo);
                var saldoDestinoDecimal = Convert.ToDecimal(saldoDestino);

                saldoDecimal -= Valor;
                saldoDestinoDecimal += Valor;

                conta.Saldo = saldoDecimal.ToString();
                contaDestino.Saldo = saldoDestinoDecimal.ToString();

                if ((_dbContext.Entry(conta).State == Microsoft.EntityFrameworkCore.EntityState.Modified) && (_dbContext.Entry(contaDestino).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //sso there was an update
                    transacoes.StatusTransferencia = TranStatus.TransferenciaEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferencia realizada com sucesso!";
                    response.Data = transacoes.DataTransferencia;

                }
                else
                {
                    transacoes.StatusTransferencia = TranStatus.FalhaNaTransferencia;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferencia falhou!";
                    response.Data = transacoes.DataTransferencia;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            transacoes.Id = id;
            transacoes.DataTransferencia = DateTime.Now;
            transacoes.TipoDeTransferencia = TranType.Deposito;
            transacoes.ValorTransferencia = Valor.ToString();
            transacoes.IsSuccessful = "sucesso";
            transacoes.SaldoTransferencia = contaPrincipalSelecionada;
            transacoes.DestinoTransferencia = NumeroConta;
            transacoes.OrigemTransferencia = OrigemTransferencia;
            transacoes.DestinoTransferencia = DestinoTransferencia;
            transacoes.ObservacaoTransferencia = $"Transacão efetuada da sua conta {JsonConvert.SerializeObject(transacoes.SaldoTransferencia)} para a conta=> " +
                $"{JsonConvert.SerializeObject(transacoes.DestinoTransferencia)} ON " +
                $"{transacoes.DataTransferencia} TipoDeTransferencia =>  " +
                $"{transacoes.TipoDeTransferencia} StatusTransferencia => " +
                $"{transacoes.StatusTransferencia}";

            _dbContext.Transacoes.Add(transacoes);
            _dbContext.SaveChanges();


            return response;

        }

        public Response FazerTransferencia(int id, string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia)
        {
            //3 contas or 2 are involved

            //Fromconta iss our current user/customer's conta and we'll Autenticacao with it...
            Response response = new Response();
            Contas saldoConta; //our current Autenticacaod customer
            Contas contaDestino; //target conta where money is being sent to...
            Transacoes Transferencia = new Transacoes();

            //let's Autenticacao first
            var AutenticacaoUser = _userService.Autenticacao(ContaOrigem, TransactionPin);
            if (AutenticacaoUser == null)
            {

                throw new ApplicationException("Credenciais inválidas");
            }
            //user Autenticacaod, then llet's process funds Transferencia;
            try
            {
                saldoConta = _userService.BuscarNumeroConta(ContaOrigem);
                contaDestino = _userService.BuscarNumeroConta(ContaDestino);

                var saldo = saldoConta.Saldo;
                var saldoDecimal = Convert.ToDecimal(saldo);

                var saldoDestino = Convert.ToDecimal(contaDestino.Saldo);
                var saldoDestinoDecimal = Convert.ToDecimal(saldoDestino);

                saldoDecimal -= Valor;
                saldoDestinoDecimal += Valor;

                saldoConta.Saldo = saldoDecimal.ToString();
                contaDestino.Saldo = saldoDestinoDecimal.ToString();

                contaPrincipalSelecionada = saldoConta.Saldo;
                saldoContaDestino = contaDestino.Saldo;


                if ((_dbContext.Entry(saldoConta).State == Microsoft.EntityFrameworkCore.EntityState.Modified) && (_dbContext.Entry(contaDestino).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so there was an update in the context State
                    Transferencia.StatusTransferencia = TranStatus.TransferenciaEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferência realizada com sucesso!";
                    response.Data = Transferencia.DataTransferencia;

                }
                else
                {
                    Transferencia.StatusTransferencia = TranStatus.FalhaNaTransferencia;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferência não realizada!";
                    response.Data = Transferencia.DataTransferencia;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transferencia.DataTransferencia = DateTime.Now;
            Transferencia.TipoDeTransferencia = TranType.Transferencia;
            Transferencia.ValorTransferencia = Valor.ToString();
            Transferencia.SaldoTransferencia = contaPrincipalSelecionada;
            Transferencia.DestinoTransferencia = saldoContaDestino;
            Transferencia.ObservacaoTransferencia = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(Transferencia.SaldoTransferencia)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(Transferencia.DestinoTransferencia)} ON " +
                $"{Transferencia.DataTransferencia} TipoTransferencia =>  " +
                $"{Transferencia.TipoDeTransferencia} StatusTransferencia => " +
                $"{Transferencia.StatusTransferencia}";

            _dbContext.Transacoes.Add(Transferencia);
            _dbContext.SaveChanges();


            return response;

        }

        public Response Saque(int id, string NumeroConta, decimal Valor, string PinTransferencia)
        {
            Response response = new Response();
            Contas saldoConta; //individual
            Transacoes Transferencia = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransferencia);
            if (autenticacaoUsuario == null)
            {
                throw new ApplicationException("Credenciais Inválidas");
            }

            try
            {
                saldoConta = _userService.BuscarNumeroConta(NumeroConta);

                var saldo = saldoConta.Saldo;
                var saldoDecimal = Convert.ToDecimal(saldo);

                var saldoFinal = "";

                saldoFinal = Convert.ToString(saldoDecimal - Valor);

                saldoConta.Saldo = saldoFinal;

                contaPrincipalSelecionada = saldoConta.Saldo;


                if ((_dbContext.Entry(saldoConta).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so there was an update in the context State
                    Transferencia.StatusTransferencia = TranStatus.TransferenciaEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Saque efetuado com sucesso!";
                    response.Data = Transferencia.DataTransferencia;

                }
                else
                {
                    Transferencia.StatusTransferencia = TranStatus.FalhaNaTransferencia;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Saque não efetuado!";
                    response.Data = Transferencia.DataTransferencia;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transferencia.Id = id;
            Transferencia.DataTransferencia = DateTime.Now;
            Transferencia.TipoDeTransferencia = TranType.Saque;
            Transferencia.ValorTransferencia = Valor.ToString();
            Transferencia.IsSuccessful = "sucesso";
            Transferencia.SaldoTransferencia = contaPrincipalSelecionada;
            Transferencia.ObservacaoTransferencia =
                $"{Transferencia.DataTransferencia} TipoTransferencia =>  " +
                $"{Transferencia.TipoDeTransferencia} StautsTransferencia => " +
                $"{Transferencia.StatusTransferencia}";

            _dbContext.Transacoes.Add(Transferencia);
            _dbContext.SaveChanges();


            return response;
        }

        public Response CompraDebito(int id, string NumeroConta, decimal Valor, string PinTransferencia)
        {
            Response response = new Response();
            Contas saldoConta; //individual
            Transacoes Transferencia = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransferencia);
            if (autenticacaoUsuario == null)
            {
                throw new ApplicationException("Credenciais Inválidas");
            }

            try
            {
                saldoConta = _userService.BuscarNumeroConta(NumeroConta);

                var saldo = saldoConta.Saldo;
                var saldoDecimal = Convert.ToDecimal(saldo);

                var saldoFinal = "";

                saldoFinal = Convert.ToString(saldoDecimal - Valor);

                saldoConta.Saldo = saldoFinal;

                contaPrincipalSelecionada = saldoConta.Saldo;


                if ((_dbContext.Entry(saldoConta).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so there was an update in the context State
                    Transferencia.StatusTransferencia = TranStatus.TransferenciaEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra efetuada com sucesso!";
                    response.Data = Transferencia.DataTransferencia;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();

                }
                else
                {
                    Transferencia.StatusTransferencia = TranStatus.FalhaNaTransferencia;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra não efetuada!";
                    response.Data = Transferencia.DataTransferencia;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transferencia.Id = id;
            Transferencia.DataTransferencia = DateTime.Now;
            Transferencia.TipoDeTransferencia = TranType.Debito;
            Transferencia.ValorTransferencia = Valor.ToString();
            Transferencia.IsSuccessful = "sucesso";
            Transferencia.SaldoTransferencia = contaPrincipalSelecionada;
            Transferencia.ObservacaoTransferencia =
                $"{Transferencia.DataTransferencia} TipoTransferencia =>  " +
                $"{Transferencia.TipoDeTransferencia} StautsTransferencia => " +
                $"{Transferencia.StatusTransferencia}";

            _dbContext.Transacoes.Add(Transferencia);
            _dbContext.SaveChanges();

            return response;
        }

        public Response CompraCredito(int id, string NumeroConta, decimal Valor, string PinTransferencia, string Fatura)
        {
            Response response = new Response();
            Contas faturaConta; //individual
            Transacoes Transferencia = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransferencia);
            if (autenticacaoUsuario == null)
            {
                throw new ApplicationException("Credenciais Inválidas");
            }

            try
            {

                faturaConta = _userService.BuscarNumeroConta(NumeroConta);

                Fatura = faturaConta.Fatura;

                string faturaFinal = Fatura + Valor;

                totalFatura = faturaFinal;

                if ((_dbContext.Entry(faturaConta).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so there was an update in the context State
                    Transferencia.StatusTransferencia = TranStatus.TransferenciaEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra no crédito efetuada com sucesso!";
                    response.Data = Transferencia.DataTransferencia;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();

                }
                else
                {
                    Transferencia.StatusTransferencia = TranStatus.FalhaNaTransferencia;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra no crédito não efetuada!";
                    response.Data = Transferencia.DataTransferencia;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            var faturaInicial = _dbContext.Contas.Where(x => x.NumeroConta == NumeroConta).SingleOrDefault();

            Transferencia.Id = id;
            Transferencia.DataTransferencia = DateTime.Now;
            Transferencia.TipoDeTransferencia = TranType.Debito;
            Transferencia.ValorTransferencia = Valor.ToString();
            Transferencia.IsSuccessful = "sucesso";
            Transferencia.Fatura = faturaInicial.Fatura;
            Transferencia.TotalFatura = totalFatura;
            Transferencia.ObservacaoTransferencia =
                $"{Transferencia.DataTransferencia} TipoTransferencia =>  " +
                $"{Transferencia.TipoDeTransferencia} StautsTransferencia => " +
                $"{Transferencia.StatusTransferencia}";

            _dbContext.Transacoes.Add(Transferencia);
            _dbContext.SaveChanges();

            return response;
        }
    }
}