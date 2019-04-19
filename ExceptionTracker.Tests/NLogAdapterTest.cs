using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Xunit;
using System.IO;

namespace ExceptionTracker.Tests
{
    public class NLogAdapterTest
    {
        private readonly ILogger logger;

        public NLogAdapterTest()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
        }


        [Fact]
        public void Test_SimpleData()
        {
            logger.Info("这是一条Info级别的日志");
            logger.Warn("这是一条Warn级别的日志");
            logger.Debug("这是一条Debug级别的日志");
            logger.Error("这是一条Error级别的日志");
            logger.Fatal("这是一条Fatal级别的日志");
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
                logger.Info(ex, "指定的文件不存在");
            }
        }
    }
}
