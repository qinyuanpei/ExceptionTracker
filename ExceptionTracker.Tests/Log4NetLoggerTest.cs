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
        public void Test_TextMessage()
        {
            logger.Info("����һ��Info�������־");
            logger.Warn("����һ��Warn�������־");
            logger.Debug("����һ��Debug�������־");
            logger.Fatal("����һ��Fatal�������־");
        }

        [Fact]
        public void Test_MessageObject()
        {
            logger.Info(new { Text = "����һ��Info�������־", CreateBy = "Payne" });
            logger.Warn(new { Text = "����һ��Warn�������־", CreateBy = "Payne" });
            logger.Debug(new { Text = "����һ��Debug�������־", CreateBy = "Payne" });
            logger.Fatal(new { Text = "����һ��Fatal�������־", CreateBy = "Payne" });
        }

        [Fact]
        public void Test_Structuredlog()
        {
            var args = new object[] {"Payne", "����ʱ�������"};
            var format = "����һ����{0}������Info�������־������Ϊ��{1}";
            logger.InfoFormat(format,args);
        }

        [Fact]
        public void Test_ExceptionLog()
        {
            try
            {
                var file = File.Open("D:\\DUAL.txt", FileMode.Open);
            }
            catch (Exception ex)
            {
                logger.Info("ָ�����ļ�������", ex);
            }
        }

    }
}
