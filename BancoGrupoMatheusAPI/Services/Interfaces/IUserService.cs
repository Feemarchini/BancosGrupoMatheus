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
        Contas AtualizarConta(AtualizarContas conta, string Pin = null);
        Response DeletarConta(int Id);
        Contas BuscarNumeroConta(string NumeroConta);
    }
}
