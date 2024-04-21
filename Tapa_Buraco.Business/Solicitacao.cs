using Tapa_Buraco.DTO;
using Tapa_Buraco.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Business
{
    public class Solicitacao
    {
        
        public async Task<DTO.Response<DTO.Solicitacao>> GetAll(string id_usuario = null, int startIndexPaging = 0, int pageSizePaging = 15)
        {
            string msgErro = "";
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            try
            {
                List<DTO.Solicitacao> objListSolicitacao = await new DB.Solicitacao().GetAll(id_usuario);

                objResponse.Sucesso = true;
                objResponse.TotalRows = objListSolicitacao == null ? 0 : objListSolicitacao.Count;
                objResponse.Protocolo = null;

                objListSolicitacao = objListSolicitacao.OrderBy(f =>f.DT_REGISTRO).ToList().Skip(startIndexPaging).Take(pageSizePaging).ToList();
                objResponse.List = objListSolicitacao;
            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível recuperar solicitações da base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        //corrigir depois --- codigo desnecesario ----
        public async Task<DTO.Response<DTO.Solicitacao>> GetById(int id)
        {
            string msgErro = "";
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            try
            {
                DTO.Solicitacao objListSolicitacao = await new DB.Solicitacao().GetById(id);
                List<DTO.Solicitacao> solicitacao = new List<DTO.Solicitacao>();
                if (objListSolicitacao != null) { solicitacao.Add(objListSolicitacao); }

                objResponse.Sucesso = true;
                objResponse.TotalRows = solicitacao == null ? 0 : solicitacao.Count;
                objResponse.Protocolo = null;
                objResponse.List = solicitacao;
            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível recuperar solicitação da base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        public async Task<DTO.Response<DTO.Solicitacao>> Save(DTO.Request<DTO.Solicitacao> objSolicitacao)
        {
            string msgErro = "";
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            try
            {
                #region VALIDAÇÕES
                if (objSolicitacao.Item == null)
                {
                    msgErro = "Não foram enviadas informações para a inclusão!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }
                #endregion
                //byte[] teste = Convert.FromBase64String(objSolicitacao.Item.IMG_URL);
                //Bitmap bitmap = new Bitmap(objSolicitacao.Item.IMG_URL);
                //using (MemoryStream ms = new MemoryStream())
                //{
                    
                //    IFormFile formFile = null;
                //    await formFile.CopyToAsync(ms);                   
                //    objSolicitacao.Item.IMG = ms.ToArray();
                //}
                    

                objSolicitacao.Item = await new DB.Solicitacao().Save(objSolicitacao.Item);
                if (objSolicitacao.Item == null || objSolicitacao.Item.ID <= 0)
                {
                    msgErro = "ERRO ao tentar salvar dados da Solicitacao!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                }
                else
                {
                    //#region LOG ATIVIDADE
                    //var logs = new List<DB.LogAtividade>();
                    //var dir = new Dictionary<string, string>();

                    //dir.Add("ID", objUsuario.Item.ID.ToString());
                    //dir.Add("NOME", objUsuario.Item.NOME);
                    //dir.Add("LOGIN", objUsuario.Item.LOGIN);
                    //dir.Add("EMAIL", objUsuario.Item.EMAIL);
                    //dir.Add("ATIVO", objUsuario.Item.ATIVO.ToString());
                    //dir.Add("ADMIN", objUsuario.Item.ADMIN.ToString());

                    //foreach (var item in dir)
                    //{
                    //    var log = new DB.LogAtividade(objUsuario.Item.ID.ToString(), "TB_USUARIO", item.Key, item.Value, objUsuario.UsuarioAutenticacao, "I");
                    //    logs.Add(log);
                    //}
                    //new DB.LogAtividade().AddLog(logs);

                    //#endregion

                    objResponse.Sucesso = true;
                    objResponse.TotalRows = 1;
                    objResponse.Protocolo = Convert.ToString(objSolicitacao.Item.ID);
                    objResponse.List = new List<DTO.Solicitacao> { objSolicitacao.Item };
                }

            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível salvar dados do Usuário na base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        public async Task<DTO.Response<DTO.Solicitacao>> Update(DTO.Request<DTO.Solicitacao> objSolicitacao)
        {
            string msgErro = "";
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            try
            {
                #region VALIDAÇÕES
                if (objSolicitacao.Item == null)
                {
                    msgErro = "Não foram enviadas informações para a alteração!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                string msgErroValidacaoCampos = "";
                //if (!ValidaDadosUsuario(ref msgErroValidacaoCampos, objSolicitacao.Item))
                //{
                //    objResponse.Sucesso = false;
                //    objResponse.Mensagem = msgErroValidacaoCampos;
                //    objResponse.TotalRows = 0;
                //    objResponse.List = null;
                //    return objResponse;
                //}

                DTO.Solicitacao solicitacao = await new DB.Solicitacao().GetById(objSolicitacao.Item.ID);
                if (solicitacao == null)
                {
                    msgErro = "Usuário não encontrado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }
                #endregion

                var solicitacaoAlterado = await new DB.Solicitacao().Update(objSolicitacao.Item);

                #region LOG ATIVIDADE
                //var logs = new List<DB.LogAtividade>();

                //var tupleList = new List<Tuple<string, string, string>>
                //{
                //    Tuple.Create("DT PRAZO", solicitacaoAlterado.DT_PRAZO_STRING, objSolicitacao.Item.DT_PRAZO_STRING),
                //    Tuple.Create("PRIORIDADE", solicitacaoAlterado.PRIORIDADE, objSolicitacao.Item.PRIORIDADE),
                //};

                //foreach (var item in tupleList)
                //{
                //    if (item.Item2 != item.Item3)
                //    {
                //        var log = new DB.LogAtividade(objSolicitacao.Item.ID.ToString(), "TB_SOLICITACAO", item.Item1, item.Item2, objSolicitacao.UsuarioAutenticacao, "U", item.Item3);
                //        logs.Add(log);
                //    }
                //}
                //new DB.LogAtividade().AddLog(logs);

                #endregion

                objResponse.Sucesso = true;
                objResponse.TotalRows = 1;
                objResponse.Protocolo = Convert.ToString(objSolicitacao.Item.ID);
                objResponse.List = new List<DTO.Solicitacao> { objSolicitacao.Item };
                

            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível atualizar dados do Usuário na base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        public async Task<DTO.Response<DTO.Solicitacao>> Delete(DTO.Request<DTO.Solicitacao> objSolicitacao)
        {
            string msgErro = "";
            bool excluiuComSucesso = false;
            DTO.Response<DTO.Solicitacao> objResponse = new DTO.Response<DTO.Solicitacao>();
            try
            {
                #region VALIDAÇÕES

                if (objSolicitacao.Item == null)
                {
                    msgErro = "Não foram enviadas informações para exclusão!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;

                }

                if (objSolicitacao.Item.ID <= 0)
                {
                    msgErro = "Id da Solicitacão para a exclusão não foi informado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                DTO.Solicitacao Solicitacao = await new DB.Solicitacao().GetById(objSolicitacao.Item.ID);
                if (Solicitacao == null)
                {
                    msgErro = "Solicitacão não encontrado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }
                #endregion

                excluiuComSucesso = await new DB.Solicitacao().Delete(objSolicitacao.Item.ID);
                if (!excluiuComSucesso)
                {
                    msgErro = "ERRO ao tentar excluir Solicitação!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                }
                else
                {
                    //#region LOG ATIVIDADE
                    //var logs = new List<DB.LogAtividade>();

                    //var log = new DB.LogAtividade(objSolicitacao.Item.ID.ToString(), "TB_USUARIO", "DT_DELETE", DateTime.Now.ToString(), objSolicitacao.UsuarioAutenticacao, "D");
                    //logs.Add(log);
                    
                    //new DB.LogAtividade().AddLog(logs);

                    //#endregion

                    objResponse.Sucesso = true;
                    objResponse.TotalRows = 1;
                    objResponse.Protocolo = Convert.ToString(objSolicitacao.Item.ID);
                    objResponse.List = new List<DTO.Solicitacao> { objSolicitacao.Item };
                }

            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível excluir dados da Solicitacao na base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        private bool ValidaDadosUsuario(ref string msgErro, DTO.Solicitacao solicitacao)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(solicitacao.CEP) || solicitacao.CEP.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O CEP é obrigatório!!!";
                retorno = false;
            }

            if (string.IsNullOrEmpty(solicitacao.PONTO_REFERENCIA) || solicitacao.PONTO_REFERENCIA.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O PONTO DE REFERENCIA é obrigatório!!!";
                retorno = false;
            }

            if (!string.IsNullOrEmpty(solicitacao.PRIORIDADE) && solicitacao.PRIORIDADE.Trim() != "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "Informe um PRIORIDADE válido!!!";
                retorno = false;
                
            }


            return retorno;
        }


    }

}
