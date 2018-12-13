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
        public static string ReadAsString(this HttpRequest request)
        {
            using (var memoryStream = new MemoryStream())
            {
                request.Body.CopyTo(memoryStream);
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
