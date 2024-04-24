using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class Usuario : Logradouro
    {
        public Usuario()
        {

        }

        public int ID { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public int ATIVO { get; set; }
        public int ADMIN { get; set; }
        public DateTime DT_INSERT { get; set; }
        public DateTime? DT_DELETE { get; set;}
        public string SETOR { get; set; }
        public string ATIVO_STRING
        {
            get
            {
                if (ATIVO == 1)
                    return "SIM";
                else
                    return "NÃO";
            }
        }
        public string ADMIN_STRING
        {
            get
            {
                if (ADMIN == 1)
                    return "ADMIN";
                else
                    return "SOLICITANTE";
            }
        }


    }
}
