using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Util
{
    public class WebApiAutenticacao : IDisposable
    {
        #region PROPRIEDADES PRIVADAS DA CLASSE

        private HttpClient clientHttp;
        private HttpClient clientHttpRefresh;

        private string baseAddress = "";
        private string endPointAccess = "";
        private int timeout = 0;
        private string usernameAcesso = "";
        private string passwordAcesso = "";
        private string token = "";
        private string idAcesso = "";
        private Encoding encodingDefault = Encoding.UTF8; //default Api
        private string mediaTypeXml = "application/xml";
        private string mediaTypeJson = "application/json";
        private string mediaTypeUrlEncoded = "application/x-www-form-urlencoded";
        private string mediaTypeHtml = "text/html";
        private DTO.HeaderAuthenticationWebApi headerAuthentication = null;

        #endregion

        #region PROPRIEDADES PÚBLICAS DA CLASSE

        private bool _autenticadoSucesso = false;
        private string _mensagem = "";
        private string _idUsuario = "";
        private string _redirectURL = "";

        public bool AutenticadoSucesso
        {
            get
            {
                return _autenticadoSucesso;
            }
        }

        public string Mensagem
        {
            get
            {
                return _mensagem;
            }
        }

        public string IdUsuario
        {
            get
            {
                return _idUsuario;
            }
        }

        public string RedirectURL
        {
            get
            {
                return _redirectURL;
            }
        }

        #endregion

        public WebApiAutenticacao(string _usernameAcesso, string _passwordAcesso)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //ServicePointManager.Expect100Continue = false;
            }
            catch { }

            #region INI_VARIAVEIS_CONFIG
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiWKF_BASE_ADDRESS"]))
            {
                baseAddress = Convert.ToString(ConfigurationManager.AppSettings["WebApiWKF_BASE_ADDRESS"]);
            }
            else
                throw new Exception("AppSetting WebApiWKF_BASE_ADDRESS não definido.");
            int i = 0;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiWKF_TIMEOUT"]) &&
                Int32.TryParse(ConfigurationManager.AppSettings["WebApiWKF_TIMEOUT"].ToString(), out i))
            {
                timeout = i;
            }
            else
                throw new Exception("AppSetting WebApiWKF_TIMEOUT não definido ou inválido.");
            if (!string.IsNullOrEmpty(_usernameAcesso))
            {
                usernameAcesso = Uri.EscapeDataString(Convert.ToString(_usernameAcesso));
            }
            else
                throw new Exception("UserNameAcesso deve ser informado.");
            if (!string.IsNullOrEmpty(_passwordAcesso))
            {
                passwordAcesso = Uri.EscapeDataString(Convert.ToString(_passwordAcesso));
            }
            else
                throw new Exception("PasswordAcesso deve ser informado.");
            #endregion INI_VARIAVEIS_CONFIG           
        }

        #region AUTENTICAÇÃO

        #region INI HTTP CLIENT
        private void IniHttpClient()
        {
            try
            {
                clientHttp = new HttpClient();
                clientHttp.BaseAddress = new Uri(baseAddress);
                clientHttp.Timeout = TimeSpan.FromMilliseconds(timeout);

                clientHttp.DefaultRequestHeaders.Clear();
                clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeJson));
                clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeHtml));
                clientHttp.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                //clientHttp.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));


                clientHttpRefresh = new HttpClient();
                clientHttpRefresh.BaseAddress = new Uri(baseAddress);
                clientHttpRefresh.Timeout = TimeSpan.FromMilliseconds(timeout);

                clientHttpRefresh.DefaultRequestHeaders.Clear();
                clientHttpRefresh.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeJson));
                clientHttpRefresh.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeHtml));
                clientHttpRefresh.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                //clientHttpRefresh.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion INI HTTP CLIENT

        public DTO.ResponseWebApi<Object> Autenticar()
        {
            DTO.ResponseWebApi<Object> objResponseWebApi = new DTO.ResponseWebApi<Object>();
            objResponseWebApi.Sucesso = false;
            _autenticadoSucesso = false;
            try
            {
                #region INI
                IniHttpClient();
                #endregion INI

                string cont = string.Format("grant_type=password&username={0}&password={1}", usernameAcesso, passwordAcesso);
                StringContent content = new StringContent(cont, encodingDefault, mediaTypeUrlEncoded);
                string endPoint = "Login/Acesso";//tipoAutenticacao=2 => Por Usuário

                objResponseWebApi.DadosEnviados = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = Task.Run(() => clientHttp.PostAsync(endPoint, content)).Result;
                var ret = response.Content.ReadAsStringAsync().Result;
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {
                    #region VALIDAÇÃO RESPONSE
                    DTO.RespostaRetornoAuthenticationWebApi objRespostaRetornoAuthentication = null;
                    try
                    {
                        objRespostaRetornoAuthentication = JsonConvert.DeserializeObject<DTO.RespostaRetornoAuthenticationWebApi>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoAuthentication = null;
                    }
                    if (objRespostaRetornoAuthentication == null || string.IsNullOrEmpty(objRespostaRetornoAuthentication.access_token))
                        throw new Exception(ret);
                    #endregion VALIDAÇÃO RESPONSE

                    //finalmente tudo OK. SUCESSO AUTENTICAÇÃO
                    headerAuthentication = new DTO.HeaderAuthenticationWebApi();
                    headerAuthentication.access_token = objRespostaRetornoAuthentication.access_token;
                    headerAuthentication.refresh_token = objRespostaRetornoAuthentication.refresh_token;

                    PrepareHeaderForFutureCalls();
                    objResponseWebApi.Sucesso = true;
                    _autenticadoSucesso = true;
                    _idUsuario = objRespostaRetornoAuthentication.idUsuario;
                }
                else
                {
                    #region ERRO AO TENTAR AUTENTICAR
                    _autenticadoSucesso = false;
                    throw new Exception(ret);
                    #endregion ERRO AO TENTAR AUTENTICAR
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "Erro ao autenticar. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public async Task<DTO.ResponseWebApi<Object>> AutenticarAsync()
        {
            DTO.ResponseWebApi<Object> objResponseWebApi = new DTO.ResponseWebApi<Object>();
            objResponseWebApi.Sucesso = false;
            _autenticadoSucesso = false;
            try
            {
                #region INI
                IniHttpClient();
                #endregion INI

                string cont = string.Format("grant_type=password&username={0}&password={1}", usernameAcesso, passwordAcesso);
                StringContent content = new StringContent(cont, encodingDefault, mediaTypeUrlEncoded);
                //string endPoint = "WorkFlow/TokenAcesso?tipoAutenticacao=2";//tipoAutenticacao=2 => Por Usuário
                string endPoint = "Login/Acesso?tipoAutenticacao=2";//tipoAutenticacao=2 => Por Usuário


                objResponseWebApi.DadosEnviados = await content.ReadAsStringAsync();
                HttpResponseMessage response = await clientHttp.PostAsync(endPoint, content);
                var ret = await response.Content.ReadAsStringAsync();
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {
                    #region VALIDAÇÃO RESPONSE
                    DTO.RespostaRetornoAuthenticationWebApi objRespostaRetornoAuthentication = null;
                    try
                    {
                        objRespostaRetornoAuthentication = JsonConvert.DeserializeObject<DTO.RespostaRetornoAuthenticationWebApi>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoAuthentication = null;
                    }
                    if (objRespostaRetornoAuthentication == null || string.IsNullOrEmpty(objRespostaRetornoAuthentication.access_token))
                        throw new Exception(ret);
                    #endregion VALIDAÇÃO RESPONSE

                    //finalmente tudo OK. SUCESSO AUTENTICAÇÃO
                    headerAuthentication = new DTO.HeaderAuthenticationWebApi();
                    headerAuthentication.access_token = objRespostaRetornoAuthentication.access_token;
                    headerAuthentication.refresh_token = objRespostaRetornoAuthentication.refresh_token;

                    PrepareHeaderForFutureCalls();
                    objResponseWebApi.Sucesso = true;
                    _autenticadoSucesso = true;
                    _idUsuario = objRespostaRetornoAuthentication.idUsuario;
                }
                else
                {
                    #region ERRO AO TENTAR AUTENTICAR
                    _autenticadoSucesso = false;

                    #region VALIDAÇÃO RESPONSE
                    DTO.RespostaRetornoERROAuthenticationWebApi objRespostaRetornoERROAuthenticationWebApiWKF = null;
                    try
                    {
                        objRespostaRetornoERROAuthenticationWebApiWKF = JsonConvert.DeserializeObject<DTO.RespostaRetornoERROAuthenticationWebApi>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoERROAuthenticationWebApiWKF = null;
                    }
                    if (objRespostaRetornoERROAuthenticationWebApiWKF == null || string.IsNullOrEmpty(objRespostaRetornoERROAuthenticationWebApiWKF.error_description))
                        throw new Exception(ret);
                    #endregion VALIDAÇÃO RESPONSE

                    objResponseWebApi.Sucesso = false;
                    _autenticadoSucesso = false;
                    objResponseWebApi.Mensagem = "Erro ao autenticar. " + objRespostaRetornoERROAuthenticationWebApiWKF.error_description;
                    objResponseWebApi.StackTrace = "";
                    #endregion ERRO AO TENTAR AUTENTICAR
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "Erro ao autenticar. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public DTO.ResponseWebApi<Object> RefreshAutenticacao()
        {
            DTO.ResponseWebApi<Object> objResponseWebApi = new DTO.ResponseWebApi<Object>();
            objResponseWebApi.Sucesso = false;
            _autenticadoSucesso = false;
            try
            {
                string cont = string.Format("grant_type=refresh_token&refresh_token={0}", headerAuthentication?.refresh_token ?? "");
                StringContent content = new StringContent(cont, encodingDefault, mediaTypeUrlEncoded);
                string endPoint = "WorkFlow/TokenAcesso";

                objResponseWebApi.DadosEnviados = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = Task.Run(() => clientHttpRefresh.PostAsync(endPoint, content)).Result;
                var ret = response.Content.ReadAsStringAsync().Result;
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {
                    #region VALIDAÇÃO RESPONSE
                    DTO.RespostaRetornoAuthenticationWebApi objRespostaRetornoAuthentication = null;
                    try
                    {
                        objRespostaRetornoAuthentication = JsonConvert.DeserializeObject<DTO.RespostaRetornoAuthenticationWebApi>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoAuthentication = null;
                    }
                    if (objRespostaRetornoAuthentication == null || string.IsNullOrEmpty(objRespostaRetornoAuthentication.access_token))
                        throw new Exception(ret);
                    #endregion VALIDAÇÃO RESPONSE

                    //finalmente tudo OK. SUCESSO AUTENTICAÇÃO
                    headerAuthentication = new DTO.HeaderAuthenticationWebApi();
                    headerAuthentication.access_token = objRespostaRetornoAuthentication.access_token;
                    headerAuthentication.refresh_token = objRespostaRetornoAuthentication.refresh_token;

                    PrepareHeaderForFutureCalls();
                    objResponseWebApi.Sucesso = true;
                    _autenticadoSucesso = true;
                    _idUsuario = objRespostaRetornoAuthentication.idUsuario;
                }
                else
                {
                    #region ERRO AO TENTAR AUTENTICAR
                    _autenticadoSucesso = false;
                    throw new Exception(ret);
                    #endregion ERRO AO TENTAR AUTENTICAR
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "Erro ao autenticar. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public async Task<DTO.ResponseWebApi<Object>> RefreshAutenticacaoAsync()
        {
            DTO.ResponseWebApi<Object> objResponseWebApi = new DTO.ResponseWebApi<Object>();
            objResponseWebApi.Sucesso = false;
            _autenticadoSucesso = false;
            try
            {
                string cont = string.Format("grant_type=refresh_token&refresh_token={0}", headerAuthentication?.refresh_token ?? "");
                StringContent content = new StringContent(cont, encodingDefault, mediaTypeUrlEncoded);
                string endPoint = "WorkFlow/TokenAcesso";

                objResponseWebApi.DadosEnviados = await content.ReadAsStringAsync();
                HttpResponseMessage response = await clientHttpRefresh.PostAsync(endPoint, content);
                var ret = await response.Content.ReadAsStringAsync();
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {
                    #region VALIDAÇÃO RESPONSE
                    DTO.RespostaRetornoAuthenticationWebApi objRespostaRetornoAuthentication = null;
                    try
                    {
                        objRespostaRetornoAuthentication = JsonConvert.DeserializeObject<DTO.RespostaRetornoAuthenticationWebApi>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoAuthentication = null;
                    }
                    if (objRespostaRetornoAuthentication == null || string.IsNullOrEmpty(objRespostaRetornoAuthentication.access_token))
                        throw new Exception(ret);
                    #endregion VALIDAÇÃO RESPONSE

                    //finalmente tudo OK. SUCESSO AUTENTICAÇÃO
                    headerAuthentication = new DTO.HeaderAuthenticationWebApi();
                    headerAuthentication.access_token = objRespostaRetornoAuthentication.access_token;
                    headerAuthentication.refresh_token = objRespostaRetornoAuthentication.refresh_token;

                    PrepareHeaderForFutureCalls();
                    objResponseWebApi.Sucesso = true;
                    _autenticadoSucesso = true;
                    _idUsuario = objRespostaRetornoAuthentication.idUsuario;
                }
                else
                {
                    #region ERRO AO TENTAR AUTENTICAR
                    _autenticadoSucesso = false;
                    throw new Exception(ret);
                    #endregion ERRO AO TENTAR AUTENTICAR
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                _autenticadoSucesso = false;
                objResponseWebApi.Mensagem = "Erro ao autenticar. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        private void PrepareHeaderForFutureCalls()
        {
            try
            {
                clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headerAuthentication.access_token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion AUTENTICAÇÃO

        public DTO.ResponseWebApi<T> ExecuteCustomPost<T>(string controler, string metodo, string json) where T : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                StringContent content = new StringContent(string.IsNullOrEmpty(json) ? "" : json.Trim(), encodingDefault, mediaTypeJson);

                objResponseWebApi.DadosEnviados = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = Task.Run(() => clientHttp.PostAsync(controler + "/" + metodo, content)).Result;
                var ret = response.Content.ReadAsStringAsync().Result;
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {//comunicação rolou com sucesso. (Ainda não quer dizer que a operação foi concluída com sucesso)
                    #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                    try
                    {
                        objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoServico = null;
                    }
                    #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    if (objRespostaRetornoServico == null)
                        throw new Exception("Erro ao tentar recuperar os dados. Retorno inesperado. Msg: '" + ret + "'");
                    else if (objRespostaRetornoServico.Sucesso == false)
                    {
                        objResponseWebApi.Mensagem = objRespostaRetornoServico.Mensagem;
                        objResponseWebApi.StackTrace = objRespostaRetornoServico.StackTrace;
                        objResponseWebApi.Sucesso = false;
                        objResponseWebApi.List = null;
                        objResponseWebApi.TotalRows = 0;
                    }
                    else
                    {
                        objResponseWebApi.Mensagem = objRespostaRetornoServico.Mensagem;
                        objResponseWebApi.Protocolo = objRespostaRetornoServico.Protocolo;
                        objResponseWebApi.Sucesso = true;
                        objResponseWebApi.List = objRespostaRetornoServico.List != null ? objRespostaRetornoServico.List : null;
                        objResponseWebApi.TotalRows = objRespostaRetornoServico.List != null ? objRespostaRetornoServico.List.Count : 0;
                    }
                }
                else
                {
                    #region ERRO NA CHAMADA DO MÉTODO
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        #region UNAUTHORIZED ERROR
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("UNAUTHORIZED ERROR - " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion UNAUTHORIZED ERROR
                    }
                    else
                    {//outros erros
                        #region OUTROS ERROS
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("Erro " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception(objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception(objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion OUTROS ERROS   
                    }
                    #endregion ERRO NA CHAMADA DO MÉTODO
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public DTO.ResponseWebApi<T> ExecuteAutoPost<T>(string controler, string metodo, object obj) where T : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                objResponseWebApi = ExecuteCustomPost<T>(controler, metodo, json);
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public DTO.ResponseWebApi<T> ExecuteCustomGet<T>(string controler, string metodo, Dictionary<string, string> parametros = null) where T : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                string endPointAndParameters = controler + "/" + metodo;
                #region PARÂMETROS QUERY STRING
                if (parametros != null && parametros.Count > 0)
                {//tem parâmetros para enviar no query string
                    for (int i = 1; i <= parametros.Count; i++)
                    {
                        if (i == 1)//primeiro parâmetros
                            endPointAndParameters += "/?";
                        else
                            endPointAndParameters += "&";

                        endPointAndParameters += string.Format("{0}={1}",
                            parametros.ElementAt(i - 1).Key, parametros.ElementAt(i - 1).Value);
                    }
                }
                #endregion PARÂMETROS QUERY STRING

                objResponseWebApi.DadosEnviados = endPointAndParameters;
                HttpResponseMessage response = Task.Run(() => clientHttp.GetAsync(endPointAndParameters)).Result;
                var ret = response.Content.ReadAsStringAsync().Result;
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {//comunicação rolou com sucesso. (Ainda não quer dizer que a operação foi concluída com sucesso)
                    #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    DTO.ResponseWebApi<T> objRespostaConsultaGET = null;
                    try
                    {
                        objRespostaConsultaGET = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaConsultaGET = null;
                    }
                    #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    if (objRespostaConsultaGET == null)
                        throw new Exception("Erro ao tentar recuperar resultado da consulta. Retorno inesperado. Msg: '" + ret + "'");
                    else if (objRespostaConsultaGET.Sucesso == false)
                    {
                        objResponseWebApi.Mensagem = objRespostaConsultaGET.Mensagem;
                        objResponseWebApi.StackTrace = objRespostaConsultaGET.StackTrace;
                        objResponseWebApi.Sucesso = false;
                        objResponseWebApi.List = null;
                        objResponseWebApi.TotalRows = 0;
                    }
                    else
                    {
                        objResponseWebApi.Mensagem = objRespostaConsultaGET.Mensagem;
                        objResponseWebApi.Protocolo = objRespostaConsultaGET.Protocolo;
                        objResponseWebApi.Sucesso = true;
                        objResponseWebApi.List = objRespostaConsultaGET.List != null ? objRespostaConsultaGET.List : null;
                        objResponseWebApi.TotalRows = objRespostaConsultaGET.List != null ? objRespostaConsultaGET.List.Count : 0;
                    }
                }
                else
                {
                    #region ERRO NA CHAMADA DO MÉTODO
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        #region UNAUTHORIZED ERROR
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("UNAUTHORIZED ERROR - " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion UNAUTHORIZED ERROR
                    }
                    else
                    {//outros erros
                        #region OUTROS ERROS
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("Erro " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception(objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception(objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion OUTROS ERROS   
                    }
                    #endregion ERRO NA CHAMADA DO MÉTODO
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }


        public async Task<DTO.ResponseWebApi<T>> ExecuteCustomPostAsync<T>(string controler, string metodo, string json) where T : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                StringContent content = new StringContent(string.IsNullOrEmpty(json) ? "" : json.Trim(), encodingDefault, mediaTypeJson);

                objResponseWebApi.DadosEnviados = await content.ReadAsStringAsync();
                HttpResponseMessage response = await clientHttp.PostAsync(controler + "/" + metodo, content);
                var ret = await response.Content.ReadAsStringAsync();
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {//comunicação rolou com sucesso. (Ainda não quer dizer que a operação foi concluída com sucesso)
                    #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                    try
                    {
                        objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaRetornoServico = null;
                    }
                    #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    if (objRespostaRetornoServico == null)
                        throw new Exception("Erro ao tentar recuperar os dados. Retorno inesperado. Msg: '" + ret + "'");
                    else if (objRespostaRetornoServico.Sucesso == false)
                    {
                        objResponseWebApi.Mensagem = objRespostaRetornoServico.Mensagem;
                        objResponseWebApi.StackTrace = objRespostaRetornoServico.StackTrace;
                        objResponseWebApi.Sucesso = false;
                        objResponseWebApi.List = null;
                        objResponseWebApi.TotalRows = 0;
                    }
                    else
                    {
                        objResponseWebApi.Mensagem = objRespostaRetornoServico.Mensagem;
                        objResponseWebApi.Protocolo = objRespostaRetornoServico.Protocolo;
                        objResponseWebApi.Sucesso = true;
                        objResponseWebApi.List = objRespostaRetornoServico.List != null ? objRespostaRetornoServico.List : null;
                        objResponseWebApi.TotalRows = objRespostaRetornoServico.List != null ? objRespostaRetornoServico.List.Count : 0;
                    }
                }
                else
                {
                    #region ERRO NA CHAMADA DO MÉTODO
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        #region UNAUTHORIZED ERROR
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("UNAUTHORIZED ERROR - " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion UNAUTHORIZED ERROR
                    }
                    else
                    {//outros erros
                        #region OUTROS ERROS
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("Erro " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception(objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception(objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion OUTROS ERROS   
                    }
                    #endregion ERRO NA CHAMADA DO MÉTODO
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public async Task<DTO.ResponseWebApi<T>> ExecuteAutoPostAsync<T, U>(DTO.Request<U> request, string controler, string metodo) where T : class where U : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                objResponseWebApi = await ExecuteCustomPostAsync<T>(controler, metodo, json);
                objResponseWebApi.Sucesso = true;
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        public async Task<DTO.ResponseWebApi<T>> ExecuteCustomGetAsync<T>(string controler, string metodo, Dictionary<string, string> parametros = null) where T : class
        {
            DTO.ResponseWebApi<T> objResponseWebApi = new DTO.ResponseWebApi<T>();
            objResponseWebApi.Sucesso = false;
            try
            {
                string endPointAndParameters = controler + "/" + metodo;
                #region PARÂMETROS QUERY STRING
                if (parametros != null && parametros.Count > 0)
                {//tem parâmetros para enviar no query string
                    for (int i = 1; i <= parametros.Count; i++)
                    {
                        if (i == 1)//primeiro parâmetro
                            endPointAndParameters += "/?";
                        else
                            endPointAndParameters += "&";

                        endPointAndParameters += string.Format("{0}={1}",
                            parametros.ElementAt(i - 1).Key, parametros.ElementAt(i - 1).Value);
                    }
                }
                #endregion PARÂMETROS QUERY STRING

                objResponseWebApi.DadosEnviados = endPointAndParameters;
                HttpResponseMessage response = await clientHttp.GetAsync(endPointAndParameters);
                var ret = await response.Content.ReadAsStringAsync();
                objResponseWebApi.DadosRecebidos = ret;

                if (response.IsSuccessStatusCode)
                {//comunicação rolou com sucesso. (Ainda não quer dizer que a operação foi concluída com sucesso)
                    #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    DTO.ResponseWebApi<T> objRespostaConsultaGET = null;
                    try
                    {
                        objRespostaConsultaGET = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                    }
                    catch (Exception ex)
                    {
                        objRespostaConsultaGET = null;
                    }
                    #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                    if (objRespostaConsultaGET == null)
                        throw new Exception("Erro ao tentar recuperar resultado da consulta. Retorno inesperado. Msg: '" + ret + "'");
                    else if (objRespostaConsultaGET.Sucesso == false)
                    {
                        objResponseWebApi.Mensagem = objRespostaConsultaGET.Mensagem;
                        objResponseWebApi.StackTrace = objRespostaConsultaGET.StackTrace;
                        objResponseWebApi.Sucesso = false;
                        objResponseWebApi.List = null;
                        objResponseWebApi.TotalRows = 0;
                    }
                    else
                    {
                        objResponseWebApi.Mensagem = objRespostaConsultaGET.Mensagem;
                        objResponseWebApi.Protocolo = objRespostaConsultaGET.Protocolo;
                        objResponseWebApi.Sucesso = true;
                        objResponseWebApi.List = objRespostaConsultaGET.List != null ? objRespostaConsultaGET.List : null;
                        objResponseWebApi.TotalRows = objRespostaConsultaGET.TotalRows;
                    }
                }
                else
                {
                    #region ERRO NA CHAMADA DO MÉTODO
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        #region UNAUTHORIZED ERROR
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("UNAUTHORIZED ERROR - " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception("UNAUTHORIZED ERROR - " + objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion UNAUTHORIZED ERROR
                    }
                    else
                    {//outros erros
                        #region OUTROS ERROS
                        #region TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        DTO.ResponseWebApi<T> objRespostaRetornoServico = null;
                        try
                        {
                            objRespostaRetornoServico = JsonConvert.DeserializeObject<DTO.ResponseWebApi<T>>(ret);
                        }
                        catch (Exception ex)
                        {
                            objRespostaRetornoServico = null;
                        }
                        #endregion TENTAR PEGAR OBJETO RESPOSTA RETORNO
                        if (objRespostaRetornoServico == null)
                            throw new Exception("Erro " + Convert.ToInt32(response.StatusCode) + " - " + response.StatusCode.ToString() + " - " + ret);
                        else
                        {
                            if (string.IsNullOrEmpty(objRespostaRetornoServico.StackTrace))
                                throw new Exception(objRespostaRetornoServico.Mensagem);
                            else
                                throw new Exception(objRespostaRetornoServico.Mensagem + " - Detalhes: " + objRespostaRetornoServico.StackTrace);
                        }
                        #endregion OUTROS ERROS   
                    }
                    #endregion ERRO NA CHAMADA DO MÉTODO
                }
            }
            #region EXCEPTIONS
            catch (TaskCanceledException ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "'Timeout'. " + Util.GetAllExceptionsMessages(ex);
                objResponseWebApi.StackTrace = ex.ToString();
            }
            catch (Exception ex)
            {
                objResponseWebApi.Sucesso = false;
                objResponseWebApi.Mensagem = "Erro: '" + Util.GetAllExceptionsMessages(ex) + "'.";
                objResponseWebApi.StackTrace = ex.ToString();
            }
            #endregion EXCEPTIONS

            return objResponseWebApi;
        }

        #region MÉTODOS INTERNOS DA CLASSE
        public void Dispose()
        {
            try
            {
                clientHttp = null;
            }
            catch { }
        }

        #endregion MÉTODOS INTERNOS DA CLASSE

    }
}
