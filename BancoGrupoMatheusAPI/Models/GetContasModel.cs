using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class GetContasModel
    {
        public int Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string Sobrenome { get; set; }
        public string NumeroDeTelefone { get; set; }
        public string Email { get; set; }
        public string NumeroContaGenerated { get; set; }        
        public string TipoDeConta { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public string Saldo { get; set; }
    }
}
