using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCSampleProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional},
                namespaces: new[] { "MVCSampleProject.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "Products/{action}/{id}",
                defaults: new { controller = "Products", action = "Index", id = 1 },
                namespaces: new[] { "MVCSampleProject.Controllers" }
            );
        }
    }
}
