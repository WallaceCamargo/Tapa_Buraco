using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Tapa_Buraco.WebApi.Controllers
{
    [RoutePrefix("api/v1/Login")]
    public class LoginController : ApiController
    {
        [AuthorizeAttributeOverride()]
        [HttpGet] [Route("DoLogin")]
        public async Task<HttpResponseMessage> DoLogin()
        {
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();

            try
            {
                objResponse.Sucesso = true;
                objResponse.Mensagem = "Logou";
                return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                
            }
            catch (Exception ex)
            {
                objResponse.Sucesso = false;
                objResponse.Mensagem = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, objResponse);
            }
        }


    }
}
