using Tapa_Buraco.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tapa_Buraco.DTO
{
    public class Response<T> where T : class
    {
        public Response()
        {
            List = new List<T>();
        }

        public bool Sucesso { get; set; }
        public string ErroCodigo { get; set; }
        public string Mensagem { get; set; }
        public string Protocolo { get; set; }
        public int TotalRows { get; set; }
        public List<T> List { get; set; }

        public override string ToString()
        {
            var ret = " - ";

            try
            {
                string list = "";
                if (List == null)
                    list = "null";
                else if (List.Count() == 0)
                    list = "0 itens";
                else
                    list = List.Count() + " itens of type *" + List[0].GetType().ToString() + "*";

                ret = "Response [Sucesso: '" + Sucesso.ToString() + "', " +
                    "ErroCodigo: '" + ErroCodigo + "', " +
                    "Mensagem: '" + Mensagem + "', " +
                    "Protocolo: '" + Protocolo + "', " +
                    "TotalRows: '" + TotalRows + "', " +
                    "List: '" + list + "']";
            }
            catch (Exception er)
            {
                try
                {
                    #region Nlog
                    NLogHelper.logger.Info("**********************************************");
                    NLogHelper.logger.Info("ERRO Inesperado respToString()");
                    NLogHelper.logger.Info("**********************************************");
                    NLogHelper.logger.Info(er.ToString());
                    NLogHelper.logger.Info("**********************************************");
                    #endregion Nlog
                }
                catch { }
            }

            return ret;
        }
    }
}
