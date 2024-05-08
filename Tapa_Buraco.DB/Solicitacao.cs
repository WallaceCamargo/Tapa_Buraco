using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DB
{
    public class Solicitacao
    {
        public async Task<int> GetNewIdSequence()
        {   
            int newId = 0;
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    SQL.Append("SELECT nvl(CAST(SEQ_SOLICITACAO.nextval as int), 0) NEW_ID from dual");
                    var resultId = await db.Connection.QueryAsync<int>(SQL.ToString());
                    int? new_autonum = resultId.FirstOrDefault();
                    if (new_autonum != null && new_autonum > 0)
                    {//pegou novo id sequence com sucesso
                        newId = Convert.ToInt32(new_autonum);
                    }
                    else
                        throw new Exception("Problema com a SEQUENCE 'SEQ_SOLICITACAO' !");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return newId;
        }

        public async Task<DTO.Solicitacao> GetById(int id)
        {
            DTO.Solicitacao usuario = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.Append("SELECT * FROM TB_SOLICITACAO WHERE ID = :ID AND DT_DELETE IS NULL");

                    var result = await db.Connection.QueryAsync<DTO.Solicitacao>(SQL.ToString(),
                                    new
                                    {
                                        ID = id
                                    });
                    usuario = result.FirstOrDefault();
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return usuario;
        }

        public async Task<List<DTO.Solicitacao>> GetAll(string nome, string status, string dt_inicio, string dt_fim, string id_usuario, string perfil_usuario)
        {
            List<DTO.Solicitacao> usuarios = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.AppendLine(" SELECT A.* FROM TB_SOLICITACAO A ");
                    SQL.AppendLine(" WHERE ((:NM_USUARIO is null)or(NM_USUARIO LIKE '%'||:NM_USUARIO||'%')) ");
                    SQL.AppendLine(" AND ((:STATUS is null)or(STATUS = :STATUS)) ");
                    SQL.AppendLine(" AND ((:DT_INICIO is null AND :DT_FIM is null)or(TRUNC(DT_REGISTRO) BETWEEN :DT_INICIO AND :DT_FIM)) ");
                    SQL.AppendLine(" AND A.DT_DELETE IS NULL ");
                    if (perfil_usuario == "0") {
                        SQL.AppendLine(" AND ID_USUARIO = :ID_USUARIO ");
                    }
                    var result = await db.Connection.QueryAsync<DTO.Solicitacao>(SQL.ToString(),
                                    new
                                    {
                                        NM_USUARIO = string.IsNullOrEmpty(nome) ? "" : nome,
                                        ID_USUARIO = string.IsNullOrEmpty(id_usuario) ? "" : id_usuario,
                                        STATUS = string.IsNullOrEmpty(status) ? "" : status,
                                        DT_INICIO = string.IsNullOrEmpty(dt_inicio) ? "" : dt_inicio,
                                        DT_FIM = string.IsNullOrEmpty(dt_fim) ? "" : dt_fim,
                                    });
                    usuarios = result.ToList();
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return usuarios;
        }


        public async Task<DTO.Solicitacao> Save(DTO.Solicitacao objSolicitacao)
        {
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region QUERY                                    
                    SQL.Append("INSERT INTO TB_SOLICITACAO ");
                    SQL.Append("  ( ");
                    SQL.Append(" ID, ");
                    SQL.Append(" CEP, ");
                    SQL.Append(" ID_USUARIO, ");
                    SQL.Append(" NM_USUARIO, ");
                    SQL.Append(" STATUS, ");
                    SQL.Append(" LOGRADOURO, ");
                    SQL.Append(" PONTO_REFERENCIA, ");
                    SQL.Append(" PRIORIDADE, ");
                    SQL.Append(" DT_PRAZO, ");
                    SQL.Append(" NOME_FOTO ");
                    SQL.Append("  ) ");
                    SQL.Append("  VALUES ");
                    SQL.Append("  ( ");
                    SQL.Append("  :ID, ");
                    SQL.Append("  :CEP, ");
                    SQL.Append("  :ID_USUARIO, ");
                    SQL.Append("  :NM_USUARIO, ");
                    SQL.Append("  :STATUS, ");
                    SQL.Append("  :LOGRADOURO, ");
                    SQL.Append("  :PONTO_REFERENCIA, ");
                    SQL.Append("  :PRIORIDADE, ");
                    SQL.Append("  :DT_PRAZO, ");
                    SQL.Append("  :NOME_FOTO ");
                    SQL.Append("  )");
                    #endregion QUERY 
                    objSolicitacao.ID = await GetNewIdSequence();
                    int result = await db.Connection.ExecuteAsync(SQL.ToString(), objSolicitacao);
                    if (result > 0)
                    {
                        objSolicitacao = await GetById(objSolicitacao.ID);
                    }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return objSolicitacao;
        }

        public async Task<DTO.Solicitacao> Update(DTO.Solicitacao objSolicitacao)
        {
            DTO.Solicitacao slcAlterado = new DTO.Solicitacao();
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region QUERY                                    
                    SQL.Append("UPDATE TB_SOLICITACAO SET ");
                    SQL.Append("    PRIORIDADE = :PRIORIDADE, ");
                    SQL.Append("    DT_PRAZO = :DT_PRAZO, ");
                    SQL.Append("    DT_ACATAMENTO = :DT_ACATAMENTO, ");
                    SQL.Append("    DT_FISCALIZACAO = :DT_FISCALIZACAO, ");
                    SQL.Append("    DT_AGENDAMENTO = :DT_AGENDAMENTO, ");
                    SQL.Append("    DT_ATENDIMENTO = :DT_ATENDIMENTO, ");
                    SQL.Append("    DT_FINALIZACAO = :DT_FINALIZACAO ");
                    SQL.Append("WHERE ID = :ID AND DT_DELETE IS NULL ");

                    #endregion QUERY 
                    int result = await db.Connection.ExecuteAsync(SQL.ToString(), objSolicitacao);
                    if (result > 0)
                    {
                        slcAlterado = await GetById(objSolicitacao.ID);
                    }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return slcAlterado;
        }

        public async Task<bool> Delete(int id)
        {
            bool retorno = false;
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region QUERY                                    
                    SQL.Append("UPDATE TB_SOLICITACAO SET DT_DELETE = SYSDATE WHERE ID = :ID AND DT_DELETE IS NULL ");
                    #endregion QUERY
                    int result = await db.Connection.ExecuteAsync(SQL.ToString(), new { ID = id });
                    if (result > 0) { retorno = true; }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return retorno;
        }

    }
}
