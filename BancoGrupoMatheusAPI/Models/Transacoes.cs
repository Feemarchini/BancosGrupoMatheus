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

        public int TransacaoUniqueId { get; set; }

        public DateTime MesTransacao { get; set; }

        public TranStatus StatusTransacao { get; set; }

        public string IsSuccessful { get; set; }

        public string OrigemTransacao { get; set; }

        public string DestinoTransacao { get; set; }

        public string ObservacaoTransacao { get; set; }

        public TranType TipoDeTransacao { get; set; }

        public DateTime DataTransacao { get; set; }

        public string ValorTransacao { get; set; }

        public string SaldoTransacao { get; set; }
        public string Fatura { get; set; }
        public string TotalFatura { get; set; }


        public Transacoes()
        {
            ObservacaoTransacao = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 17)}";
        }


    }

    public enum TranStatus
    {
        FalhaNaTransacao,
        TransacaoEfetuadaComSucesso,
        Erro
    }

    public enum TranType
    {
        Deposito,
        Saque,
        Transacao,
        Debito,
        Credito
    }
}
