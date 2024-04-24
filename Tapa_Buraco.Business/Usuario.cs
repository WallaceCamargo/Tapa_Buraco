using Tapa_Buraco.DTO;
using Tapa_Buraco.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Business
{
    public class Usuario
    {
        public async Task<DTO.Response<DTO.UsuarioAutenticacao>> Autenticacao(string login, string senha)
        {
            string msgErro = "";
            DTO.Response<DTO.UsuarioAutenticacao> objResponse = new DTO.Response<DTO.UsuarioAutenticacao>();
            objResponse.List = new List<DTO.UsuarioAutenticacao>();

            try
            {
                var hash = Helper.Encrypt(senha);
                DTO.Usuario objUsuario = await new DB.Usuario().GetByLogin(login, hash);
                if (objUsuario == null)
                {
                    msgErro = "ERRO. Login não encontrado.";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                }
                else
                {
                    ////validar AD - usuário interno da rede
                    //if (!ADHelper.ValidarUsuarioESenha(login, senha))
                    //{
                    //    msgErro = "ERRO. Senha inválida.";
                    //    objResponse.Sucesso = false;
                    //    objResponse.Mensagem = msgErro;
                    //    objResponse.TotalRows = 0;
                    //    objResponse.List = null;
                    //}
                    //else
                    //{
                        var usuarioAutenticacao = new DTO.UsuarioAutenticacao();
                        usuarioAutenticacao.USUARIO = objUsuario;                   

                        msgErro = "Senha válida.";
                        objResponse.Sucesso = true;
                        objResponse.Mensagem = msgErro;
                        objResponse.TotalRows = 1;
                        objResponse.List.Add(usuarioAutenticacao);
                        objResponse.Protocolo = Convert.ToString(objUsuario.ID); ;
                    //}
                }
            }
            catch (Exception ex)
            {
                #region Nlog                
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível realizar autenticação. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        public async Task<DTO.Response<DTO.Usuario>> GetAll(string nome = null, int startIndexPaging = 0, int pageSizePaging = 15)
        {
            string msgErro = "";
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();
            try
            {
                List<DTO.Usuario> objListUsuario = await new DB.Usuario().GetAll(nome);

                objResponse.Sucesso = true;
                objResponse.TotalRows = objListUsuario == null ? 0 : objListUsuario.Count;
                objResponse.Protocolo = null;

                objListUsuario = objListUsuario.OrderBy(f =>f.NOME).ToList().Skip(startIndexPaging).Take(pageSizePaging).ToList();
                objResponse.List = objListUsuario;
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

        public async Task<DTO.Response<DTO.Usuario>> GetById(int id)
        {
            string msgErro = "";
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();
            try
            {
                DTO.Usuario objListUsuario = await new DB.Usuario().GetById(id);
                List<DTO.Usuario> usuarios = new List<DTO.Usuario>();
                if (objListUsuario != null) { usuarios.Add(objListUsuario); }

                objResponse.Sucesso = true;
                objResponse.TotalRows = usuarios == null ? 0 : usuarios.Count;
                objResponse.Protocolo = null;
                objResponse.List = usuarios;
            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível recuperar usuário da base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        public async Task<DTO.Response<DTO.Usuario>> Save(DTO.Request<DTO.Usuario> objUsuario)
        {
            string msgErro = "";
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();
            try
            {
                #region VALIDAÇÕES
                if (objUsuario.Item == null)
                {
                    msgErro = "Não foram enviadas informações para a inclusão!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                string msgErroValidacaoCampos = "";
                if (!ValidaDadosUsuario(ref msgErroValidacaoCampos, objUsuario.Item))
                {
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErroValidacaoCampos;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                if (await new DB.Usuario().VerificaSeJaExisteUsuarioPorLogin(objUsuario.Item.LOGIN) == true)
                {
                    msgErro = "Login já utilizado por outro usuário!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                //var usuarios = ADHelper.ConsultarUsuarioPorLogin(objUsuario.Item.LOGIN);
                //if (usuarios.Count() == 0)
                //{
                //    msgErro = "Usuário não encontrado no AD!";
                //    objResponse.Sucesso = false;
                //    objResponse.Mensagem = msgErro;
                //    objResponse.TotalRows = 0;
                //    objResponse.List = null;
                //    return objResponse;
                //}
                #endregion
                objUsuario.Item.SENHA = Helper.Encrypt(objUsuario.Item.SENHA);
                objUsuario.Item = await new DB.Usuario().Save(objUsuario.Item);
                if (objUsuario.Item == null || objUsuario.Item.ID <= 0)
                {
                    msgErro = "ERRO ao tentar salvar dados do Usuário!";
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
                    objResponse.Protocolo = Convert.ToString(objUsuario.Item.ID);
                    objResponse.List = new List<DTO.Usuario> { objUsuario.Item };
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

        public async Task<DTO.Response<DTO.Usuario>> Update(DTO.Request<DTO.Usuario> objUsuario)
        {
            string msgErro = "";
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();
            try
            {
                #region VALIDAÇÕES
                if (objUsuario.Item == null)
                {
                    msgErro = "Não foram enviadas informações para a alteração!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                string msgErroValidacaoCampos = "";
                if (!ValidaDadosUsuario(ref msgErroValidacaoCampos, objUsuario.Item))
                {
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErroValidacaoCampos;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                //var usuarios = ADHelper.ConsultarUsuarioPorLogin(objUsuario.Item.LOGIN);
                //if (usuarios.Count() == 0)
                //{
                //    msgErro = "Usuário não encontrado no AD!";
                //    objResponse.Sucesso = false;
                //    objResponse.Mensagem = msgErro;
                //    objResponse.TotalRows = 0;
                //    objResponse.List = null;
                //    return objResponse;
                //}

                DTO.Usuario usuario = await new DB.Usuario().GetById(objUsuario.Item.ID);
                if (usuario == null)
                {
                    msgErro = "Usuário não encontrado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }
                #endregion

                var usuarioAlterado = await new DB.Usuario().Update(objUsuario.Item);

                #region LOG ATIVIDADE
                var logs = new List<DB.LogAtividade>();

                var tupleList = new List<Tuple<string, string, string>>
                {
                    Tuple.Create("NOME", usuarioAlterado.NOME, usuario.NOME),
                    Tuple.Create("LOGIN", usuarioAlterado.LOGIN, usuario.LOGIN),
                    Tuple.Create("EMAIL", usuarioAlterado.EMAIL, usuario.EMAIL),
                    Tuple.Create("ATIVO", usuarioAlterado.ATIVO.ToString(), usuario.ATIVO.ToString()),
                    Tuple.Create("ADMIN", usuarioAlterado.ADMIN.ToString(), usuario.ADMIN.ToString()),
                };

                foreach (var item in tupleList)
                {
                    if (item.Item2 != item.Item3)
                    {
                        var log = new DB.LogAtividade(objUsuario.Item.ID.ToString(), "TB_USUARIO", item.Item1, item.Item2, objUsuario.UsuarioAutenticacao, "U", item.Item3);
                        logs.Add(log);
                    }
                }
                new DB.LogAtividade().AddLog(logs);

                #endregion

                objResponse.Sucesso = true;
                objResponse.TotalRows = 1;
                objResponse.Protocolo = Convert.ToString(objUsuario.Item.ID);
                objResponse.List = new List<DTO.Usuario> { objUsuario.Item };
                

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

        public async Task<DTO.Response<DTO.Usuario>> Delete(DTO.Request<DTO.Usuario> objUsuario)
        {
            string msgErro = "";
            bool excluiuComSucesso = false;
            DTO.Response<DTO.Usuario> objResponse = new DTO.Response<DTO.Usuario>();
            try
            {
                #region VALIDAÇÕES

                if (objUsuario.Item == null)
                {
                    msgErro = "Não foram enviadas informações para exclusão!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;

                }

                if (objUsuario.Item.ID <= 0)
                {
                    msgErro = "Id do usuário para a exclusão não foi informado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }

                DTO.Usuario usuario = await new DB.Usuario().GetById(objUsuario.Item.ID);
                if (usuario == null)
                {
                    msgErro = "Usuário não encontrado!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                    return objResponse;
                }
                #endregion

                excluiuComSucesso = await new DB.Usuario().Delete(objUsuario.Item.ID);
                if (!excluiuComSucesso)
                {
                    msgErro = "ERRO ao tentar salvar dados do Usuário!";
                    objResponse.Sucesso = false;
                    objResponse.Mensagem = msgErro;
                    objResponse.TotalRows = 0;
                    objResponse.List = null;
                }
                else
                {
                    #region LOG ATIVIDADE
                    var logs = new List<DB.LogAtividade>();

                    var log = new DB.LogAtividade(objUsuario.Item.ID.ToString(), "TB_USUARIO", "DT_DELETE", DateTime.Now.ToString(), objUsuario.UsuarioAutenticacao, "D");
                    logs.Add(log);
                    
                    new DB.LogAtividade().AddLog(logs);

                    #endregion

                    objResponse.Sucesso = true;
                    objResponse.TotalRows = 1;
                    objResponse.Protocolo = Convert.ToString(objUsuario.Item.ID);
                    objResponse.List = new List<DTO.Usuario> { objUsuario.Item };
                }

            }
            catch (Exception ex)
            {
                #region Nlog
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                #endregion Nlog
                msgErro = "ERRO. Impossível excluir dados do Usuário na base. Msg > '" + Util.Util.GetAllExceptionsMessages(ex) + "'";
                objResponse.Sucesso = false;
                objResponse.Mensagem = msgErro;
                objResponse.List = null;
            }

            return objResponse;
        }

        private bool ValidaDadosUsuario(ref string msgErro, DTO.Usuario usuario)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(usuario.NOME) || usuario.NOME.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O nome é obrigatório!!!";
                retorno = false;
            }

            if (string.IsNullOrEmpty(usuario.LOGIN) || usuario.LOGIN.Trim() == "")
            {
                if (msgErro != "")
                    msgErro += "<br>";
                msgErro += "O login é obrigatório!!!";
                retorno = false;
            }

            if (!string.IsNullOrEmpty(usuario.EMAIL) && usuario.EMAIL.Trim() != "")
            {
                if (!usuario.EMAIL.Trim().Contains("@") || !usuario.EMAIL.Trim().Contains(".com"))
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
