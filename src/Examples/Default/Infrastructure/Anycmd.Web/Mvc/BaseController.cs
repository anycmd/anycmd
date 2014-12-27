
namespace Anycmd.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// 自定义ASP.NET MVC控制器抽象基类
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected static IAcDomain Host
        {
            get
            {
                return System.Web.HttpContext.Current.Application["AcDomainInstance"] as IAcDomain;
            }
        }

        /// <summary>
        /// Gets a service. Returns null if service is not found.
        /// </summary>
        protected T GetRequiredService<T>() where T : class
        {
            return Host.RetrieveRequiredService<T>();
        }

        protected ViewResultBase ViewResult()
        {
            if (!string.IsNullOrEmpty(this.Request["isInner"]) || !string.IsNullOrEmpty(Request["isTooltip"]))
            {
                return this.PartialView("Partials/" + this.ControllerContext.RouteData.Values["Action"]);
            }
            return this.View();
        }

        /// <summary>
        /// 当请求与此控制器匹配但在此控制器中找不到任何具有指定操作名称的方法时调用。
        /// </summary>
        /// <param name="actionName">尝试的操作的名称。</param>
        protected override void HandleUnknownAction(string actionName)
        {
            IController errorController = new ErrorController();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Http404");
            errorRoute.Values.Add("url", Request.Url.OriginalString);
            errorRoute.Values.Add("unknownAction", actionName);
            errorController.Execute(new RequestContext(HttpContext, errorRoute));
        }
    }
}
