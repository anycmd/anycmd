
namespace Anycmd.Engine.Host.Edi
{
    using Hecp;

    /// <summary>
    /// 节点身份验证器/令牌验证器
    /// </summary>
    public interface IAuthenticator : IWfResource
    {
        /// <summary>
        /// 认证给定的Edi请求
        /// </summary>
        /// <param name="request">Edi请求消息</param>
        /// <returns></returns>
        ProcessResult Auth(HecpRequest request);
    }
}
