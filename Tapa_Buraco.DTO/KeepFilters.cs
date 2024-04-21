using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class KeepFilters
    {
        public KeepFilters()
        {
            Parametros = new Dictionary<string, string>();
        }

        [Key]
        public string Pagina { get; set; }
        public Dictionary<string, string> Parametros { get; set; }
    }
}
