using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class RespostaRetornoAuthenticationWebApi
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string idUsuario { get; set; }
        public string loginAcesso { get; set; }
        public string redirectURL { get; set; }
    }
}
