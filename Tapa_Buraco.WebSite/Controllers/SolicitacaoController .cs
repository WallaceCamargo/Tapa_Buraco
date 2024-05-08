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
using System.Drawing;

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

        
        public async Task<JsonResult> GetAll(string nm_solicitante,string status,string dt_inicio, string dt_fim, int jtStartIndex = 0, int jtPageSize = 20)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Solicitacao> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("nm_solicitante", string.IsNullOrEmpty(nm_solicitante) ? "" : nm_solicitante.Trim());
                parametros.Add("status", string.IsNullOrEmpty(status) ? "" : status.Trim());
                parametros.Add("dt_inicio", string.IsNullOrEmpty(dt_inicio) ? "" : dt_inicio.Trim());
                parametros.Add("dt_fim", string.IsNullOrEmpty(dt_fim) ? "" : dt_fim.Trim());
                parametros.Add("id_usuario", UsuarioAutenticacao.USUARIO.ID.ToString().Trim());
                parametros.Add("perfil_usuario", UsuarioAutenticacao.USUARIO.ADMIN.ToString().Trim());
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
        public async Task<JsonResult> SalvarImagem(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                Random rnd = new Random();
                string extensao = Path.GetExtension(file.FileName);

                var fileName = Path.GetFileName(DateTime.Now.ToString("ddMMyyyy") + rnd.Next(100000,999999) + extensao);
                var folderPath = Server.MapPath("../font-awesome/img"); // Pasta onde a imagem será salva

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath); // Crie o diretório se não existir
                }

                var filePath = Path.Combine(folderPath, fileName);

                file.SaveAs(filePath); // Salva o arquivo


                return Json(new { Result = "OK", fileName = fileName, Message = "Sucesso" });
            }

            return Json(new { Result = "ERROR", Message = "erro" });
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