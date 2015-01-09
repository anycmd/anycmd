
namespace Anycmd.Mis.Web.Mvc
{
    using Anycmd.Web.Mvc;
    using Ef;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : HttpApplication
    {
        public override void Init()
        {
            EfContext.InitStorage(new WebEfContextStorage(this));
            base.Init();
        }

        protected void Application_Start()
        {
            #region ASP.NET MVC
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            ModelBinders.Binders.DefaultBinder = new PlistModelBinder();
            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
            AreaRegistration.RegisterAllAreas();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            RouteTable.Routes.IgnoreRoute("ws/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("api/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "HomeIndex",
                "",
                new { controller = "Home", action = "Index" });

            RouteTable.Routes.MapRoute(
                "Home",
                "Home/{action}",
                new { controller = "Home", action = "Index" });

            RouteTable.Routes.MapRoute(
               "Default",
               "{controller}/{action}/{id}",
               new { controller = "Error", action = "Http404", id = UrlParameter.Optional });
            #endregion

            new MisAcDomain(this).Init();
        }
    }
}