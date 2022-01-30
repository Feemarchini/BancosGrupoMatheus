using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Services.Interfaces
{
    public interface IServiceTransferencia
    {
        Response Depositar(string NumeroConta, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia);
        Response Saque(string NumeroConta, decimal Valor, string TransactionPin);
        Response FazerTransferencia(string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia);
        Response CompraDebito(string NumeroConta, decimal Valor, string TransactionPin);
        Response CompraCredito(string NumeroConta, decimal Valor, string PinTransferencia, string Fatura);

    }
}
