using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DB
{
    public class Usuario
    {
        public async Task<int> GetNewIdSequence()
        {   
            int newId = 0;
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    SQL.Append("SELECT nvl(CAST(SEQ_TB_USUARIO.nextval as int), 0) NEW_ID from dual");
                    var resultId = await db.Connection.QueryAsync<int>(SQL.ToString());
                    int? new_autonum = resultId.FirstOrDefault();
                    if (new_autonum != null && new_autonum > 0)
                    {//pegou novo id sequence com sucesso
                        newId = Convert.ToInt32(new_autonum);
                    }
                    else
                        throw new Exception("Problema com a SEQUENCE 'SEQ_TB_USUARIO' !");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return newId;
        }

        public async Task<DTO.Usuario> GetById(int id)
        {
            DTO.Usuario usuario = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.Append("SELECT * FROM TB_USUARIO WHERE ID = :ID AND DT_DELETE IS NULL");

                    var result = await db.Connection.QueryAsync<DTO.Usuario>(SQL.ToString(),
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

        public async Task<DTO.Usuario> GetByLogin(string login, string senha)
        {
            DTO.Usuario usuario = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.Append("SELECT * FROM TB_USUARIO USR WHERE LOWER(USR.LOGIN) = LOWER(:login) AND USR.SENHA = :SENHA AND DT_DELETE IS NULL");

                    var result = await db.Connection.QueryAsync<DTO.Usuario>(SQL.ToString(),
                                    new
                                    {
                                        Login = string.IsNullOrEmpty(login) ? "" : login,
                                        SENHA = string.IsNullOrEmpty(senha) ? "" : senha
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

        public async Task<List<DTO.Usuario>> GetAll(string nome)
        {
            List<DTO.Usuario> usuarios = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.AppendLine("SELECT A.* FROM TB_USUARIO A ");
                    SQL.AppendLine("WHERE ( UPPER(A.NOME) LIKE '%' || UPPER(:NOME) || '%' OR:NOME IS NULL )");
                    SQL.AppendLine("AND A.DT_DELETE IS NULL");

                    var result = await db.Connection.QueryAsync<DTO.Usuario>(SQL.ToString(),
                                    new
                                    {
                                        NOME = string.IsNullOrEmpty(nome) ? "" : nome
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

        public async Task<bool> VerificaSeJaExisteUsuarioPorLogin(string login)
        {
            bool ret = true;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region Query
                    SQL.Append("SELECT COUNT(1) AS QTD ");
                    SQL.Append("FROM TB_USUARIO ");                    
                    SQL.Append("WHERE LOWER(LOGIN) = LOWER(:Login)");
                    SQL.Append("AND DT_DELETE IS NULL");
                    #endregion Query
                    int result = await db.Connection.ExecuteScalarAsync<int>(SQL.ToString(),
                                    new
                                    {
                                        Login = login
                                    });
                    if (result <= 0)
                    {
                        ret = false;
                    }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return ret;
        }

        public async Task<DTO.Usuario> Save(DTO.Usuario objUsuario)
        {
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region QUERY                                    
                    SQL.Append("INSERT INTO TB_USUARIO ");
                    SQL.Append("  ( ");
                    SQL.Append("    ID , ");
                    SQL.Append("    NOME , ");
                    SQL.Append("    LOGIN , ");
                    SQL.Append("    SENHA , ");
                    SQL.Append("    EMAIL , ");
                    SQL.Append("    ATIVO, ");
                    SQL.Append("    ADMIN, ");
                    SQL.Append("    DT_INSERT ");
                    SQL.Append("  ) ");
                    SQL.Append("  VALUES ");
                    SQL.Append("  ( ");
                    SQL.Append("    :ID , ");
                    SQL.Append("    :NOME , ");
                    SQL.Append("    :LOGIN , ");
                    SQL.Append("    :SENHA , ");
                    SQL.Append("    :EMAIL , ");
                    SQL.Append("    :ATIVO, ");
                    SQL.Append("    :ADMIN, ");
                    SQL.Append("    SYSDATE ");
                    SQL.Append("  )");
                    #endregion QUERY 
                    objUsuario.ID = await GetNewIdSequence();
                    int result = await db.Connection.ExecuteAsync(SQL.ToString(), objUsuario);
                    if (result > 0)
                    {
                        objUsuario = await GetById(objUsuario.ID);
                    }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return objUsuario;
        }

        public async Task<DTO.Usuario> Update(DTO.Usuario objUsuario)
        {
            DTO.Usuario usuarioAlterado = new DTO.Usuario();
            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");
                    #region QUERY                                    
                    SQL.Append("UPDATE TB_USUARIO SET ");
                    SQL.Append("    NOME = :NOME, ");
                    SQL.Append("    LOGIN = :LOGIN, ");
                    SQL.Append("    EMAIL = :EMAIL, ");
                    SQL.Append("    ATIVO = :ATIVO, ");
                    SQL.Append("    ADMIN = :ADMIN ");
                    SQL.Append("WHERE ID = :ID AND DT_DELETE IS NULL ");

                    #endregion QUERY 
                    int result = await db.Connection.ExecuteAsync(SQL.ToString(), objUsuario);
                    if (result > 0)
                    {
                        usuarioAlterado = await GetById(objUsuario.ID);
                    }
                }
            }
            #region ERROS
            catch (Exception)
            {
                throw;
            }
            #endregion ERROS

            return usuarioAlterado;
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
                    SQL.Append("UPDATE TB_USUARIO SET DT_DELETE = SYSDATE WHERE ID = :ID AND DT_DELETE IS NULL ");
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
