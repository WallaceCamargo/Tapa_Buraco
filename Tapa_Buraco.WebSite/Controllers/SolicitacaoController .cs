using Tapa_Buraco.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Tapa_Buraco.WebSite.Controllers
{
    public class SolicitacaoController : BaseController
    {        
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _ModalExluir()
        {
            return PartialView();
        }

        [Route("Solicitacao/Cadastro/{id}")]
        public ActionResult Cadastro(string id)
        {
            ViewData["id"] = id == null ? 0 : Convert.ToInt32(id);

            return View();
        }


        public async Task<JsonResult> GetAll(string id_usuario, int jtStartIndex = 0, int jtPageSize = 20)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("id_usuario", string.IsNullOrEmpty(id_usuario) ? "" : id_usuario.Trim());
                parametros.Add("startIndexPaging", jtStartIndex.ToString().Trim());
                parametros.Add("pageSizePaging", jtPageSize.ToString().Trim());

                List<DTO.Solicitacao> objListSolicitacao = new List<DTO.Solicitacao>();
                objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Solicitacao>("Solicitacao", "GetAll", parametros);
                if (objResponseWebApiMetodo.Sucesso)
                {
                    if (objResponseWebApiMetodo.TotalRows > 0 &&
                        objResponseWebApiMetodo.List != null)
                    {
                        objListSolicitacao = objResponseWebApiMetodo.List;
                    }
                }
                else
                {
                    throw new Exception(objResponseWebApiMetodo.Mensagem);
                }

                return Json(new { Result = "OK", Records = objListSolicitacao, TotalRecordCount = ((objListSolicitacao == null || objListSolicitacao.Count <= 0) ? 0 : (objListSolicitacao[0] == null ? 0 : objResponseWebApiMetodo.TotalRows)) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("id", id.ToString().Trim());

                List<DTO.Solicitacao> objListUsuario = new List<DTO.Solicitacao>();
                objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Solicitacao>("Solicitacao", "GetById", parametros);
                if (objResponseWebApiMetodo.Sucesso)
                {
                    if (objResponseWebApiMetodo.TotalRows > 0 &&
                        objResponseWebApiMetodo.List != null)
                    {
                        objListUsuario = objResponseWebApiMetodo.List;
                    }
                }
                else
                {
                    throw new Exception(objResponseWebApiMetodo.Mensagem);
                }

                return Json(new { Result = "OK", Records = objListUsuario, TotalRecordCount = ((objListUsuario == null || objListUsuario.Count <= 0) ? 0 : (objListUsuario[0] == null ? 0 : objResponseWebApiMetodo.TotalRows)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<JsonResult> Post(DTO.Solicitacao solicitacao)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;
                DTO.Solicitacao objSolicitacao = new DTO.Solicitacao();

                string msgErro = "";
                if (true)
                {
                    objSolicitacao = solicitacao;
                    DTO.Request<DTO.Solicitacao> objRequest = new Request<Solicitacao>();
                    objRequest.Item = objSolicitacao;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;
                    objRequest.Item.ID_USUARIO = UsuarioAutenticacao.USUARIO.ID;
                    objRequest.Item.NM_USUARIO = UsuarioAutenticacao.USUARIO.NOME;

                    //if (solicitacao.IMG != null)
                    //{
                    //    var img_file = new DTO.File();

                    //    var memStream = new MemoryStream();
                    //    solicitacao.IMG.InputStream.CopyTo(memStream);

                    //    img_file.dataArray = memStream.ToArray();
                    //    img_file.Extension = System.IO.Path.GetExtension(solicitacao.IMG.FileName).ToLower();
                    //    img_file.Name = solicitacao.IMG.FileName.Replace(img_file.Extension, "");
                    //    img_file.System = 0;
                    //    objRequest.Item.IMG_file = img_file;
                    //}

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Solicitacao, DTO.Solicitacao>(objRequest, "Solicitacao", "Post");
                    if (objResponseWebApiMetodo.Sucesso)
                    {
                        if (objResponseWebApiMetodo.TotalRows > 0 &&
                            objResponseWebApiMetodo.List != null)
                        {
                            objSolicitacao = objResponseWebApiMetodo.List[0];
                        }
                    }
                    else
                    {
                        throw new Exception(objResponseWebApiMetodo.Mensagem);
                    }

                    return Json(new { Result = "OK", Record = objSolicitacao, Message = objResponseWebApiMetodo.Mensagem });
                }
                else
                    return Json(new { Result = "ERROR", Message = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Put(DTO.Solicitacao solicitacao)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;
                DTO.Solicitacao objSolicitacao = new DTO.Solicitacao();

                string msgErro = "";
                if (true)
                {
                    objSolicitacao = solicitacao;
                    DTO.Request<DTO.Solicitacao> objRequest = new Request<Solicitacao>();
                    objRequest.Item = objSolicitacao;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Solicitacao, DTO.Solicitacao>(objRequest, "Solicitacao", "Put");
                    if (objResponseWebApiMetodo.Sucesso)
                    {
                        if (objResponseWebApiMetodo.TotalRows > 0 &&
                            objResponseWebApiMetodo.List != null)
                        {
                            objSolicitacao = objResponseWebApiMetodo.List[0];
                        }
                    }
                    else
                    {
                        throw new Exception(objResponseWebApiMetodo.Mensagem);
                    }

                    return Json(new { Result = "OK", Record = objSolicitacao, Message = objResponseWebApiMetodo.Mensagem });
                }
                else
                    return Json(new { Result = "ERROR", Message = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { Result = "ERROR", Message = "Id da Solicitação esperado. Valor não pode ser 0." });
                }
                else
                {
                    DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;

                    DTO.Request<DTO.Solicitacao> objRequest = new Request<Solicitacao>();
                    objRequest.Item = new DTO.Solicitacao();
                    objRequest.Item.ID = id;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Solicitacao, DTO.Solicitacao>(objRequest, "Solicitacao", "Delete");
                    if (!objResponseWebApiMetodo.Sucesso)
                    {
                        throw new Exception(objResponseWebApiMetodo.Mensagem);
                    }

                    return Json(new { Result = "OK", Message = objResponseWebApiMetodo.Mensagem });
                }
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> FileUpload(DTO.Solicitacao solicitacao)
        {
            try
            {
                int systemFile = 0;
                //int.TryParse(ConfigurationManager.AppSettings["WebApi:File:System"], out systemFile);

                DTO.Request<DTO.Solicitacao> request = new DTO.Request<DTO.Solicitacao>();
                request.Item = solicitacao;
                if (solicitacao.IMG != null)
                {
                    var img_file = new DTO.File();

                    var memStream = new MemoryStream();
                    solicitacao.IMG.InputStream.CopyTo(memStream);

                    img_file.dataArray = memStream.ToArray();
                    img_file.Extension = System.IO.Path.GetExtension(solicitacao.IMG.FileName).ToLower();
                    img_file.Name = solicitacao.IMG.FileName.Replace(img_file.Extension, "");
                    img_file.System = systemFile;
                    request.Item.IMG_file = img_file;
                }


                request.UsuarioAutenticacao = UsuarioAutenticacao;
               
                var model = await webApi.ExecuteAutoPostAsync<DTO.Solicitacao, DTO.Solicitacao>(request, "Solicitacao", "Post");

                var record = model.List.FirstOrDefault();

                return Json(new { Result = "OK", Message = model.Mensagem });

            }
            catch (Exception error)
            {

                return Json(new { ID = "", Result = false, Message = error.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        private bool ValidaDadosUsuario(ref string msgErro, string nome, string login, string email)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(nome) || nome.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O nome é obrigatório!!!";
                retorno = false;
            }

            if (string.IsNullOrEmpty(login) || login.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O login é obrigatório!!!";
                retorno = false;
            }

            if (!string.IsNullOrEmpty(email) && email.Trim() != "")
            {
                if (!email.Trim().Contains("@") || !email.Trim().Contains(".com"))
                {
                    if (msgErro != "")
                        msgErro += "<br>";
                    msgErro += "Informe um e-mail válido!!!";
                    retorno = false;
                }
            }

            return retorno;
        }


    }
}