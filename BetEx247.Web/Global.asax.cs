using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;

namespace BetEx247.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "LeagueCountry", // Route name
                "{controller}/{action}/{id}/{sid}", // URL with parameters
                new { controller = "League", action = "Index", id = UrlParameter.Optional, sid = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "League", // Route name
                "{controller}/{action}/{id}/{cid}/{sid}", // URL with parameters
                new { controller = "League", action = "Index", id = UrlParameter.Optional,cid=UrlParameter.Optional,sid=UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Session_Start(object src, EventArgs e)
        {
            if (Context.Session != null)
            {
                if (Context.Session.IsNewSession)
                {
                    //string sCookieHeader = Request.Headers["Cookie"];
                    //if ((null != sCookieHeader) && (sCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    //{
                    //    // how to simulate it ???   
                    //    // RedirectToAction(“ActionName”, “ControllerName”,  route values);  
                    //    Response.Redirect("/Home/TestAction");
                    //}
                    var instanceGui= GuiService.Instance;
                }
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IoC.Initialize(new UnityDependencyResolver());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}