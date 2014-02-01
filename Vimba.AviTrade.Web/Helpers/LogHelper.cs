using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class LogHelper
    {
        public static bool LoggingEnabled { get { return !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LogFile"]); } }

        public static string GetLogPath(this HttpServerUtility server)
        {
            var serverBase = new HttpServerUtilityWrapper(server) as HttpServerUtilityBase;
            return serverBase.GetLogPath();
        }

        public static string GetLogPath(this HttpServerUtilityBase server)
        {
            return LoggingEnabled ? server.MapPath(string.Concat("~/", ConfigurationManager.AppSettings["LogFile"])) : null;
        }

        public static XDocument GetLogXml(this HttpServerUtility server)
        {
            var serverBase = new HttpServerUtilityWrapper(server) as HttpServerUtilityBase;
            return serverBase.GetLogXml();
        }

        public static XDocument GetLogXml(this HttpServerUtilityBase server)
        {
            string logPath = server.GetLogPath();
            XDocument log = null;
            if (!LoggingEnabled)
            {
                throw new NullReferenceException("Logging isn't enabled. Please set an \"appSettings\" item in web.config with \"LogFile\" as key and the Log's file name as value.");
            }
            else if (!File.Exists(logPath))
            {
                log = new XDocument();
                log.Declaration = new XDeclaration("1.0", "utf-8", "yes");
                log.Add(new XElement("errors"));
                log.Save(logPath);
            }
            else
            {
                log = log ?? XDocument.Load(logPath);
            }

            return log;
        }

        public static bool LogException(this HttpServerUtility server, Exception exception)
        {
            var serverBase = new HttpServerUtilityWrapper(server) as HttpServerUtilityBase;
            return serverBase.LogException(exception);
        }

        public static bool LogException(this HttpServerUtilityBase server, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (LoggingEnabled)
            {
                XDocument log = server.GetLogXml();
                XElement error = new XElement("error", new XAttribute("datetime", DateTime.Now.ToString()));
                string stackTrace = null;

                Exception inner = exception;
                do
                {
                    error.Add(new XElement("exception", inner.Message));
                    if (string.IsNullOrWhiteSpace(stackTrace))
                        stackTrace = inner.StackTrace;
                    inner = inner.InnerException;
                } while (inner != null);

                error.Add(new XElement("trace", stackTrace));

                log.Element("errors").Add(error);
                log.Save(server.GetLogPath());
                log = null;

                return true; //Logging enabled and exception saved
            }
            return false;
        }
    }
}
