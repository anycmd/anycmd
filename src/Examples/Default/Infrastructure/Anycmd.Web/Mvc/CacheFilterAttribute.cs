
namespace Anycmd.Web.Mvc
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerStepThrough]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the cache duration in seconds. The default is int.MaxValue seconds.
        /// </summary>
        /// <value>The cache duration in seconds.</value>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// 以分号分割，形如"appSystemCode;isSmall"
        /// </summary>
        public string VaryByParam { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CacheFilterAttribute()
        {
            Duration = int.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var host = filterContext.HttpContext.Application["AcDomainInstance"] as IAcDomain;
            if (Enable || host.Config.EnableClientCache)
            {
                if (Duration <= 0) return;

                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(cacheDuration));
                if (!string.IsNullOrEmpty(VaryByParam))
                {
                    string[] parms = VaryByParam.Split(';');
                    foreach (var parm in parms)
                    {
                        cache.VaryByParams[parm] = true;
                    }
                }
                cache.SetMaxAge(cacheDuration);
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }
        }
    }
}
