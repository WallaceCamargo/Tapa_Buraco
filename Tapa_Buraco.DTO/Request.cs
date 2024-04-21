using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class Request<T> where T : class
    {
        public Request()
        {
            List = new List<T>();
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public UsuarioAutenticacao UsuarioAutenticacao { get; set; }
        public List<T> List { get; set; }
        public T Item { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }
}
