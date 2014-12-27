
namespace Anycmd.Engine.Hecp
{
    using System.ComponentModel;

    /// <summary>
    /// 证书类型
    /// </summary>
    public enum CredentialType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义的证书类型")]
        Undefined = 0,
        /// <summary>
        /// 令牌
        /// </summary>
        [Description("令牌")]
        Token = 1,
        /// <summary>
        /// 签名
        /// </summary>
        [Description("签名")]
        Signature = 2,
        /// <summary>
        /// 开放授权
        /// </summary>
        [Description("开放授权")]
        OAuth = 3
    }
}
