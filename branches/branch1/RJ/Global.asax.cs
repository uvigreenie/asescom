using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RJ
{

    public class MvcApplication : HttpApplication
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
                new { controller = "Index", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(Areas.Seguridad.Controllers.IndexController).Namespace }// Parameter defaults
            ).DataTokens.Add("Areas", "Seguridad");

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //GlobalFilters.Filters.Add(new RequireHttpsAttribute());
            ControllerBuilder.Current.DefaultNamespaces.Add("RJ.Areas.Seguridad.Controllers");
        }
    }
}