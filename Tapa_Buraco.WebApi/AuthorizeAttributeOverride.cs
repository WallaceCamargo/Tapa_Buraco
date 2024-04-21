using Tapa_Buraco.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Tapa_Buraco.WebApi
{
    public class AuthorizeAttributeOverride : AuthorizeAttribute
    {
        public AuthorizeAttributeOverride() { }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string token = string.Empty;

            try
            {
                token = (actionContext.Request.Headers.Any(x => x.Key == "Authorization")) ? actionContext.Request.Headers.Where(x => x.Key == "Authorization").FirstOrDefault().Value.SingleOrDefault().Replace("Bearer ", "") : "";

                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Parâmetro Header 'Authorization Bearer' deve ser enviado. Acesso negado.");
                    return;
                }

                string idUsuario = null;
                string tipoAutenticacao = null;
                string _vUsuarioLogado = null;
                DTO.Usuario objUsuarioLogado = null;
                if (ClaimsPrincipal.Current != null && ClaimsPrincipal.Current.Claims != null)
                {
                    var claim_IdUsuario = ClaimsPrincipal.Current.Claims.FirstOrDefault(o => o.Type.Equals("idUsuario"));
                    var claim_TipoAutenticacao = ClaimsPrincipal.Current.Claims.FirstOrDefault(o => o.Type.Equals("tipoAutenticacao"));
                    var claim_objUsuarioLogado = ClaimsPrincipal.Current.Claims.FirstOrDefault(o => o.Type.Equals("objUsuarioLogado"));

                    if (claim_IdUsuario != null)
                    {
                        idUsuario = claim_IdUsuario.Value;
                    }
                    if (claim_TipoAutenticacao != null)
                    {
                        tipoAutenticacao = claim_TipoAutenticacao.Value;
                    }
                    if (claim_objUsuarioLogado != null)
                    {
                        _vUsuarioLogado = claim_objUsuarioLogado.Value;
                        if (!string.IsNullOrEmpty(_vUsuarioLogado) && _vUsuarioLogado.Trim() != "")
                            objUsuarioLogado = JsonConvert.DeserializeObject<DTO.Usuario>(_vUsuarioLogado);
                    }
                }

                if ((string.IsNullOrEmpty(tipoAutenticacao) || tipoAutenticacao.Trim() == "" || tipoAutenticacao.Trim() == "0") ||                    
                    ((string.IsNullOrEmpty(idUsuario) || idUsuario.Trim() == "" || idUsuario.Trim() == "0" || string.IsNullOrEmpty(_vUsuarioLogado) || objUsuarioLogado == null) && (tipoAutenticacao == "2")))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Requisição inválida. Token expirado. Favor solicitar um novo token de acesso.");
                    return;
                }
                //else if (tipoAutenticacaoSecur != TIPO_AUTENTICACAO.Nao_Definido &&
                //         ((TIPO_AUTENTICACAO)Convert.ToInt32(tipoAutenticacao.Trim())) != tipoAutenticacaoSecur)
                //{
                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Requisição inválida. Sem acesso para executar essa ação com esse tipo de autenticação!");
                //    return;
                //}
            }
            catch (Exception err)
            {
                //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                try
                {
                    #region Nlog
                    NLogHelper.logger.Info("**********************************************");
                    NLogHelper.logger.Info("ERRO Inesperado - OnAuthorization");
                    NLogHelper.logger.Info("**********************************************");
                    NLogHelper.logger.Info(err.ToString());
                    NLogHelper.logger.Info("**********************************************");
                    #endregion Nlog
                }
                catch { }
            }
           
            base.OnAuthorization(actionContext);
        }
    }
}