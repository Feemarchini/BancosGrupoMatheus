using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class RegistrarNovaConta
    {
        
        public int Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string Sobrenome { get; set; }
        public string NumeroDeTelefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public string TipoDeConta { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        //cummulative
        [Required]
        //[RegularExpression(@"^[0-9]{4}$")]
        public string Pin { get; set; }
        [Required]
        //[Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }

    }
}
