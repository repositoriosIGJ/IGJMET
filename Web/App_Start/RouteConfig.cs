using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Report",
                url: "Reporte/TramitePago/{operacion}/{timestamp}/{token}",
                defaults: new
                {
                    controller = "Reporte", 
                    action = "TramitePago", 
                    operacion = UrlParameter.Optional, 
                    timestamp = UrlParameter.Optional, 
                    token = UrlParameter.Optional
                }
            );
        }
    }
}