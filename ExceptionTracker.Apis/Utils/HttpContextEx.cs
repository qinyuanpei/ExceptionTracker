using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExceptionTracker.Apis.Utils
{
    public static class HttpContextEx
    {
        /// <summary>
        /// 返回Http请求中的Body
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ReadAsString(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
