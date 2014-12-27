
namespace Anycmd.Engine.Hecp
{
    using System.ComponentModel;

    /// <summary>
    /// 接口版本号
    /// </summary>
    public enum ApiVersion : byte
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义的版本号")]
        Undefined = 0,
        /// <summary>
        /// 版本1
        /// </summary>
        [Description("V1版本")]
        V1 = 1
    }
}
