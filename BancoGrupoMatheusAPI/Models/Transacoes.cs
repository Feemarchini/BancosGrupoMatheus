using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    [Table("Transacoes")]
    public class Transacoes
    {
        [Key]
        public int Id { get; set; }

        public int TransferenciaUniqueId { get; set; }

        public DateTime MesTransferencia { get; set; }

        public TranStatus StatusTransferencia { get; set; }

        public string IsSuccessful { get; set; }

        public string OrigemTransferencia { get; set; }

        public string DestinoTransferencia { get; set; }

        public string ObservacaoTransferencia { get; set; }

        public TranType TipoDeTransferencia { get; set; }

        public DateTime DataTransferencia { get; set; }

        public string ValorTransferencia { get; set; }

        public string SaldoTransferencia { get; set; }
        public string Fatura { get; set; }
        public string TotalFatura { get; set; }


        public Transacoes()
        {
            ObservacaoTransferencia = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 17)}";
        }


    }

    public enum TranStatus
    {
        FalhaNaTransferencia,
        TransferenciaEfetuadaComSucesso,
        Erro
    }

    public enum TranType
    {
        Deposito,
        Saque,
        Transferencia,
        Debito,
        Credito
    }
}
