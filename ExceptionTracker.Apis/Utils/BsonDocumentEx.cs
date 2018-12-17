using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace ExceptionTracker.Apis.Utils
{
    public static class BsonDocumentEx
    {
        /// <summary>
        /// 转换BsonDocument为字典结构
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static object ToJsonEx(this BsonDocument document)
        {
            //针对JSON对象数组序列化进行特殊处理
            var keys = document.Elements.Select(e => e.Name);
            if (keys.Any(k => Regex.Match(k, @"^[0-9]\d*").Success))
                return document.Elements.Select(e => e.Value.AsObject()).ToList();
            return document.Elements.ToDictionary(e => e.Name, e => e.Value.AsObject());
        }

        /// <summary>
        /// 转换BsonDocument为C#内置类型
        /// </summary>
        /// <param name="bsonValue"></param>
        /// <returns></returns>
        public static object AsObject(this BsonValue bsonValue)
        {
            if (bsonValue.IsBsonDocument)
                return bsonValue.AsBsonDocument.ToJsonEx();
            else if (bsonValue.IsBsonArray)
                return bsonValue.AsBsonArray;
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
