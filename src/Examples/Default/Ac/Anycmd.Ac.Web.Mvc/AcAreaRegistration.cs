using System.Web.Mvc;

namespace Anycmd.Ac.Web.Mvc
{
    public class AcAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ac";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ac_default",
                "Ac/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Anycmd.Ac.Web.Mvc.Controllers" }
            );
        }
    }
}
