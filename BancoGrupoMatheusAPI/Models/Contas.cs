using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    [Table("Contas")]
    public class Contas
    {
        [Key]
        public int Id { get; set; }

        public string PrimeiroNome { get; set; }

        public string Sobrenome { get; set; }

        public string Email { get; set; }

        public string NumeroConta { get; set; }

        public DateTime DataDeCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string Saldo { get; set; }

        public string PinStoredHash { get; set; }

        public string PinStoredSalt { get; set; }
        public string Pin { get; set; }

        public string CNPJ { get; set; }

        public string CPF { get; set; }

        public string TipoDeConta { get; set; }

        public string NumeroDeTelefone { get; set; }
        
        public string Fatura { get; }


        Random rand = new Random();


        public Contas()
        {
            //generrate NumeroConta for customer
            NumeroConta = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
        }

    }
}
