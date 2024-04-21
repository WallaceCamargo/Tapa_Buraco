using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DB
{
    internal class Dados : IDisposable
    {
        private DbConnection connection;
        private DbCommand cmd;
        private OracleConnection con;

        public string CommandText
        {
            get { return cmd.CommandText; }
            set { cmd.CommandText = value; }
        }

        public CommandType CommandType
        {
            get { return cmd.CommandType; }
            set { cmd.CommandType = value; }
        }

        public Dados(string nomeConexao)
        {
            try
             {
                if (nomeConexao == "SISTEMARAP")
                {
                     con = new OracleConnection(ConfigurationManager.AppSettings["Data Source"]);
                }
                else
                    throw new Exception("Conexão '" + nomeConexao + "' não definida.");

                if (con.State == ConnectionState.Closed)
                    con.Open();

                this.cmd = con.CreateCommand();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Propriedades
        /// <summary>
        /// Retorna a conexao ativa com o banco de dados
        /// </summary>
        /// <remarks>O tipo DbConnection é abstrato, o tipo concreto que será retornado depende do provider informado na connection string</remarks>
        ///
        public DbConnection Connection
        {
            get
            {
                return con;
            }
        }

        // Métodos
        /// <summary>
        /// Retorna uma nova transacao com o banco de dados
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbTransaction beginTrans()
        {
            DbTransaction retorno;
            retorno = con.BeginTransaction(IsolationLevel.ReadCommitted);
            return retorno;
        }

        public void CommitTrans(DbTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTrans(DbTransaction transaction)
        {
            transaction.Rollback();
        }

        /// <summary>
        /// Retorna uma nova instância do objeto DbCommand
        /// </summary>
        /// <returns></returns>
        /// <remarks>O tipo DbCommand é abstrato, o tipo real que será retornado dependerá do provider utilizado pelo objeto DbConnection (ex: OracleCommand, SqlCommand, OleDbCommand) </remarks>
        public DbCommand CreateCommand()
        {
            cmd = con.CreateCommand();
            cmd.Parameters.Clear();
            return cmd;
        }

        public DbDataReader ExecuteReader()
        {
            try
            {
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ExecuteNonQuery()
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object ExecuteScalar()
        {
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Executar consulta com retorno em DataReader
        /// </summary>
        /// <param name="sql">Query</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbDataReader doSel(string sql)
        {
            cmd = con.CreateCommand();
            cmd.CommandText = sql;
            return cmd.ExecuteReader();
        }

        public DbDataReader doSel(string sql, ref DbCommand cmd)
        {
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Retorna uma nova instância do objeto DbCommand
        /// </summary>
        /// <param name="sql">Propriedade CommandText do objeto</param>
        /// <param name="type">Propriedade CommandType do objeto</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DbCommand oleDbCmd(string sql, CommandType type)
        {
            DbCommand functionReturnValue = default(DbCommand);
            functionReturnValue = con.CreateCommand();
            functionReturnValue.CommandText = sql;
            functionReturnValue.CommandType = type;
            return functionReturnValue;
        }

        /// <summary>
        /// Fechar a conexao ativa
        /// </summary>
        /// <remarks></remarks>
        public void Close()
        {
            if (con != null && con.State != ConnectionState.Closed)
            {
                try
                {
                    con.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// Executar uma stored procedure
        /// </summary>
        /// <param name="storedProc">Nome da stored procedure</param>
        /// <param name="paramRet">Índice do parâmetro de retorno (Opcional)</param>
        /// <param name="Vcmd">Referência para um objeto DbCommand existente</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string exeProc(string storedProc, int paramRet, DbCommand Vcmd)
        {
            DbCommand cmd;
            string retorno = "0";

            try
            {
                if (Vcmd == null)
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = storedProc;
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd = Vcmd;
                }

                cmd.ExecuteNonQuery();

                if (paramRet != -1)
                {
                    retorno = cmd.Parameters[paramRet].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
            }

            return retorno;
        }

        public void Dispose()
        {
            try
            {
                con.Close();
            }
            catch { }
            try
            {
                con.Dispose();
            }
            catch { }
            try
            {
                con = null;
            }
            catch { }
        }

        #region Parameters
        public void AddParameter(string nome, object valor)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = nome;
            param.Value = valor;
            cmd.Parameters.Add(param);
        }

        public void AddParameter(string nome, object valor, DbType type)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = nome;
            param.Value = valor;
            param.DbType = type;
            cmd.Parameters.Add(param);
        }

        public DbParameterCollection Parameters
        {
            get { return cmd.Parameters; }
        }

        public void ParameterDirection(string nome, ParameterDirection direction)
        {
            cmd.Parameters[nome].Direction = direction;
        }

        public void ParemetersClear()
        {
            cmd.Parameters.Clear();
        }
        #endregion
    }
}
