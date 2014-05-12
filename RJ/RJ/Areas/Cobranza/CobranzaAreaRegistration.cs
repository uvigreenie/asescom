using System.Web.Mvc;

namespace RJ.Areas.Cobranza
{
    public class CobranzaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cobranza";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Cobranza_default",
                "Cobranza/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
