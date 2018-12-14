using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ExceptionTracker.Apis.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionTracker.Apis.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public virtual void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult)
            {
                var result = context.Result as ObjectResult;
                if (result != null)
                {
                    context.Result = new JsonResult(new ApiResult
                    {
                        Flag = true,
                        Result = result,
                        StatusCode = "200",
                        Msssage = string.Empty
                    });
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult
                    {
                        Flag = false,
                        Result = null,
                        StatusCode = "404",
                        Msssage = string.Empty
                    });
                }
            }
        }
    }
}
