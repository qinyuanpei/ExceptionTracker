using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Xunit;
using System.IO;

namespace ExceptionTracker.Tests
{
    public class NLogAdapterTest : ILoggerAdapterTest
    {
        private readonly ILogger logger;

        public NLogAdapterTest()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
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
            var args = new object[] { "Payne", "想你时你在天边" };
            var format = "这是一条由{0}创建的Info级别的日志，内容为：{1}";
            logger.Info(format, args);
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
                logger.Error(ex, "指定的文件不存在");
            }
        }

        [Fact]
        public void Test_CustomFields()
        {

        }
    }
}
