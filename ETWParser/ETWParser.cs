using System;
using System.IO;
using CommandLine;
using NLog;

namespace ETWParser
{
    class Program
    {
        /// <summary>
        /// Program Entry
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Setup console
            Console.Title = "ETW Trace Processor";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //Parse args
            if (!ParseArgs(args))
            {
                Console.WriteLine("Parse arguments failed");
                return;
            }

            //Init Logger
            Logger.InitLogger("ETWParser", LogLevel.Trace);

            //Init 
            Init();

            //debug purpose
            Symbol.GetEnv();

            //Process Trace
            string etlFilePath = m_option.Trace.ToString();
            if (File.Exists(etlFilePath))
            {
                Console.WriteLine("etl path: {0}", etlFilePath);
                m_eventHander.ProcessTrace(etlFilePath);
            }
            else
            {
                Logger.logger.Info("xperf.etl isn't existing: {0}", etlFilePath);
            }

        }

        static bool Init()
        {
            m_eventHander = new EventHandler();
            return true;
        }

        /// <summary>
        /// Parse program arguments
        /// </summary>
        /// <param name="args"></param>
        static bool ParseArgs(string[] args)
        {
            //m_option= Parser.Default.ParseArguments<Options>(args);
            //if (m_option.Value.ReportDirPath == null || !Directory.Exists(m_option.Value.ReportDirPath))
            //    return false;
            //Console.WriteLine(m_option.Value.ReportDirPath);
            m_option = new Options();
            Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => m_option = parsed);
            if (m_option.Trace == null || !File.Exists(m_option.Trace))
                return false;
            return true;

        }

        static Options m_option;
        static EventHandler m_eventHander;
    }
}
