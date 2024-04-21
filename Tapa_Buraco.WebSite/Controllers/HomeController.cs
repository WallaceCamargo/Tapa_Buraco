using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Tapa_Buraco.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(int id, string nome, string joint, string loa, string numeroViagem, string agencia, int jtStartIndex = 0, int jtPageSize = 25)
        {
            try
            {
                DTO.ResponseWebApi<DTO.Home> objResponseWebApiMetodo = null;
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("id", string.IsNullOrEmpty(id.ToString()) ? "" : id.ToString().Trim());
                parametros.Add("nome", string.IsNullOrEmpty(nome) ? "" : nome.Trim());
                parametros.Add("joint", string.IsNullOrEmpty(joint) ? "" : joint.Trim());
                parametros.Add("loa", string.IsNullOrEmpty(loa) ? "" : loa.Trim());
                parametros.Add("numeroViagem", string.IsNullOrEmpty(numeroViagem) ? "" : numeroViagem.Trim());
                parametros.Add("agencia", string.IsNullOrEmpty(agencia) ? "" : agencia.Trim());
                parametros.Add("startIndexPaging", jtStartIndex.ToString().Trim());
                parametros.Add("pageSizePaging", jtPageSize.ToString().Trim());

                List<DTO.Home> objListHome = new List<DTO.Home>();
                objResponseWebApiMetodo = await webApi.ExecuteCustomGetAsync<DTO.Home>("Home", "GetAll", parametros);
                if (objResponseWebApiMetodo.Sucesso)
                {
                    if (objResponseWebApiMetodo.TotalRows > 0 && objResponseWebApiMetodo.List != null)
                    {
                        objListHome = objResponseWebApiMetodo.List;
                    }
                }
                else
                {
                    throw new Exception(objResponseWebApiMetodo.Mensagem);
                }

                return Json(new { Result = "OK", Records = objListHome, TotalRecordCount = ((objListHome == null || objListHome.Count <= 0) ? 0 : (objListHome[0] == null ? 0 : objResponseWebApiMetodo.TotalRows)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}