using Tapa_Buraco.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tapa_Buraco.Util;
using System.Web.UI.WebControls;
using System.Net;

namespace Tapa_Buraco.WebSite.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            UsuarioAutenticacao = null;
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> DoLogin(DTO.Login request)
        {
            string returnUrl = "";
            try
            {
                if (string.IsNullOrWhiteSpace(request.LogIn) || string.IsNullOrWhiteSpace(request.Password))
                    return Json(new { Result = false, Record = UsuarioAutenticacao, Message = string.IsNullOrWhiteSpace(request.LogIn) ? "Preencha o campo usuário" : "Preencha o campo senha" });

                string hash = request.Password;
                webApi = new WebApiAutenticacao(request.LogIn, hash);
                DTO.ResponseWebApi<object> objResponseAutenticacao = await webApi.AutenticarAsync();
                bool logado = false;
                string msgErro = "";
                if (objResponseAutenticacao != null && objResponseAutenticacao.Sucesso &&
                    webApi.AutenticadoSucesso && !string.IsNullOrEmpty(webApi.IdUsuario))
                {
                    #region Codigo para quando consultar usuario no banco
                    DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("id", webApi.IdUsuario.Trim());

                    objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Usuario>("Usuario", "GetById", parametros);
                    if (objResponseWebApiMetodo.Sucesso && objResponseWebApiMetodo.TotalRows > 0 &&
                       objResponseWebApiMetodo.List != null)
                    {
                        DTO.Usuario usr = objResponseWebApiMetodo.List[0];
                        if (usr.ATIVO == 1)
                        {
                            await webApi.RefreshAutenticacaoAsync();

                            UsuarioAutenticacao = new DTO.UsuarioAutenticacao();
                            UsuarioAutenticacao.USUARIO = usr;

                            string host = Dns.GetHostName();
                            UsuarioAutenticacao.Host = host;

                            IPHostEntry ipEntry = Dns.GetHostEntry(host);
                            var addressList = ipEntry.AddressList.ToList();

                            foreach (var address in addressList)
                            {
                                UsuarioAutenticacao.IpList += string.Format(", {0}", address.ToString());
                            }

                            if (!string.IsNullOrEmpty(UsuarioAutenticacao.IpList) && UsuarioAutenticacao.IpList.Contains(","))
                                UsuarioAutenticacao.IpList = UsuarioAutenticacao.IpList.Substring(1, UsuarioAutenticacao.IpList.Length - 1).Trim();

                            CreateUserSessionNumber();

                            logado = true;

                            if (TempData["returnUrl"] != null)
                            {
                                returnUrl = TempData["returnUrl"].ToString();
                            }
                        }
                        else
                            msgErro = "Usuário desativado, entre em contato com um Adminstrador do sistema";

                    }
                    #endregion
                }
                else
                    msgErro = objResponseAutenticacao.Mensagem;

                SetUserSessionCookie();
                SetCheckSessionCookie();

                if (UsuarioAutenticacao != null)
                {
                    if (logado)
                    {
                        return Json(new { Result = true, Record = UsuarioAutenticacao, Message = "Login efetuado com sucesso!", ReturnUrl = returnUrl });
                    }
                    else
                        return Json(new { Result = false, Record = UsuarioAutenticacao, Message = msgErro });
                }
                else
                {
                    if (string.IsNullOrEmpty(msgErro))
                        return Json(new { Result = false, Record = UsuarioAutenticacao, Message = "Usuário ou Senha inválidos!" });
                    else
                        return Json(new { Result = false, Record = UsuarioAutenticacao, Message = msgErro });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Record = UsuarioAutenticacao, Message = ex.Message.Replace(@"\", "").Replace("/", "").Replace("\n", "") });
            }
        }
    }
}