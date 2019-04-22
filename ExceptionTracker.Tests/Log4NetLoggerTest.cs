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
            logger.Info("这是一条Info级别的日志");
            logger.Warn("这是一条Warn级别的日志");
            logger.Debug("这是一条Debug级别的日志");
            logger.Fatal("这是一条Fatal级别的日志");
        }

        [Fact]
        public void Test_MessageObject()
        {
            logger.Info(new { Text = "这是一条Info级别的日志", CreateBy = "Payne" });
            logger.Warn(new { Text = "这是一条Warn级别的日志", CreateBy = "Payne" });
            logger.Debug(new { Text = "这是一条Debug级别的日志", CreateBy = "Payne" });
            logger.Fatal(new { Text = "这是一条Fatal级别的日志", CreateBy = "Payne" });
        }

        [Fact]
        public void Test_Structuredlog()
        {
            var args = new object[] {"Payne", "想你时你在天边"};
            var format = "这是一条由{0}创建的Info级别的日志，内容为：{1}";
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
                logger.Info("指定的文件不存在", ex);
            }
        }

    }
}
