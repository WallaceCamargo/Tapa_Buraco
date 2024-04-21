using Dapper;
using Tapa_Buraco.DTO;
using Tapa_Buraco.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DB
{
    public class LogAtividade
    {
        public LogAtividade()
        {
                
        }

        public LogAtividade(string chave, string tabela, string campo, string valor_new, DTO.UsuarioAutenticacao usuario, string acao, string valor_old = "")
        {
            popularCampo(chave, tabela, campo, valor_new, usuario, acao, valor_old);
        }

        public int AUTONUM { get; private set; }
        public string USUARIO { get; set; }
        public DateTime HORARIO { get; set; }
        public string TABELA { get; set; }
        public string CAMPO { get; set; }
        public string ACAO { get; set; }
        public string VALOR_OLD { get; set; }
        public string VALOR_NEW { get; set; }
        public string CHAVE { get; set; }
        public string PROGRAMA { get; set; }
        public string IP_USUARIO { get; set; }
        public string HOST_USUARIO { get; set; }

        private void popularCampo(string chave, string tabela, string campo, string valor_new, DTO.UsuarioAutenticacao usuario, string acao, string valor_old)
        {
            USUARIO = usuario == null ? "" : usuario.USUARIO.LOGIN;
            TABELA = tabela;
            CAMPO = campo;
            ACAO = acao;
            VALOR_OLD = valor_old;
            VALOR_NEW = valor_new;
            CHAVE = chave;
            PROGRAMA = "TAPA BURACO";
            IP_USUARIO = usuario == null ? "" : usuario.IpList;
            HOST_USUARIO = usuario == null ? "" : usuario.Host;
        }

        public void AddLog(List<LogAtividade> logsAtividade)
        {
            try
            {
                Save(logsAtividade);       
            }
            catch (Exception error)
            {
                NLogHelper.logger.Error(error);
                throw error;
            }
        }
        public async Task<int> GetNewIdSequence()
        {
            int newId = 0;
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    SQL.Append("SELECT nvl(CAST(SEQ_LOG_ATIVIDADE.nextval as int), 0) NEW_ID from dual");
                    var resultId = await db.Connection.QueryAsync<int>(SQL.ToString());
                    int? new_autonum = resultId.FirstOrDefault();
                    if (new_autonum != null && new_autonum > 0)
                    {//pegou novo id sequence com sucesso
                        newId = Convert.ToInt32(new_autonum);
                    }
                    else
                        throw new Exception("Problema com a SEQUENCE 'SEQ_LOG_ATIVIDADE' !");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return newId;
        }
            public async void Save(List<LogAtividade> logAtividades)
            {
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    using (System.Data.Common.DbTransaction trans = db.beginTrans())
                    {
                        try
                        {
                            bool salvoComSucesso = false;
                            foreach (var logAtividade in logAtividades)
                            {
                                StringBuilder SQL = new StringBuilder("");

                                SQL.Append("INSERT INTO LOG_ATIVIDADE ");
                                SQL.Append("    ( ");
                                SQL.Append("        AUTONUM, ");
                                SQL.Append("        USUARIO, ");
                                SQL.Append("        HORARIO, ");
                                SQL.Append("        TABELA, ");
                                SQL.Append("        ACAO, ");
                                SQL.Append("        CAMPO, ");
                                SQL.Append("        VALOR_OLD, ");
                                SQL.Append("        VALOR_NEW, ");
                                SQL.Append("        CHAVE, ");
                                SQL.Append("        PROGRAMA, ");
                                SQL.Append("        IP_USUARIO, ");
                                SQL.Append("        HOST_USUARIO ");
                                SQL.Append("    ) ");
                                SQL.Append("   VALUES ");
                                SQL.Append("   ( ");
                                SQL.Append("        :AUTONUM, ");
                                SQL.Append("        :USUARIO, ");
                                SQL.Append("        SYSDATE, ");
                                SQL.Append("        :TABELA, ");
                                SQL.Append("        :ACAO, ");
                                SQL.Append("        :CAMPO, ");
                                SQL.Append("        :VALOR_OLD, ");
                                SQL.Append("        :VALOR_NEW, ");
                                SQL.Append("        :CHAVE, ");
                                SQL.Append("        :PROGRAMA, ");
                                SQL.Append("        :IP_USUARIO, ");
                                SQL.Append("        :HOST_USUARIO ");
                                SQL.Append("    ) ");
                                logAtividade.AUTONUM = await GetNewIdSequence();
                                int result = trans.Connection.Execute(SQL.ToString(), logAtividade);
                                if (result > 0)
                                {
                                    salvoComSucesso = true;
                                    continue;
                                }
                                else
                                {
                                    NLogHelper.logger.Info("**********************************************");
                                    NLogHelper.logger.Error("Não salvou no banco mas não deu exceção");
                                    NLogHelper.logger.Error("Classe LogAtividade, Metodo Save");
                                    NLogHelper.logger.Error($"Obj: {logAtividade}");
                                    NLogHelper.logger.Info("**********************************************");
                                    salvoComSucesso = false;
                                    break;
                                }
                            }
                            
                            if (salvoComSucesso)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            NLogHelper.logger.Info("**********************************************");
                            NLogHelper.logger.Error("Não salvou no banco e deu exceção");
                            NLogHelper.logger.Error("Classe LogAtividade, Metodo Save");
                            NLogHelper.logger.Error($"Obj: {logAtividades}");
                            NLogHelper.logger.Error(ex, ex.ToString());
                            NLogHelper.logger.Info("**********************************************");
                            trans.Rollback();
                            throw new Exception("Transação não concluída !!! Rollback executado!");
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                throw ex;
            }
        }


        public LogAtividade GetByPropriedades(string usuario, DateTime horario, string tabela, string campo, string acao, string valor_old, string valor_new, string chave, string programa, string ip_usuario, string host_usuario)
        {
            LogAtividade logAtividadeRetorno = new LogAtividade();

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder();

                    SQL.AppendLine("SELECT * FROM LOG_ATIVIDADE ");
                    SQL.AppendLine("WHERE USUARIO = :USUARIO, TRUNC(HORARIO) = TRUNC(:HORARIO), TABELA = :TABELA, CAMPO = :CAMPO, ");
                    SQL.AppendLine("ACAO = :ACAO, VALOR_OLD = :VALOR_OLD, VALOR_NEW = :VALOR_NEW, CHAVE = :CHAVE ");
                    SQL.AppendLine("PROGRAMA = :PROGRAMA, IP_USUARIO = :IP_USUARIO, HOST_USUARIO = :HOST_USUARIO ");

                    var result = db.Connection.Query<LogAtividade>(SQL.ToString(), 
                        new 
                        {
                            USUARIO = usuario,
                            HORARIO = horario,
                            TABELA = tabela,
                            CAMPO = campo,
                            ACAO = acao,
                            VALOR_OLD = valor_old,
                            VALOR_NEW = valor_new,
                            CHAVE = chave,
                            PROGRAMA = programa,
                            IP_USUARIO = ip_usuario,
                            HOST_USUARIO = host_usuario,

                        });
                    logAtividadeRetorno = result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.logger.Info("**********************************************");
                NLogHelper.logger.Error("Classe LogAtividade, Metodo GetByPropriedades");
                NLogHelper.logger.Error($"usuario: {usuario}, horario: {horario}, tabela: {tabela}, campo: {campo}, acao: {acao}, valor_old: {valor_old}, valor_new: {valor_new}, chave: {chave}, programa: {programa}, ip_usuario: {ip_usuario}, host_usuario: {host_usuario}  ");
                NLogHelper.logger.Error(ex, ex.ToString());
                NLogHelper.logger.Info("**********************************************");
                throw new Exception("Erro ao consultar log atividade pelas propriedades");
            }

            return logAtividadeRetorno;
        }

    }
}
