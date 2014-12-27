using Anycmd.Util;

namespace Anycmd.Engine.Hecp
{
    using DataContracts;
    using System;

    /// <summary>
    /// 证书
    /// </summary>
    public sealed class CredentialObject
    {
        /// <summary>
        /// 构造令牌对象
        /// </summary>
        private CredentialObject() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public CredentialObject(ICredentialData data)
        {
            this.CredentialData = data;
            if (data != null)
            {
                ClientType clientType;
                UserType userType;
                CredentialType credentialType;
                SignatureMethod signatureMethod;
                data.ClientType.TryParse(out clientType);
                data.UserType.TryParse(out userType);
                data.CredentialType.TryParse(out credentialType);
                data.SignatureMethod.TryParse(out signatureMethod);

                this.ClientType = clientType;
                this.UserType = userType;
                this.SignatureMethod = signatureMethod;
                this.ClientId = data.ClientId;
                this.CredentialType = credentialType;
                this.Password = data.Password;
                this.UserName = data.UserName;
                this.Ticks = data.Ticks;
            }
        }

        /// <summary>
        /// 原始证书
        /// </summary>
        public ICredentialData CredentialData { get; private set; }

        /// <summary>
        /// 证书类型
        /// </summary>
        public CredentialType CredentialType { get; private set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        public ClientType ClientType { get; private set; }

        /// <summary>
        /// 签名算法
        /// </summary>
        public SignatureMethod SignatureMethod { get; private set; }

        /// <summary>
        /// 客户标识，客户往往是个计算机程序。
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType { get; private set; }

        /// <summary>
        /// 用户名，用户往往是个人。用户通过ClientID标识的计算机程序发送请求。
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public Int64 Ticks { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="secretKey"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Valid(HecpRequest request, string secretKey, SignatureMethod method)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var credential = request.Credential;

            switch (method)
            {
                case SignatureMethod.Undefined:
                    return false;
                case SignatureMethod.HMAC_SHA1:
                    return credential.Password == Signature.Sign(request.ToOrignalString(request.Credential.CredentialData), secretKey);
                default:
                    return false;
            }
        }
    }
}
