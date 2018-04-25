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
                    logger = LogManager.GetLogger("LogTool.Logger");
                    var str = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
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

        public static void Error(Exception ex)
        {
            logger.Error(ex);
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }
    }
}

