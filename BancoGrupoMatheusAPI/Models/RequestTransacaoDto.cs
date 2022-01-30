using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class RequestTransferenciaDto
    {
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
    }
}
