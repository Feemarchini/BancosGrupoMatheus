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

            var conta = _dbContext.Contas.SingleOrDefault(x => x.NumeroConta == NumeroConta);
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

            //if validation passes
            string pinHash, pinSalt;
            CriarSenha(Pin, out pinHash, out pinSalt);

            conta.PinStoredHash = pinHash;
            conta.PinStoredSalt = pinSalt;

            _dbContext.Contas.Add(conta);
            _dbContext.SaveChanges();

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta criada com sucesso";
            response.Data = conta.DataDeCriacao;

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

        public Response DeletarConta(int Id)
        {
            Response response = new Response();

            var conta = _dbContext.Contas.Find(Id);
            if (conta != null)
            {
                _dbContext.Contas.Remove(conta);

                _dbContext.SaveChanges();
            }
            DateTime Hoje = DateTime.Today.AddDays(0);

            response.ResponseCode = "00";
            response.ResponseMessage = "Conta deletada com sucesso!";
            response.Data = Hoje;

            return response;
        }

        public IEnumerable<Contas> BuscarTodasAsContas()
        {
            return _dbContext.Contas.ToList();
            DateTime Hoje = DateTime.Today.AddDays(0);

            Response response = new Response();
            response.ResponseCode = "00";
            response.ResponseMessage = "Conta deletada com sucesso!";
            response.Data = Hoje;

        }


        public void AtualizarConta(Contas conta, string Pin = null)
        {
            // fnd userr
            var contaToBeUpdated = _dbContext.Contas.Find(conta.Id);
            if (contaToBeUpdated == null) throw new ApplicationException("Conta não encontrada");
            //so we have a match
            if (!string.IsNullOrWhiteSpace(conta.Email) && conta.Email != contaToBeUpdated.Email)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbContext.Contas.Any(x => x.Email == conta.Email)) throw new ApplicationException("Email " + conta.Email + " já existente");
                contaToBeUpdated.Email = conta.Email;
            }

            if (!string.IsNullOrWhiteSpace(conta.NumeroDeTelefone) && conta.Email != contaToBeUpdated.NumeroDeTelefone)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbContext.Contas.Any(x => x.NumeroDeTelefone == conta.NumeroDeTelefone)) throw new ApplicationException("NumeroDeTelefone " + conta.NumeroDeTelefone + " já existente");
                contaToBeUpdated.NumeroDeTelefone = conta.NumeroDeTelefone;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                string pinHash, pinSalt;
                CriarSenha(Pin, out pinHash, out pinSalt);

                contaToBeUpdated.PinStoredHash = pinHash;
                contaToBeUpdated.PinStoredSalt = pinSalt;

            }

            _dbContext.Contas.Update(contaToBeUpdated);
            _dbContext.SaveChanges();
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
