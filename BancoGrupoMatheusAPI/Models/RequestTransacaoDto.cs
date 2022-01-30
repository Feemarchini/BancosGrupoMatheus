using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class RequestTransacaoDto
    {
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
    }
}
