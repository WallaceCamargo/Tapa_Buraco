using Tapa_Buraco.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Tapa_Buraco.WebApi.Controllers
{
    [RoutePrefix("api/v1/Solicitacao")]
    public class SolicitacaoController : ApiController
    {
        [AuthorizeAttributeOverride()]
        [HttpGet]
        [Route("GetAll")]
        public async Task<HttpResponseMessage> GetAll([FromUri] string id_usuario = null, [FromUri] int startIndexPaging = 0, [FromUri] int pageSizePaging = 15)
        {
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            string caminho = "Solicitacao/GetAll()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = $"id_usuario = {id_usuario}";
                    NLogHelper.InicioRequisição(caminho, parametros);
                    #endregion Nlog                    
                }
                catch (Exception ex)
                {
                    //se houver problema com o LOG, ainda assim não interrompo a requisição.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                objResponse = await new Business.Solicitacao().GetAll(id_usuario, startIndexPaging, pageSizePaging);
                if (objResponse != null && objResponse.Sucesso) 
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.OK(200), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Consulta feita com sucesso";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
                else
                {
                    #region LOGs 
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Sucesso = false;
                    objResponse.Mensagem = "Erro ao fazer a consulta";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
                }

            }
            catch (Exception ex)
            {

                objResponse.Sucesso = false;
                objResponse.Mensagem = Util.Util.GetAllExceptionsMessages(ex);

                #region LOGs
                try
                {
                    #region Nlog
                    string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                        (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                    NLogHelper.FimRequisição(caminho, retorno);
                    #endregion Nlog
                }
                catch (Exception ex2)
                {
                    //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex2);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }

        [AuthorizeAttributeOverride()]
        [HttpGet]
        [Route("GetById")]
        public async Task<HttpResponseMessage> GetById([FromUri] int id)
        {
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            string caminho = "Solicitacao/GetById()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = $"ID = {id}";
                    NLogHelper.InicioRequisição(caminho, parametros);
                    #endregion Nlog                    
                }
                catch (Exception ex)
                {
                    //se houver problema com o LOG, ainda assim não interrompo a requisição.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                objResponse = await new Business.Solicitacao().GetById(id);

                if (objResponse != null && objResponse.Sucesso)
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.OK(200), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Consulta feita com sucesso";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
                else
                {
                    #region LOGs 
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Sucesso = false;
                    objResponse.Mensagem = "Erro ao fazer a consulta";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
                }

            }
            catch (Exception ex)
            {
                objResponse.Sucesso = false;
                objResponse.Mensagem = Util.Util.GetAllExceptionsMessages(ex);

                #region LOGs
                try
                {
                    #region Nlog
                    string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                        (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                    NLogHelper.FimRequisição(caminho, retorno);
                    #endregion Nlog
                }
                catch (Exception ex2)
                {
                    //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex2);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }

        [AuthorizeAttributeOverride()]
        [HttpPost]
        [Route("Post")]
        public async Task<HttpResponseMessage> Post([FromBody] DTO.Request<DTO.Solicitacao> objSolicitacaoRequest)
        {
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            string caminho = "Solicitacao/Post()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = "IdUsuarioRequisicao= '" + objSolicitacaoRequest.UsuarioAutenticacao.USUARIO.ID + "', " +
                        (objSolicitacaoRequest == null ? "objUsuarioRequest [Nulo]" : Newtonsoft.Json.JsonConvert.SerializeObject(objSolicitacaoRequest));
                    NLogHelper.InicioRequisição(caminho, parametros);
                    #endregion Nlog                    
                }
                catch (Exception ex)
                {
                    //se houver problema com o LOG, ainda assim não interrompo a requisição.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                objResponse = await new Business.Solicitacao().Save(objSolicitacaoRequest);
                if (objResponse != null && objResponse.Sucesso)
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.OK(200), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Dados do Usuário foram salvos com sucesso.";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
                else
                {
                    #region LOGs 
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Erro ao inserir usuário.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Sucesso = false;
                objResponse.Mensagem = Util.Util.GetAllExceptionsMessages(ex);

                #region LOGs
                try
                {
                    #region Nlog
                    string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                        (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                    NLogHelper.FimRequisição(caminho, retorno);
                    #endregion Nlog
                }
                catch (Exception ex2)
                {
                    //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex2);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }

        [AuthorizeAttributeOverride()]
        [HttpPost]
        [Route("Put")]
        public async Task<HttpResponseMessage> Put([FromBody] DTO.Request<DTO.Solicitacao> objSolicitacaoRequest)
        {
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            string caminho = "Solicitacao/Put()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = "IdUsuarioRequisicao= '" + objSolicitacaoRequest.UsuarioAutenticacao.USUARIO.ID + "', " +
                        (objSolicitacaoRequest == null ? "objSolicitacaoRequest [Nulo]" : Newtonsoft.Json.JsonConvert.SerializeObject(objSolicitacaoRequest));
                    NLogHelper.InicioRequisição(caminho, parametros);
                    #endregion Nlog                    
                }
                catch (Exception ex)
                {
                    //se houver problema com o LOG, ainda assim não interrompo a requisição.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                objResponse = await new Business.Solicitacao().Update(objSolicitacaoRequest);
                if (objResponse != null && objResponse.Sucesso)
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.OK(200), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Dados do Usuário foram alterados com sucesso.";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
                else
                {
                    #region LOGs 
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Sucesso = false;
                objResponse.Mensagem = Util.Util.GetAllExceptionsMessages(ex);

                #region LOGs
                try
                {
                    #region Nlog
                    string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                        (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                    NLogHelper.FimRequisição(caminho, retorno);
                    #endregion Nlog
                }
                catch (Exception ex2)
                {
                    //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex2);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }

        [AuthorizeAttributeOverride()]
        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> Delete([FromBody] DTO.Request<DTO.Solicitacao> objUsuarioRequest)
        {
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            string caminho = "Solicitacao/Delete()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = "IdUsuarioRequisicao= '" + objUsuarioRequest.UsuarioAutenticacao.USUARIO.ID + "', " +
                        (objUsuarioRequest == null ? "objUsuarioRequest [Nulo]" : Newtonsoft.Json.JsonConvert.SerializeObject(objUsuarioRequest));
                    NLogHelper.InicioRequisição(caminho, parametros);
                    #endregion Nlog                    
                }
                catch (Exception ex)
                {
                    //se houver problema com o LOG, ainda assim não interrompo a requisição.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                objResponse = await new Business.Solicitacao().Delete(objUsuarioRequest);
                if (objResponse != null && objResponse.Sucesso)
                {
                    #region LOGs                
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.OK(200), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    objResponse.Mensagem = "Solicitação foi excluido com sucesso!";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
                else
                {
                    #region LOGs 
                    try
                    {
                        #region Nlog
                        string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                            (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                        NLogHelper.FimRequisição(caminho, retorno);
                        #endregion Nlog
                    }
                    catch (Exception ex)
                    {
                        //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                        try
                        {
                            #region Nlog
                            NLogHelper.ErroNlog(caminho, ex);
                            #endregion Nlog
                        }
                        catch { }
                    }
                    #endregion LOGs

                    return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.Sucesso = false;
                objResponse.Mensagem = Util.Util.GetAllExceptionsMessages(ex);

                #region LOGs
                try
                {
                    #region Nlog
                    string retorno = "Retorno => " + "HttpStatusCode.BadRequest(400), " +
                        (objResponse == null ? "objResponse [Nulo]" : objResponse.ToString());
                    NLogHelper.FimRequisição(caminho, retorno);
                    #endregion Nlog
                }
                catch (Exception ex2)
                {
                    //se houver problema com o LOG, ainda assim retorno a Msg ao cliente.
                    try
                    {
                        #region Nlog
                        NLogHelper.ErroNlog(caminho, ex2);
                        #endregion Nlog
                    }
                    catch { }
                }
                #endregion LOGs

                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }
    }
}
