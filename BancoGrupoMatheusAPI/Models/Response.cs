using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Models
{
    [Table("Response")]
    public class Response
    {
        public int Id { get; set; }
        public string ResponseCode { get; set; }
        public string RequestId => $"{Guid.NewGuid().ToString()}";
        public string ResponseMessage { get; set; }
        public DateTime Data { get; set; } 

    }
}
