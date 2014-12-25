

namespace Anycmd.Extensions
{
    using System;
    using System.Diagnostics;
    using Util;

    /// <summary>
    /// 扩展<see cref="DateTime"/>类型对象的类
    /// </summary>
    public static class DateTimeExtension
    {
        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= SystemTime.MinDate) && (target <= SystemTime.MaxDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsNotValid(this DateTime target)
        {
            return (target < SystemTime.MinDate) || (target > SystemTime.MaxDate);
        }
    }
}