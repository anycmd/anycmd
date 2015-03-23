
namespace Anycmd.Web
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// 扩展<see cref="String"/>类型对象的类
    /// </summary>
    public static class StringExtension
    {
        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        #region Html

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHtmlExpression.Replace(target, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        #endregion
    }
}