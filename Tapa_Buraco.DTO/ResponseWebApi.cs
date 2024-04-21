using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class ResponseWebApi<T> where T : class
    {
        public ResponseWebApi()
        {
            List = new List<T>();
        }

        public bool Sucesso { get; set; }
        public string ErroCodigo { get; set; }
        public string Mensagem { get; set; }
        public string Protocolo { get; set; }
        public int TotalRows { get; set; }
        public List<T> List { get; set; }
        public string StackTrace { get; set; }
        public string DadosEnviados { get; set; }
        public string DadosRecebidos { get; set; }

        public override string ToString()
        {
            string list = "";
            if (List == null)
                list = "null";
            else if (List.Count() == 0)
                list = "0 itens";
            else
                list = List.Count() + " itens of type *" + List[0].GetType().ToString() + "*";

            return "Response [Sucesso: '" + Sucesso.ToString() + "', " +
                "ErroCodigo: '" + ErroCodigo + "', " +
                "Mensagem: '" + Mensagem + "', " +
                "Protocolo: '" + Protocolo + "', " +
                "TotalRows: '" + TotalRows + "', " +
                "List: '" + list + "']";
        }
    }
}
