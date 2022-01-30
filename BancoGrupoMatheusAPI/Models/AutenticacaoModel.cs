using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    public class AutenticacaoModel
    {
        [Required]
        public string NumeroConta { get; set; }
        [Required]      
        public string Pin { get; set; }
    }
}
