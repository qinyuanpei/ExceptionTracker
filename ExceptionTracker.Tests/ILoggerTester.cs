﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionTracker.Tests
{
    public interface ILoggerTester
    {
        void Test_TextMessage();

        void Test_MessageObject();

        void Test_Structuredlog();

        void Test_ExceptionLog();
    } 
}
