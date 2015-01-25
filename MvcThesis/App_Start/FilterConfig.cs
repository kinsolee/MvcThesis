﻿using System.Web;
using System.Web.Mvc;

namespace MvcThesis
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MultipleResponseFormatsAttribute());
        }

    }

    public class MultipleResponseFormatsAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["controller"].ToString()=="Error") return;
            if (filterContext.HttpContext.Request.Browser.Browser == "IE" && filterContext.HttpContext.Request.Browser.MajorVersion < 8)
                filterContext.HttpContext.Response.Redirect("~/Error/Browser", true);
            //base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var ViewResult = filterContext.Result as ViewResult;
            if (ViewResult == null) return;
            if (request.IsAjaxRequest() || request.QueryString["ajax"]=="1")
            {
                filterContext.Result = new PartialViewResult
                {
                    TempData = ViewResult.TempData,
                    ViewData = ViewResult.ViewData,
                    ViewName = ViewResult.ViewName
                };
            }
        }
    }

}