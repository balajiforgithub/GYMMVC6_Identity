using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace GYMONE.Filters
{
    ////public class MyExceptionHandler : ActionFilterAttribute 
    ////{
    ////    public override void OnActionExecuting(ActionExecutingContext filterContext)
    ////    {
    ////        HttpContextBase ctx = filterContext.HttpContext;

    ////        // check if session is supported
    ////        if (ctx.Session["UserID"] == null)
    ////        {
    ////            filterContext.Result = new RedirectResult("/Account/Login");
    ////        }

    ////        base.OnActionExecuting(filterContext);
    ////    }
    ////}
}