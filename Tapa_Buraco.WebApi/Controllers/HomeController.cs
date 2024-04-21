using Tapa_Buraco.DTO;
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
    [System.Web.Mvc.RoutePrefix("api/v1/Home")]
    public class HomeController : ApiController
    {

        [AuthorizeAttributeOverride()]
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("GetAll")]
        public async Task<HttpResponseMessage> GetAll([FromUri] int id, string nome = null, string joint = null, string loa = null, string numeroViagem = null, string agencia = null, [FromUri] int startIndexPaging = 0, [FromUri] int pageSizePaging = 25)
        {
            DTO.Response<DTO.Home> objResponse = new DTO.Response<DTO.Home>();
            string caminho = "Home/GetAll()";

            try
            {
                #region LOGs
                try
                {
                    #region Nlog
                    string parametros = $"Nome = {nome}";
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

                objResponse = await new Business.Home().GetAll(id, nome, joint, loa, numeroViagem, agencia, startIndexPaging, pageSizePaging);
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

    }
}
