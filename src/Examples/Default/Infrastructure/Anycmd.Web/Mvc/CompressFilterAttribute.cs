using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace Anycmd.Web.Mvc
{
    /// <summary>
    /// 表示一个特性，标记该特性以支持压缩
    /// </summary>
    [DebuggerStepThrough]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CompressFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
