using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Services.Interfaces
{
    public interface IServiceTransferencia
    {
        Response Depositar(int id, string NumeroConta, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia);
        Response Saque(int id, string NumeroConta, decimal Valor, string TransactionPin);
        Response FazerTransferencia(int id, string ContaOrigem, string ContaDestino, decimal Valor, string TransactionPin, string OrigemDestino, string DestinoTransferencia);
        Response CompraDebito(int id, string NumeroConta, decimal Valor, string TransactionPin);
        Response CompraCredito(int id, string NumeroConta, decimal Valor, string PinTransferencia, string Fatura);

    }
}
