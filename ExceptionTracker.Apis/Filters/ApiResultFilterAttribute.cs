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
            
        }
    }
}
