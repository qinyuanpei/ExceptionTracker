﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExceptionTracker.Apis.Utils
{
    /// <summary>
    /// FilterDefinition扩展类
    /// </summary>
    public static class FilterDefinitionEx
    {
        public static List<FilterDefinition<BsonDocument>> BuildFilterDefinition(this BsonDocument document)
        {
            return null;
        }
    }
}
