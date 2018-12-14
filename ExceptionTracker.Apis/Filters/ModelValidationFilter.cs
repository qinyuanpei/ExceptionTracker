using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExceptionTracker.Apis.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionTracker.Apis.Filters
{
    /// <summary>
    /// 模型校验过滤器
    /// </summary>
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errorMsgs = modelState.SelectMany(e => e.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                var result = new ApiResult()
                {
                    Flag = false,
                    Result = null,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Msssage = string.Join(",", errorMsgs),
                };

                context.Result = new JsonResult(result);
            }
        }
    }
}
