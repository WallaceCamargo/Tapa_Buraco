using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tapa_Buraco.DTO;
using Tapa_Buraco.Util;
using System.Security.Principal;
using System.Security.Claims;
using System.Configuration;

namespace Tapa_Buraco.WebApi
{
    public class WebApiSecurity : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {          
            Response<UsuarioAutenticacao> objResponseUsuario = new Response<UsuarioAutenticacao>();

            TIPO_AUTENTICACAO tipoAutenticacao = TIPO_AUTENTICACAO.Nao_Definido;

            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                #region ACESSO_AUTENTICAÇÃO_APLICAÇÃO

                string msgErroAutenticacao = "";

                if (!(context.Request == null || context.Request.Query == null || context.Request.Query["tipoAutenticacao"] == null || string.IsNullOrEmpty(context.Request.Query["tipoAutenticacao"].ToString())))
                {
                    tipoAutenticacao = (TIPO_AUTENTICACAO)Convert.ToInt32(context.Request.Query["tipoAutenticacao"].Trim());

                    if (tipoAutenticacao == TIPO_AUTENTICACAO.Nao_Definido)
                        msgErroAutenticacao = "Necessário informar via QueryString o parâmetro 'tipoAutenticacao' com o valor '2' (autenticação por usuário).";
                }

                if (string.IsNullOrEmpty(context.UserName))
                    msgErroAutenticacao = "Login não informado.";
                else if (string.IsNullOrEmpty(context.Password))
                    msgErroAutenticacao = "Password não informado.";

                if (string.IsNullOrEmpty(msgErroAutenticacao))
                {
                    if (tipoAutenticacao == TIPO_AUTENTICACAO.Usuario)
                    {
                        objResponseUsuario = await new Business.Usuario().Autenticacao(context.UserName, context.Password);

                        if (objResponseUsuario == null || !objResponseUsuario.Sucesso)
                        {
                            if (objResponseUsuario == null || string.IsNullOrEmpty(objResponseUsuario.Mensagem))
                                msgErroAutenticacao = "ERRO. Problema com autenticação !!!";
                            else
                                msgErroAutenticacao = objResponseUsuario.Mensagem;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(msgErroAutenticacao))
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        NLogHelper.logger.Info("");
                        NLogHelper.logger.Info("Retorno: '{0}'", msgErroAutenticacao);
                        NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        NLogHelper.logger.Info("Fim requisição WORKFLOW-WebApi / (Token())");
                        NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.logger.Info("**********************************************");
                            NLogHelper.logger.Info("ERRO Inesperado");
                            NLogHelper.logger.Info("**********************************************");
                            NLogHelper.logger.Info(ex.ToString());
                            NLogHelper.logger.Info("**********************************************");
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    context.SetError("access_denied", msgErroAutenticacao);
                    return;
                }

                #endregion ACESSO_AUTENTICAÇÃO_APLICAÇÃO

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim("tipoAutenticacao", Convert.ToInt32(tipoAutenticacao).ToString()));
                identity.AddClaim(new Claim("idUsuario", objResponseUsuario.Protocolo));
                //if (objResponseUsuario.List[0] != null && objResponseUsuario.List[0].PerfisAcesso != null &&
                //   objResponseUsuario.List[0].PerfisAcesso.Count > 0)
                //{
                //    foreach (DTO.PerfilAcesso pac in objResponseUsuario.List[0].PerfisAcesso)
                //    {
                //        identity.AddClaim(new Claim(ClaimTypes.Role, pac.PAC_NOME));
                //    }
                //}
                if (objResponseUsuario.List != null && objResponseUsuario.List[0] != null)
                    identity.AddClaim(new Claim("objUsuarioLogado", Newtonsoft.Json.JsonConvert.SerializeObject(objResponseUsuario.List[0].USUARIO)));                

                context.Validated(identity);
                context.Ticket.Properties.Dictionary.Add("idUsuario", objResponseUsuario.Protocolo);

            }
            catch (Exception ex)
            {
                context.SetError("severe_error", "Internal error. Msg: '" + ex.Message + "'");
                throw;
            }

            context.Validated();
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            if (context.TokenIssued)
            {
                if (context.TokenEndpointRequest.GrantType == "password")
                {
                    var accessExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["MINUTES_ACCESS_TOKEN_EXPIRATION"]?.ToString() ?? "1"));
                    context.Properties.ExpiresUtc = accessExpiration;
                }
                else if (context.TokenEndpointRequest.GrantType == "refresh_token")
                {
                    var accessExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["MINUTES_REFRESH_TOKEN_EXPIRATION"]?.ToString() ?? "720"));
                    context.Properties.ExpiresUtc = accessExpiration;
                }
            }

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }

}
