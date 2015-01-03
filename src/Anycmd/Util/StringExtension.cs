
namespace Anycmd.Util
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 扩展<see cref="String"/>类型对象的类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string SafeToLower(this string target)
        {
            return string.IsNullOrEmpty(target) ? target : target.ToLower();
        }

        [DebuggerStepThrough]
        public static string SafeTrim(this string target)
        {
            return target == null ? null : target.Trim();
        }

        public static string FormatWith(this string text, params object[] args)
        {
            return String.Format(text, args);
        }

        public static string Fmt(this string text, params object[] args)
        {
            return String.Format(text, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="str"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string AppendIf(this string s, string str, bool predicate)
        {
            if (predicate)
            {
                return s + str;
            }
            return s;
        }

        public static string AppendIf(this string s, Func<string> str, bool predicate)
        {
            if (predicate)
            {
                return s + str();
            }
            return s;
        }
    }
}