﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ExceptionTracker.Apis.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionTracker.Apis.Filters
{
    /// <summary>
    /// WebApi返回值过滤器
    /// </summary>
    public class WebApiResultFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

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
                        StatusCode = (int)HttpStatusCode.OK,
                        Msssage = string.Empty
                    });
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult
                    {
                        Flag = false,
                        Result = null,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Msssage = string.Empty
                    });
                }
            }
        }
    }
}
