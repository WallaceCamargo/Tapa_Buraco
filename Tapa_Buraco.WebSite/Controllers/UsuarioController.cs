using Tapa_Buraco.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Tapa_Buraco.WebSite.Controllers
{
    public class UsuarioController : BaseController
    {        
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _ModalExluir()
        {
            return PartialView();
        }

        [Route("Usuario/Cadastro/{id}")]
        public ActionResult Cadastro(int? id = 0)
        {
            ViewData["id"] = id == null ? 0 : Convert.ToInt32(id);

            return View();
        }

        public PartialViewResult _ModalBuscaUsuarioAD()
        {
            return PartialView();
        }

        public async Task<JsonResult> GetAll(string nome, int jtStartIndex = 0, int jtPageSize = 20)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("nome", string.IsNullOrEmpty(nome) ? "" : nome.Trim());
                parametros.Add("startIndexPaging", jtStartIndex.ToString().Trim());
                parametros.Add("pageSizePaging", jtPageSize.ToString().Trim());

                List<DTO.Usuario> objListUsuario = new List<DTO.Usuario>();
                objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Usuario>("Usuario", "GetAll", parametros);
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

                return Json(new { Result = "OK", Records = objListUsuario, TotalRecordCount = ((objListUsuario == null || objListUsuario.Count <= 0) ? 0 : (objListUsuario[0] == null ? 0 : objResponseWebApiMetodo.TotalRows)) });
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
                DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("id", id.ToString().Trim());

                List<DTO.Usuario> objListUsuario = new List<DTO.Usuario>();
                objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Usuario>("Usuario", "GetById", parametros);
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
        public async Task<JsonResult> Post(DTO.Usuario usuario)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;
                DTO.Usuario objUsuario = new DTO.Usuario();

                string msgErro = "";
                if (ValidaDadosUsuario(ref msgErro, usuario.NOME, usuario.LOGIN, usuario.EMAIL))
                {                  
                    objUsuario = usuario;
                    DTO.Request<DTO.Usuario> objRequest = new Request<Usuario>();
                    objRequest.Item = objUsuario;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Usuario, DTO.Usuario>(objRequest,"Usuario", "Post");
                    if (objResponseWebApiMetodo.Sucesso)
                    {
                        if (objResponseWebApiMetodo.TotalRows > 0 &&
                            objResponseWebApiMetodo.List != null)
                        {
                            objUsuario = objResponseWebApiMetodo.List[0];
                        }
                    }
                    else
                    {
                        throw new Exception(objResponseWebApiMetodo.Mensagem);
                    }

                    return Json(new { Result = "OK", Record = objUsuario, Message = objResponseWebApiMetodo.Mensagem });
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
        public async Task<JsonResult> Put(DTO.Usuario usuario)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;
                DTO.Usuario objUsuario = new DTO.Usuario();

                string msgErro = "";
                if (ValidaDadosUsuario(ref msgErro, usuario.NOME, usuario.LOGIN, usuario.EMAIL))
                {
                    objUsuario = usuario;
                    DTO.Request<DTO.Usuario> objRequest = new Request<Usuario>();
                    objRequest.Item = objUsuario;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Usuario, DTO.Usuario>(objRequest, "Usuario", "Put");
                    if (objResponseWebApiMetodo.Sucesso)
                    {
                        if (objResponseWebApiMetodo.TotalRows > 0 &&
                            objResponseWebApiMetodo.List != null)
                        {
                            objUsuario = objResponseWebApiMetodo.List[0];
                        }
                    }
                    else
                    {
                        throw new Exception(objResponseWebApiMetodo.Mensagem);
                    }

                    return Json(new { Result = "OK", Record = objUsuario, Message = objResponseWebApiMetodo.Mensagem });
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
                    return Json(new { Result = "ERROR", Message = "Id do Usuário esperado. Valor não pode ser 0." });
                }
                else
                {
                    DTO.ResponseWebApi<DTO.Usuario> objResponseWebApiMetodo = null;

                    DTO.Request<DTO.Usuario> objRequest = new Request<Usuario>();
                    objRequest.Item = new DTO.Usuario();
                    objRequest.Item.ID = id;
                    objRequest.UsuarioAutenticacao = UsuarioAutenticacao;

                    objResponseWebApiMetodo = await webApi.ExecuteAutoPostAsync<DTO.Usuario, DTO.Usuario>(objRequest, "Usuario", "Delete");
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