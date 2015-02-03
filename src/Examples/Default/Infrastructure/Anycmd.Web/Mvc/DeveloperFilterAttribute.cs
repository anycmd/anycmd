
namespace Anycmd.Web.Mvc
{
    using Engine.Ac;
    using Engine.Host;
    using Exceptions;
    using System;
    using System.Web.Mvc;
    using Util;
    using ViewModel;

    /// <summary>
    /// 表示一个特性。表示使用该标记标记的功能需要开发人员身份。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class DeveloperFilterAttribute : ActionFilterAttribute
    {
        private const string Msg = "对不起您不是开发人员";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var acDomain = (filterContext.HttpContext.Application[Constants.ApplicationRuntime.AcDomainCacheKey] as IAcDomain);
            if (acDomain == null)
            {
                throw new AnycmdException("");
            }
            var storage = acDomain.GetRequiredService<IAcSessionStorage>();
            var user = storage.GetData(acDomain.Config.CurrentAcSessionCacheKey) as IAcSession;
            if (!user.IsDeveloper())
            {
                var request = filterContext.HttpContext.Request;
                var isAjaxRequest = request.IsAjaxRequest();
                if (isAjaxRequest)
                {
                    filterContext.Result = new FormatJsonResult
                    {
                        Data = new ResponseData { success = false, msg = Msg }
                    };
                }
                else
                {
                    filterContext.Result = new ContentResult() { Content = Msg }; ;
                }
                return;
            }
        }
    }
}
