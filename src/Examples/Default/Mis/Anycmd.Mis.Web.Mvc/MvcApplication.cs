
namespace Anycmd.Mis.Web.Mvc
{
    using Ac.Queries.Ef.Identity;
    using Anycmd.Web;
    using Anycmd.Web.Mvc;
    using Edi.Application;
    using Edi.MessageServices;
    using Edi.Queries.Ef;
    using Ef;
    using Engine.Host;
    using Engine.Host.Impl;
    using Logging;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Util;

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

            var acDomain = new DefaultAcDomain();
            Application.Add(Constants.ApplicationRuntime.AcDomainCacheKey, acDomain);
            acDomain.AddService(typeof(IFunctionListImport), new FunctionListImport());
            acDomain.AddService(typeof(IEfFilterStringBuilder), new EfFilterStringBuilder());
            acDomain.AddService(typeof(ILoggingService), new Log4NetLoggingService(acDomain));
            acDomain.AddService(typeof(IUserSessionStorage), new WebUserSessionStorage());
            acDomain.Init();

            acDomain.RegisterRepository(new List<string>
            {
                "EdiEntities",
                "AcEntities",
                "InfraEntities",
                "IdentityEntities"
            }, typeof(AcDomain).Assembly);
            acDomain.RegisterQuery(typeof(BatchQuery).Assembly, typeof(AccountQuery).Assembly);
            acDomain.RegisterEdiCore();

            (new ServiceHost(acDomain, "", typeof(MessageService).Assembly)).Init();
        }
    }
}