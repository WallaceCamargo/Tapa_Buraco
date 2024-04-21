using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DTO
{
    public class Authentication
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Enum Authenticate { get; set; }
        public List<Access> Access { get; set; }
    }
    public class Access
    {
        public string ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Menu { get; set; }
    }
}
