using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Tapa_Buraco.DTO
{
    public class Home
    {
        public int ID { get; set; } 
        public string NOME { get; set; }
        public string JOINT { get; set; }
        public string LOA { get; set; }
        public string NUMERO_VIAGEM { get; set; }
        public string AGENCIA { get; set; }
    }
}
