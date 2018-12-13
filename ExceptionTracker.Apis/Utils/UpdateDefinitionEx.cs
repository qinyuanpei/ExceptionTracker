using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExceptionTracker.Apis.Utils
{
    /// <summary>
    /// Mongodb UpdateDefinition扩展类
    /// </summary>
    public static class UpdateDefinitionEx
    {
        /// <summary>
        /// 根据BsonDocument生成UpdateDefinition
        /// </summary>
        /// <param name="document">BsonDocument</param>
        /// <param name="parent">父级节点</param>
        /// <returns></returns>
        public static List<UpdateDefinition<BsonDocument>> BuildUpdateDefine(this BsonDocument document, string parent = null)
        {
            var updateDefinitions = new List<UpdateDefinition<BsonDocument>>();
            foreach (var element in document.Elements)
            {
                //主键Id不允许更新
                if (element.Name == "_id") continue;

                //空值不允许更新
                if (element.Value.IsBsonNull) continue;

                var key = string.IsNullOrEmpty(parent) ? element.Name : $"{parent}.{element.Name}";

                //BsonDocument
                if (element.Value.IsBsonDocument)
                {
                    updateDefinitions.AddRange(element.Value.ToBsonDocument().BuildUpdateDefine(key));
                }
                //BsonArray
                else if (element.Value.IsBsonArray)
                {
                    var bsonArray = element.Value.AsBsonArray;
                    for (int i = 0; i < bsonArray.Count; i++)
                    {
                        var bsonValue = bsonArray[i];
                        //BsonDocument
                        if (bsonValue.IsBsonDocument)
                        {
                            updateDefinitions.AddRange(bsonValue.ToBsonDocument().BuildUpdateDefine($"{key}.{i}"));
                        }
                        else
                        {
                            updateDefinitions.Add(Builders<BsonDocument>.Update.Set(e => e[key], bsonValue));
                        }
                    }
                }
                else
                {
                    updateDefinitions.Add(Builders<BsonDocument>.Update.Set(e => e[key], element.Value));
                }
            }

            return updateDefinitions;
        }
    }
}
