using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using Xunit;

namespace ExceptionTracker.Tests
{
    public class Log4NetLoggerTest
    {
        private readonly ILog logger;

        public Log4NetLoggerTest()
        {
            ILoggerRepository repository = LogManager.CreateRepository("EtlogRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, typeof(Log4NetLoggerTest));
        }

        [Fact]
        public void Test_SimpleData()
        {
            logger.Info("����һ��Info�������־");
            logger.Warn("����һ��Warn�������־");
            logger.Error("����һ��Error�������־");
            logger.Fatal("����һ��Fatal�������־");
        }

        [Fact]
        public void Test_LogException()
        {
            try
            {
                var file = File.Open("D:\\DUAL.txt", FileMode.Open);
            }
            catch (Exception ex)
            {
                logger.Info("ָ�����ļ�������",ex);
            }
        }
    }
}
