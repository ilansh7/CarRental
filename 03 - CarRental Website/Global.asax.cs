using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SunshineRentals
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error()
        //{
        //    var exception = Server.GetLastError();
        //    var httpException = exception as HttpException;
        //    Response.Clear();
        //    Server.ClearError();
        //    var routeData = new RouteData();
        //    routeData.Values["controller"] = "Errors";
        //    routeData.Values["action"] = "General";
        //    routeData.Values["exception"] = exception;
        //    Response.StatusCode = 500;
        //    if (httpException != null)
        //    {
        //        Response.StatusCode = httpException.GetHttpCode();
        //        switch (Response.StatusCode)
        //        {
        //            case 403:
        //                routeData.Values["action"] = "Http403";
        //                break;
        //            case 404:
        //                routeData.Values["action"] = "Http404";
        //                break;
        //        }
        //    }
        //    // Avoid IIS7 getting in the middle
        //    Response.TrySkipIisCustomErrors = true;
        //    IController errorsController = new ErrorsController();
        //    HttpContextWrapper wrapper = new HttpContextWrapper(Context);
        //    var rc = new RequestContext(wrapper, routeData);
        //    errorsController.Execute(rc);
        //}

        public class ErrorsController : Controller
        {
            public ActionResult General(Exception exception)
            {
                return View("~/Views/Shared/Error.cshtml", exception); //Exception
            }

            public ActionResult Http404()
            {
                return View("~/Views/Shared/Error.cshtml");//404
            }

            public ActionResult Http403()
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}