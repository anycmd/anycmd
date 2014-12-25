
namespace Anycmd.DataContracts
{

    /// <summary>
    /// 证书模型
    /// </summary>
    public interface ICredentialData
    {
        /// <summary>
        /// 
        /// </summary>
        string ClientId { get; }
        /// <summary>
        /// 
        /// </summary>
        string ClientType { get; }
        /// <summary>
        /// 
        /// </summary>
        string CredentialType { get; }
        /// <summary>
        /// 
        /// </summary>
        string Password { get; }
        /// <summary>
        /// 
        /// </summary>
        string SignatureMethod { get; }
        /// <summary>
        /// 
        /// </summary>
        long Ticks { get; }
        /// <summary>
        /// 可以作为命令发起人。Action、Command、Event发起人，如果客户端本地未记录发起人则使用该字段委托中心端代为记录。
        /// 注意：中心端记录下了它收到的每一条影响实体状态的命令，但暂未开放给应用节点查询。
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// 
        /// </summary>
        string UserType { get; }
    }
}
