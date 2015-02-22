using System.Web.Mvc;

namespace RJ.Areas.Laboral
{
    public class LaboralAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Laboral";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Laboral_default",
                "Laboral/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
