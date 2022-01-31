using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Services.Interfaces
{
    public interface IServiceTransacao
    {
        Response Depositar(string NumeroConta, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransacao);
        Response Saque(string NumeroConta, decimal Valor, string TransactionPin);
        Response FazerTransacao(string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransacao);
        Response CompraDebito(string NumeroConta, decimal Valor, string TransactionPin);
        Response CompraCredito(string NumeroConta, decimal Valor, string PinTransacao);

    }
}
