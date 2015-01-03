
namespace Anycmd.Util
{
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public static class StringBuilderExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static StringBuilder Append(this StringBuilder sb, string value, bool predicate)
        {
            if (predicate)
            {
                sb.Append(value);
            }

            return sb;
        }
    }
}
