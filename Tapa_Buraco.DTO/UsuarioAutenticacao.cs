using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class UsuarioAutenticacao
    {
        public DTO.Usuario USUARIO { get; set; }
        public string Host { get; set; }
        public string IpList { get; set; }
        public string Session { get; set; }
        public DateTime SessionValidUntil { get; set; }

        //= null, entao sempre refresh token
        public int? QtdAjaxRequestBeforeRefreshToken { get; set; }

        public int QtdAjaxRequestAtual { get; set; }
    }
}
