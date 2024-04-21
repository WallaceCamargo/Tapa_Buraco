using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Util
{
    public class NLogHelper
    {
        public static Logger logger;
        static NLogHelper()
        {      
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") 
            { 
                FileName = "C:\\logs\\SistemaRAP.WebAPi\\WebAPI.log",
                Layout = "${longdate} ${logger} ${uppercase:${level}} ${message}",
                ArchiveFileName = "C:\\logs\\SSistemaRAP.WebAPi\\WebAPI.{#####}.log",
                ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
                MaxArchiveFiles = 7,
                ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = Encoding.UTF8,
            };          

            // Rules for mapping loggers to targets           
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;


            logger = LogManager.GetCurrentClassLogger();
        }



        public static void InicioRequisição(string caminho, string parametros)
        {
            NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            NLogHelper.logger.Info("Início requisição WebAPi {0})", caminho);
            NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            NLogHelper.logger.Info("Parâmetros Recebidos => {0}", parametros);
            NLogHelper.logger.Info("");
        }

        public static void FimRequisição(string caminho, string retorno)
        {
            NLogHelper.logger.Info("");
            NLogHelper.logger.Info("Retorno: '{0}'", retorno);
            NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            NLogHelper.logger.Info("Fim requisição WebAPi {0}", caminho);
            NLogHelper.logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        }

        public static void ErroNlog(string caminho, Exception ex)
        {
            NLogHelper.logger.Info("**********************************************");
            NLogHelper.logger.Info("ERRO Inesperado - Requisição WebAPi {0}", caminho);
            NLogHelper.logger.Info("**********************************************");
            NLogHelper.logger.Error(ex, ex.ToString());
            NLogHelper.logger.Info("**********************************************");
        }



    }
}
