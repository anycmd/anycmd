
namespace Anycmd.DataContracts
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 证书。证书对象在结构上有CredentialType、ClientType、UserName、Password、Ticks五个属性，它们用于验证节点身份。
    /// </summary>
    [DataContract(Namespace = Consts.Namespace)]
    public sealed class CredentialData : ICredentialData
    {
        /// <summary>
        /// 构造令牌对象
        /// </summary>
        public CredentialData() { }

        /// <summary>
        /// 证书类型
        /// </summary>
        [DataMember]
        public string CredentialType { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        [DataMember]
        public string ClientType { get; set; }

        /// <summary>
        /// 签名算法
        /// </summary>
        [DataMember]
        public string SignatureMethod { get; set; }

        /// <summary>
        /// 客户标识，客户往往是个计算机程序。
        /// </summary>
        [DataMember]
        public string ClientId { get; set; }

        /// <summary>
        /// 用户类型。默认为entitySelf
        /// </summary>
        [DataMember]
        public string UserType { get; set; }

        /// <summary>
        /// 用户名，用户往往是个人。用户通过ClientId标识的计算机程序发送请求。
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// 时间戳。该时间戳参与令牌字符串的计算。
        /// </summary>
        [DataMember]
        public Int64 Ticks { get; set; }
    }
}
