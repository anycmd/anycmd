
namespace Anycmd.Web.Mvc
{
    using Engine.Ac;
    using System;
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// 表示一个特性。表示使用该标记标记的功能需要开发人员身份。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class DeveloperFilterAttribute : ActionFilterAttribute
    {
        private const string MSG = "对不起您不是开发人员";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = (filterContext.HttpContext.Application["AcDomainInstance"] as IAcDomain).UserSession;
            if (!user.IsDeveloper())
            {
                var request = filterContext.HttpContext.Request;
                var isAjaxRequest = request.IsAjaxRequest();
                if (isAjaxRequest)
                {
                    filterContext.Result = new FormatJsonResult
                    {
                        Data = new ResponseData { success = false, msg = MSG }
                    };
                }
                else
                {
                    filterContext.Result = new ContentResult() { Content = MSG }; ;
                }
                return;
            }
        }
    }
}
