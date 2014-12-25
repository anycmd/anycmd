using System.Web.Mvc;

namespace Anycmd.Mis.Web.Mvc
{
    public class MisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mis_default",
                "Mis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Anycmd.Mis.Web.Mvc.Controllers" }
            );
        }
    }
}
