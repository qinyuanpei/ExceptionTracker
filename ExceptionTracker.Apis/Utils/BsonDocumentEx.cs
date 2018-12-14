﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ExceptionTracker.Apis.Utils
{
    public static class BsonDocumentEx
    {
        /// <summary>
        /// 转换BsonDocument为字典结构
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this BsonDocument document)
        {
            return document.Elements.ToDictionary(
                e => e.Name,
                e => e.Value.AsObject()
            );
        }

        /// <summary>
        /// 转换BsonDocument为C#内置类型
        /// </summary>
        /// <param name="bsonValue"></param>
        /// <returns></returns>
        public static object AsObject(this BsonValue bsonValue)
        {
            if (bsonValue.IsBsonDocument)
                return bsonValue.AsBsonDocument.ToDictionary();
            else if (bsonValue.IsBsonArray)
                return bsonValue.AsBsonArray.Select(e => e.AsObject()).ToArray();
            else if (bsonValue.IsBoolean)
                return bsonValue.AsBoolean;
            else if (bsonValue.IsValidDateTime)
                return bsonValue.ToUniversalTime();
            else if (bsonValue.IsBsonNull)
                return null;
            else if (bsonValue.IsObjectId)
                return bsonValue.AsObjectId;
            else if (bsonValue.IsDouble)
                return bsonValue.AsDouble;
            else if (bsonValue.IsInt32)
                return bsonValue.AsInt32;
            else if (bsonValue.IsInt64)
                return bsonValue.AsInt64;
            else if (bsonValue.IsString)
                return bsonValue.AsString;
            else
                return bsonValue;
        }
    }
}