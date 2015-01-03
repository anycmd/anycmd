
namespace Anycmd.Web.Mvc
{
    using Engine.Ac;
    using Engine.Host;
    using Exceptions;
    using Logging;
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Util;
    using ViewModel;

    /// <summary>
    /// 拦截Action的异常
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionContext"></param>
        public void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.Exception != null)
            {
                var host = (exceptionContext.HttpContext.Application[Constants.ApplicationRuntime.AcDomainCacheKey] as IAcDomain);
                if (host == null)
                {
                    throw new AnycmdException("");
                }
                var storage = host.GetRequiredService<IUserSessionStorage>();
                var user = storage.GetData(host.Config.CurrentUserSessionCacheKey) as IUserSession;
                bool isValidationException = exceptionContext.Exception is ValidationException;
                bool isAjaxRequest = exceptionContext.HttpContext.Request.IsAjaxRequest();
                ActionResult result = null;
                // 如果是验证异常就不记录异常信息了
                if (isValidationException)
                {
                    result = GetValidationException(exceptionContext, isAjaxRequest);
                }
                else
                {
                    var logMessage = new AnycmdLogMessage(exceptionContext.Exception.Message);

                    // 记录异常
                    host.LoggingService.Error(logMessage, exceptionContext.Exception);

                    // 如果当前登录的不是开发人员就不展示详细异常了
                    if (user.IsDeveloper() || GetClientIp() == IPAddress.Loopback.ToString())
                    {
                        result = GetErrorForDeveloper(exceptionContext, isAjaxRequest, logMessage.Id);
                    }
                    else
                    {
                        result = GetErrorForNormalUser(host, isAjaxRequest);
                    }
                }

                exceptionContext.Result = result;
            }
            exceptionContext.ExceptionHandled = true;
        }

        #region private methods

        private static ActionResult GetValidationException(ExceptionContext exceptionContext
            , bool isAjaxRequest)
        {
            ActionResult result;
            var msg = string.Format(
@"<b>验证失败:  </b><span style='color:Red;'>{0}</span>", exceptionContext.Exception.Message);
            if (isAjaxRequest)
            {
                result = new FormatJsonResult()
                {
                    Data = new ResponseData
                    {
                        success = false,
                        msg = msg
                    }.Info()
                };
            }
            else
            {
                result = new ContentResult()
                {
                    Content = msg
                };
            }

            return result;
        }

        #region GetErrorForDeveloper
        private static ActionResult GetErrorForDeveloper(ExceptionContext exceptionContext
            , bool isAjaxRequest, Guid logId)
        {
            var urlHelper = new UrlHelper(exceptionContext.RequestContext, RouteTable.Routes);
            var url = urlHelper.Action(
                "Details",
                "ExceptionLog",
                new RouteValueDictionary { { "area", "Ac" } },
                "http",
                exceptionContext.RequestContext.HttpContext.Request.Url.Host) + "?id=" + logId.ToString();
            string msg = string.Format(
@"<div style='text-align:left;'>
    <b>异常:  </b>{0}<br />
    <b>异常类型:  </b>{1}<br />
    <b>Controller:  </b>{2}<br />
    <b>Action:  </b>{3}<br />
    <b>异常详细：</b>{4}</div>",
                            exceptionContext.Exception.Message,
                            exceptionContext.Exception.GetBaseException().GetType().ToString(),
                            (exceptionContext.RouteData.Values["Controller"] ?? string.Empty).ToString(),
                            exceptionContext.RouteData.Values["Action"].ToString(),
                            "<a href='" + url + "' target='_blank'>" + url + "</a>");
            if (isAjaxRequest)
            {
                return new FormatJsonResult()
                {
                    Data = new ResponseData
                    {
                        success = false,
                        msg = msg
                    }.Error()
                };
            }
            else
            {
                return new ContentResult() { Content = msg };
            }
        }
        #endregion

        #region GetErrorForNormalUser
        private static ActionResult GetErrorForNormalUser(IAcDomain host, bool isAjaxRequest)
        {
            AccountState account;
            host.SysUsers.TryGetDevAccount(host.AppSystemSet.SelfAppSystem.PrincipalId, out account);
            string name = string.Empty;
            string email = string.Empty;
            string qq = string.Empty;
            if (account != null)
            {
                name = account.Name;
                email = account.Email;
                qq = account.Qq;
            }
            string msg =
string.Format(@"出错了，系统已记录下本异常，相关人员会周期进行处理。如果本异常严重影响您的使用，<br />
请联系负责人：姓名：{0}，邮箱：{1}，QQ：{2}。", name, email, qq);
            if (isAjaxRequest)
            {
                return new FormatJsonResult()
                {
                    Data = new ResponseData
                    {
                        success = false,
                        msg = msg
                    }.Error()
                };
            }
            else
            {
                return new ContentResult() { Content = msg };
            }
        }
        #endregion

        private static string GetClientIp()
        {
            if (HttpContext.Current == null)
            {
                return "127.0.0.1";
            }
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }
        #endregion
    }
}