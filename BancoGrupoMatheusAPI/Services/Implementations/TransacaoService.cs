using Microsoft.Extensions.Logging;
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
    public class TransacaoService : IServiceTransacao
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

        public Response Depositar(string NumeroConta, decimal Valor, string PinTransaction, string OrigemTransacao, string DestinoTransacao)
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
                    transacoes.StatusTransacao = TranStatus.TransacaoEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transacao realizada com sucesso!";
                    response.Data = transacoes.DataTransacao;

                }
                else
                {
                    transacoes.StatusTransacao = TranStatus.FalhaNaTransacao;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transacao falhou!";
                    response.Data = transacoes.DataTransacao;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            transacoes.DataTransacao = DateTime.Now;
            transacoes.TipoDeTransacao = TranType.Deposito;
            transacoes.ValorTransacao = Valor.ToString();
            transacoes.IsSuccessful = "sucesso";
            transacoes.SaldoTransacao = contaPrincipalSelecionada;
            transacoes.DestinoTransacao = NumeroConta;
            transacoes.OrigemTransacao = OrigemTransacao;
            transacoes.DestinoTransacao = DestinoTransacao;
            transacoes.ObservacaoTransacao = $"Transacão efetuada da sua conta {JsonConvert.SerializeObject(transacoes.SaldoTransacao)} para a conta=> " +
                $"{JsonConvert.SerializeObject(transacoes.DestinoTransacao)} ON " +
                $"{transacoes.DataTransacao} TipoDeTransacao =>  " +
                $"{transacoes.TipoDeTransacao} StatusTransacao => " +
                $"{transacoes.StatusTransacao}";

            _dbContext.Transacoes.Add(transacoes);
            _dbContext.SaveChanges();


            return response;

        }

        public Response FazerTransacao(string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransacao)
        {
            //3 contas or 2 are involved

            //Fromconta iss our current user/customer's conta and we'll Autenticacao with it...
            Response response = new Response();
            Contas saldoConta; //our current Autenticacaod customer
            Contas contaDestino; //target conta where money is being sent to...
            Transacoes Transacao = new Transacoes();

            //let's Autenticacao first
            var AutenticacaoUser = _userService.Autenticacao(ContaOrigem, TransactionPin);
            if (AutenticacaoUser == null)
            {

                throw new ApplicationException("Credenciais inválidas");
            }
            //user Autenticacaod, then llet's process funds Transacao;
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
                    Transacao.StatusTransacao = TranStatus.TransacaoEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferência realizada com sucesso!";
                    response.Data = Transacao.DataTransacao;

                }
                else
                {
                    Transacao.StatusTransacao = TranStatus.FalhaNaTransacao;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transferência não realizada!";
                    response.Data = Transacao.DataTransacao;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transacao.DataTransacao = DateTime.Now;
            Transacao.TipoDeTransacao = TranType.Transacao;
            Transacao.ValorTransacao = Valor.ToString();
            Transacao.SaldoTransacao = contaPrincipalSelecionada;
            Transacao.DestinoTransacao = saldoContaDestino;
            Transacao.ObservacaoTransacao = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(Transacao.SaldoTransacao)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(Transacao.DestinoTransacao)} ON " +
                $"{Transacao.DataTransacao} TipoTransacao =>  " +
                $"{Transacao.TipoDeTransacao} StatusTransacao => " +
                $"{Transacao.StatusTransacao}";

            _dbContext.Transacoes.Add(Transacao);
            _dbContext.SaveChanges();


            return response;

        }

        public Response Saque(string NumeroConta, decimal Valor, string PinTransacao)
        {
            Response response = new Response();
            Contas saldoConta; //individual
            Transacoes Transacao = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransacao);
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
                    Transacao.StatusTransacao = TranStatus.TransacaoEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Saque efetuado com sucesso!";
                    response.Data = Transacao.DataTransacao;

                }
                else
                {
                    Transacao.StatusTransacao = TranStatus.FalhaNaTransacao;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Saque não efetuado!";
                    response.Data = Transacao.DataTransacao;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transacao.DataTransacao = DateTime.Now;
            Transacao.TipoDeTransacao = TranType.Saque;
            Transacao.ValorTransacao = Valor.ToString();
            Transacao.IsSuccessful = "sucesso";
            Transacao.SaldoTransacao = contaPrincipalSelecionada;
            Transacao.ObservacaoTransacao =
                $"{Transacao.DataTransacao} TipoTransacao =>  " +
                $"{Transacao.TipoDeTransacao} StautsTransacao => " +
                $"{Transacao.StatusTransacao}";

            _dbContext.Transacoes.Add(Transacao);
            _dbContext.SaveChanges();


            return response;
        }

        public Response CompraDebito(string NumeroConta, decimal Valor, string PinTransacao)
        {
            Response response = new Response();
            Contas saldoConta; //individual
            Transacoes Transacao = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransacao);
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
                    Transacao.StatusTransacao = TranStatus.TransacaoEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra efetuada com sucesso!";
                    response.Data = Transacao.DataTransacao;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();

                }
                else
                {
                    Transacao.StatusTransacao = TranStatus.FalhaNaTransacao;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra não efetuada!";
                    response.Data = Transacao.DataTransacao;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            Transacao.DataTransacao = DateTime.Now;
            Transacao.TipoDeTransacao = TranType.Debito;
            Transacao.ValorTransacao = Valor.ToString();
            Transacao.IsSuccessful = "sucesso";
            Transacao.SaldoTransacao = contaPrincipalSelecionada;
            Transacao.ObservacaoTransacao =
                $"{Transacao.DataTransacao} TipoTransacao =>  " +
                $"{Transacao.TipoDeTransacao} StautsTransacao => " +
                $"{Transacao.StatusTransacao}";

            _dbContext.Transacoes.Add(Transacao);
            _dbContext.SaveChanges();

            return response;
        }

        public Response CompraCredito(string NumeroConta, decimal Valor, string PinTransacao, string Fatura)
        {
            Response response = new Response();
            Contas faturaConta; //individual
            Transacoes Transacao = new Transacoes();

            var autenticacaoUsuario = _userService.Autenticacao(NumeroConta, PinTransacao);
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
                    Transacao.StatusTransacao = TranStatus.TransacaoEfetuadaComSucesso;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra no crédito efetuada com sucesso!";
                    response.Data = Transacao.DataTransacao;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();

                }
                else
                {
                    Transacao.StatusTransacao = TranStatus.FalhaNaTransacao;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Compra no crédito não efetuada!";
                    response.Data = Transacao.DataTransacao;

                    _dbContext.Response.Add(response);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURRED => MESSAGE: {ex.Message}");
            }

            var faturaInicial = _dbContext.Contas.Where(x => x.NumeroConta == NumeroConta).SingleOrDefault();

            Transacao.DataTransacao = DateTime.Now;
            Transacao.TipoDeTransacao = TranType.Debito;
            Transacao.ValorTransacao = Valor.ToString();
            Transacao.IsSuccessful = "sucesso";
            Transacao.Fatura = faturaInicial.Fatura;
            Transacao.TotalFatura = totalFatura;
            Transacao.ObservacaoTransacao =
                $"{Transacao.DataTransacao} TipoTransacao =>  " +
                $"{Transacao.TipoDeTransacao} StautsTransacao => " +
                $"{Transacao.StatusTransacao}";

            _dbContext.Transacoes.Add(Transacao);
            _dbContext.SaveChanges();

            return response;
        }
    }
}
