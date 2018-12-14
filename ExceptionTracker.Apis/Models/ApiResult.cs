using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ExceptionTracker.Apis.Models
{
    [Serializable]
    public class ApiResult<TResult>
    {
        /// <summary>
        /// 结果标记
        /// </summary>
        public bool Flag { get; set; }

        public string StatusCode { get; set; }

        public string Msssage { get; set; }

        public TResult Result { get; set; }
    }

    [Serializable]
    public class ApiResult
    {
        /// <summary>
        /// 结果标记
        /// </summary>
        public bool Flag { get; set; }

        public int StatusCode { get; set; }

        public string Msssage { get; set; }

        public object Result { get; set; }
    }
}
