using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using ExceptionTracker.Logger.Adapter.SeriLog;
using Xunit;
using System.IO;
using ExceptionTracker.Logger.Adapter;

namespace ExceptionTracker.Tests
{
    public class SeriLogAdapterTest : ILoggerAdapterTest
    {
        private readonly ILogger logger;
        public SeriLogAdapterTest()
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.MongoAdapter(new MongoAdapterOptions()
                {
                    ConnectionString = "mongodb://localhost:27017",
                    CollectionName = "mongo-serilog"
                })
                .CreateLogger();
        }

        [Fact]
        public void Test_TextMessage()
        {
            logger.Information("这是一条Info级别的日志");
            logger.Warning("这是一条Warn级别的日志");
            logger.Debug("这是一条Debug级别的日志");
            logger.Fatal("这是一条Fatal级别的日志");
        }

        [Fact]
        public void Test_MessageObject()
        {
            //logger.Info(new { Text = "这是一条Info级别的日志", CreateBy = "Payne" });
            //logger.Warn(new { Text = "这是一条Warn级别的日志", CreateBy = "Payne" });
            // //logger.Debug(new { Text = "这是一条Debug级别的日志", CreateBy = "Payne" });
            ////logger.Fatal(new { Text = "这是一条Fatal级别的日志", CreateBy = "Payne" });
        }

        [Fact]
        public void Test_Structuredlog()
        {
            //var args = new object[] { "Payne", "想你时你在天边" };
            //var format = "这是一条由{0}创建的Info级别的日志，内容为：{1}";
            //logger.Info(format, args);
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
