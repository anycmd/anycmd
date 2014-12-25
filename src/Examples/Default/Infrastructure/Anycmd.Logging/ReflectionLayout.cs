
namespace Anycmd.Logging
{
    /// <summary>
    /// <see cref="log4net.Layout.PatternLayout"/>
    /// </summary>
    public class ReflectionLayout : log4net.Layout.PatternLayout
    {
        /// <summary>
        /// 
        /// </summary>
        public ReflectionLayout()
        {
            this.AddConverter("property", typeof(ReflectionPatternConverter));
        }
    }
}
