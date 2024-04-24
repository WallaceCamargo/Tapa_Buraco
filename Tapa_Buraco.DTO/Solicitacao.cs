using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tapa_Buraco.DTO
{
    public class Solicitacao : Logradouro
    {
        public Solicitacao() {
            USUARIO = new Usuario();
        }

        public int ID { get; set; }
        public int ID_USUARIO { get; set; }
        public string NM_USUARIO { get; set; }
        public Usuario USUARIO { get; set; }
        public int STATUS { get; set; }
        public string STATUS_STRING {
            get
            {
                if (STATUS == 0) return "ABERTA";
                if (STATUS == 1) return "ACATADA";
                if (STATUS == 2) return "VISTORIADA";
                if (STATUS == 3) return "PROGRAMADA";
                if (STATUS == 4) return "EXECUTADA";
                if (STATUS == 5) return "FATURADA";
                else return "";
            }
        }
        public DateTime? DT_REGISTRO { get; set; }
        public DateTime? DT_DELETE { get; set; }
        public DateTime? DT_ACATAMENTO { get; set; }
        public string DT_ACATAMENTO_STRING
        {
            get
            {
                if (DT_ACATAMENTO != null)
                    return DT_ACATAMENTO.ToString();
                else
                    return "";
            }
        }
        public DateTime? DT_FISCALIZACAO { get; set; }
        public string DT_FISCALIZACAO_STRING
        {
            get
            {
                if (DT_FISCALIZACAO != null)
                    return DT_FISCALIZACAO.ToString();
                else
                    return "";
            }
        }
        public DateTime? DT_AGENDAMENTO { get; set; }
        public string DT_AGENDAMENTO_STRING
        {
            get
            {
                if (DT_AGENDAMENTO != null)
                    return DT_AGENDAMENTO.ToString();
                else
                    return "";
            }
        }
        public DateTime? DT_ATENDIMENTO { get; set; }
        public string DT_ATENDIMENTO_STRING
        {
            get
            {
                if (DT_ATENDIMENTO != null)
                    return DT_ATENDIMENTO.ToString();
                else
                    return "";
            }
        }
        public DateTime? DT_FINALIZACAO { get; set; }
        public string DT_FINALIZACAO_STRING
        {
            get
            {
                if (DT_FINALIZACAO != null)
                    return DT_FINALIZACAO.ToString();
                else
                    return "";
            }
        }
        public DateTime? DT_PRAZO { get; set; }
        public string DT_PRAZO_STRING {
            get
            {
                if (DT_PRAZO != null)
                    return DT_PRAZO.ToString(); 
                else
                    return "";
            }
        }
        public int METRAGEM { get; set; }
        public int TONELADA { get; set; }
        public string PRIORIDADE { get; set; }
        public HttpPostedFileBase IMG { get; set; }
        public string IMG_URL { get; set;}
        public string NOME_FOTO { get; set; }
        public File IMG_file { get; set;}
    }

    public class File
    {
        public Guid ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Extension { get; set; }
        public int System { get; set; }
        public byte[] dataArray { get; set; }

        public bool IsError { get; set; }
        public string Message { get; set; }

    }
}
