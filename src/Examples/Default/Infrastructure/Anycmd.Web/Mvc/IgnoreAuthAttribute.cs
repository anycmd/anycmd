using System;

namespace Anycmd.Web.Mvc
{
    /// <summary>
    /// 表示一个特性，标记该特性的Action方法将绕过身份认证。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreAuthAttribute : Attribute
    {
    }
}
