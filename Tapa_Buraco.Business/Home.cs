using Tapa_Buraco.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Business
{
    public class Home
    {
        public async Task<DTO.Response<DTO.Home>> GetAll(int id, string nome = null, string joint = null, string loa = null, string numeroViagem = null, string agencia = null, int startIndexPaging = 0, int pageSizePaging = 25)
        {
            string msgErro = "";
            DTO.Response<DTO.Home> objResponse = new DTO.Response<DTO.Home>();
            try
            {
                List<DTO.Home> objListHome = await new DB.Home().GetAll(id, nome, joint, loa, numeroViagem, agencia);

                objResponse.Sucesso = true;
                objResponse.TotalRows = objListHome == null ? 0 : objListHome.Count;
                objResponse.Protocolo = null;

                objListHome = objListHome.OrderBy(f => f.NOME).ToList().Skip(startIndexPaging).Take(pageSizePaging).ToList();
                objResponse.List = objListHome;
            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível recuperar usuários da base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

    }
}
