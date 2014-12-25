
namespace Anycmd.Engine.Host.Info
{
    using Model;

    /// <summary>
    /// 信息项验证器。信息项验证器插件库
    /// </summary>
    public abstract class InfoRuleEntityBase : EntityBase
    {
        protected InfoRuleEntityBase()
        {
        }

        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
