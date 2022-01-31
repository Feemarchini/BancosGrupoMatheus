using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Services.Interfaces
{
    public interface IUserService
    {
        Contas Autenticacao(string NumeroConta, string Pin);
        IEnumerable<Contas> BuscarTodasAsContas();
        Contas CriarConta(Contas conta, string Pin, string ConfirmPin);
        Response AtualizarConta(string primeiroNome, string Sobrenome, string Email, string numeroConta, string Pin, string cnpj, string cpf, string tipoDeConta, string numeroDeTelefone);
        Contas DeletarConta(string NumeroConta, string Pin);
        Contas BuscarNumeroConta(string NumeroConta);
    }
}
