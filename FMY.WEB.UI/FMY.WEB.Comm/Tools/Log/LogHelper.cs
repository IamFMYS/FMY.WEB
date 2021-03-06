﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace FMY.WEB.Comm.Tools.Log
{
    public class LogTool
    {
        private static ILog logger = null;

        static LogTool()
        {            
            LoadConfig();
        }

        private static void LoadConfig()
        {
            //log4net.Util.LogLog.InternalDebugging = true;//??
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/log4net.xml");
            FileInfo fileInfo = new FileInfo(fileName);
            log4net.Config.XmlConfigurator.Configure(fileInfo);
        }

        private LogTool() { }

        public static ILog Logger
        {
            get
            {
                if (logger == null)
                {
                    //string loggerName = System.Configuration.ConfigurationManager.AppSettings["LoggerName"];
                    logger = LogManager.GetLogger("FMY.UI.LOG");                    
                }
                return logger;
            }
            private set { }
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void ErrorFromate(string message,params object[] args)
        {
            Logger.ErrorFormat(message, args);
        }

        public static void Error(Exception ex)
        {
            Logger.Error(ex);
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }
    }
}

