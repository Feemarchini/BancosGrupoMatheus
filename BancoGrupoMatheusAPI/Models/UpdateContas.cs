using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class UpdateContas
    {
        public string PrimeiroNome { get; set; }
        public string Sobrenome { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [RegularExpression(@"^[0-9]{4}$/", ErrorMessage = "")]
        public string Pin { get; set; }

    }
}
