using Microsoft.Extensions.Logging;
using BancoGrupoMatheusAPI.DAL;
using BancoGrupoMatheusAPI.Models;
using BancoGrupoMatheusAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace BancoGrupoMatheusAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DBContextBancoGrupoMatheus _dbContext;
        private ILogger<UserService> _logger;

        public UserService(
            DBContextBancoGrupoMatheus dbContext,
            ILogger<UserService> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Contas Autenticacao(string NumeroConta, string Pin)
        {
            if (string.IsNullOrEmpty(NumeroConta) || string.IsNullOrEmpty(Pin))
                return null;

            var conta = _dbContext.Contas.Where(x => x.NumeroConta == NumeroConta).FirstOrDefault();
            //is conta null
            if (conta == null)
                return null;

            //so user exists,

            if (conta.PinStoredSalt != Pin)
            {
                throw new ApplicationException("Senha Incorreta");

            }
            DateTime Hoje = DateTime.Today.AddDays(0);

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Autenticado";
            response.Data = Hoje;

            _dbContext.Response.Add(response);
            _dbContext.SaveChanges();

            //auth successful
            return conta;
        }

        public Contas CriarConta(Contas conta, string Pin, string ConfirmPin)
        {
            //validate
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin não pode ser null");
            //does a user with this email exist already?
            if (_dbContext.Contas.Any(x => x.Email == conta.Email)) throw new ApplicationException("Já existe um usuário com esse email");
            //is pin eequal to confirmmpin
            if (!Pin.Equals(ConfirmPin)) throw new ApplicationException("Pins não confere.");

            if(conta.TipoDeConta.Contains("Corrente") || conta.TipoDeConta.Contains("Conta Corrente") || conta.TipoDeConta.Contains("string") || conta.TipoDeConta.Contains("String"))
            {
                if(conta.CPF == null)
                throw new ApplicationException("O campo CPF deve ser obrigatório");
            }

            if (conta.TipoDeConta.Contains("Juridica") || conta.TipoDeConta.Contains("Pessoa Juridica") || conta.TipoDeConta.Contains("Jurídica") || conta.TipoDeConta.Contains("Pessoa Jurídica") ||
                conta.TipoDeConta.Contains("Juridíca") || conta.TipoDeConta.Contains("Pessoa Juridíca") || conta.TipoDeConta.Contains("string") || conta.TipoDeConta.Contains("String"))
            {
                if (conta.CNPJ == null)
                    throw new ApplicationException("O campo CNPJ deve ser obrigatório");
            }

            //if validation passes
            string pinHash, pinSalt;
            CriarSenha(Pin, out pinHash, out pinSalt);
            string numeroConta = conta.NumeroConta;

            conta.PinStoredHash = pinHash;
            conta.PinStoredSalt = pinSalt;

            conta.Saldo = "100000000000";

            Random rnd = new Random();
            conta.NumeroConta = Convert.ToString((long)Math.Floor(rnd.NextDouble() * 9_000_000_000L + 1_000_000_000L));

            _dbContext.Contas.Add(conta);
            _dbContext.SaveChanges();

            DateTime Hoje = DateTime.Today.AddDays(0);

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta criada com sucesso";
            response.Data = Hoje;

            _dbContext.Response.Add(response);
            _dbContext.SaveChanges();

            return conta;

        }

        private static void CriarSenha(string Pin, out string pinHash, out string pinSalt)
        {
            //checks pin
            if (string.IsNullOrEmpty(Pin)) throw new ArgumentNullException("Pin");
            {
                pinHash = Pin;
                pinSalt = Pin;
            }
        }

        public Contas DeletarConta(string NumeroConta, string Pin)
        {
            Response response = new Response();

            var conta = _dbContext.Contas.Where( x => x.NumeroConta == NumeroConta).SingleOrDefault();
            if (conta != null)
            {
                _dbContext.Contas.Remove(conta);

                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Conta não encontrada!");
            }
            DateTime Hoje = DateTime.Today.AddDays(0);

            response.ResponseCode = "00";
            response.ResponseMessage = "Conta deletada com sucesso!";
            response.Data = Hoje;

            _dbContext.Response.Add(response);
            _dbContext.SaveChanges();

            return conta;
        }

        public IEnumerable<Contas> BuscarTodasAsContas()
        {
            return _dbContext.Contas.ToList();
            DateTime Hoje = DateTime.Today.AddDays(0);

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta deletada com sucesso!";
            response.Data = Hoje;

            _dbContext.Response.Add(response);
            _dbContext.SaveChanges();

        }


        public Response AtualizarConta(string primeiroNome, string Sobrenome,string Email, string numeroConta, string Pin, string cnpj, string cpf, string tipoDeConta, string numeroDeTelefone)
        {
            // fnd userr
            var contaAtualizada = _dbContext.Contas.Where(x => x.NumeroConta == numeroConta).SingleOrDefault();
            if (contaAtualizada == null) throw new ApplicationException("Conta não encontrada");
            //so we have a match
            if (!string.IsNullOrWhiteSpace(Email) && Email != contaAtualizada.Email)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbContext.Contas.Any(x => x.Email == Email)) throw new ApplicationException("Email " + Email + " já existente");
                contaAtualizada.Email = Email;
            }

            if (!string.IsNullOrWhiteSpace(numeroDeTelefone) && Email != contaAtualizada.NumeroDeTelefone)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbContext.Contas.Any(x => x.NumeroDeTelefone == numeroDeTelefone)) throw new ApplicationException("NumeroDeTelefone " + numeroDeTelefone + " já existente");
                contaAtualizada.NumeroDeTelefone = numeroDeTelefone;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                string pinHash, pinSalt;
                CriarSenha(Pin, out pinHash, out pinSalt);

                contaAtualizada.PinStoredHash = pinHash;
                contaAtualizada.PinStoredSalt = pinSalt;
                contaAtualizada.Pin = Pin;
            }

            _dbContext.Contas.Update(contaAtualizada);
            _dbContext.SaveChanges();

            var dataAtualizacao = DateTime.Today.AddDays(0);

            Contas conta = new Contas();
            conta.PrimeiroNome = primeiroNome;
            conta.NumeroConta = numeroConta;
            conta.Email = Email;
            conta.DataAtualizacao = dataAtualizacao;
            conta.PinStoredHash = Pin;
            conta.PinStoredSalt = Pin;
            conta.Pin = Pin;
            conta.CNPJ = cnpj;
            conta.CPF = cpf;
            conta.TipoDeConta = tipoDeConta;
            conta.NumeroDeTelefone = numeroDeTelefone;


            _dbContext.Contas.Add(conta);
            _dbContext.SaveChanges();

            DateTime Hoje = DateTime.Today.AddDays(0);
            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta atualizada com sucesso!";
            response.Data = Hoje;

            _dbContext.Response.Add(response);
            _dbContext.SaveChanges();

            return response;
        }

        public Contas BuscarNumeroConta(string NumeroConta)
        {
            var conta = _dbContext.Contas.Where(x => x.NumeroConta == NumeroConta).SingleOrDefault();
            if (conta == null)
            {
                return null;
            }
            DateTime Hoje = DateTime.Today.AddDays(0);

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta deletada com sucesso!";
            response.Data = Hoje;


            return conta;
        }
    }
}
