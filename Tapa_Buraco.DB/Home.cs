using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.DB
{
    public class Home
    {
        public async Task<List<DTO.Home>> GetAll(int id, string nome, string joint, string loa, string numeroViagem, string agencia)
        {
            List<DTO.Home> linesUp = null;

            try
            {
                using (var db = new Dados("SISTEMARAP"))
                {
                    StringBuilder SQL = new StringBuilder("");

                    SQL.AppendLine("SELECT A.ID, B.NOME, A.JOINT, B.LOA, A.NUMERO_VIAGEM, B.AGENCIA FROM TB_LINE_UP A JOIN TB_NAVIO B ON B.ID = A.ID_NAVIO ORDER BY B.NOME ASC");
                    
                    var result = await db.Connection.QueryAsync<DTO.Home>(SQL.ToString(),
                                    new
                                    {
                                        ID = id,
                                        NOME = string.IsNullOrEmpty(nome) ? "" : nome,
                                        JOINT = string.IsNullOrEmpty(joint) ? "" : joint,
                                        LOA = string.IsNullOrEmpty(loa) ? "" : loa,
                                        NUMERO_VIAGEM = string.IsNullOrEmpty(numeroViagem) ? "" : numeroViagem,
                                        AGENCIA = string.IsNullOrEmpty(agencia) ? "" : agencia,
                                    });
                    linesUp = result.ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return linesUp;
        }
    }
}
