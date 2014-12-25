
namespace Anycmd.Engine.Host.Edi
{
    using Engine.Edi;
    using Hecp;
    using System;
    using Util;

    /// <summary>
    /// 节点身份验证器/令牌验证器
    /// </summary>
    public sealed class DefaultAuthenticator : IAuthenticator
    {
        private static readonly Guid _id = new Guid("6DD6757A-0701-415E-943E-91C5DA9A4D8D");
        private static readonly string _name = "默认的身份验证器";
        private static readonly string _description = "身份验证器";

        /// <summary>
        /// 
        /// </summary>
        public DefaultAuthenticator()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public BuiltInResourceKind BuiltInResourceKind
        {
            get { return BuiltInResourceKind.Authenticator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        #region Auth
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ProcessResult Auth(HecpRequest request)
        {
            if (request == null)
            {
                return new ProcessResult(false, Status.NoneCommand, "空请求");
            }
            CredentialObject credential = request.Credential;
            if (credential == null)
            {
                return new ProcessResult(false, Status.NoneCredential, "未传入证书对象");
            }
            else if (credential.CredentialType == CredentialType.Undefined)
            {
                return new ProcessResult(false, Status.InvalidCredentialType, "未定义的证书类型");
            }
            else if (credential.Ticks < SystemTime.UtcNow().AddYears(-1).Ticks
                || credential.Ticks > SystemTime.UtcNow().AddYears(1).Ticks)
            {
                return new ProcessResult(false, Status.InvalidTicks, "非法的时间戳:" + credential.Ticks);
            }
            var t = new DateTime(credential.Ticks, DateTimeKind.Utc);
            if (t.AddSeconds(request.Host.Config.TicksTimeout) < SystemTime.UtcNow()
                || t.AddSeconds(-request.Host.Config.TicksTimeout) > SystemTime.UtcNow())
            {
                return new ProcessResult(false, Status.NotAuthorized, "时间戳超时:" + credential.Ticks);
            }
            else
            {
                switch (credential.ClientType)
                {
                    case ClientType.Undefined:
                        return new ProcessResult(false, Status.InvalidClientType, "非法的客户端类型");
                    case ClientType.Node:
                        {
                            // 向后兼容uia的实名认证在使用的token证书类型。如果ClientID为空则从UserName字段提取ClientID
                            string clientId = credential.ClientId;
                            NodeDescriptor node;
                            if (!request.Host.NodeHost.Nodes.TryGetNodeByPublicKey(clientId, out node))
                            {
                                return new ProcessResult(false, Status.InvalidClientId, "未知的节点");
                            }
                            else if (node.Node.IsEnabled != 1)
                            {
                                return new ProcessResult(false, Status.NodeIsDisabled, "节点已被禁用");
                            }
                            else if (!node.Node.IsReceiveEnabled)
                            {
                                return new ProcessResult(false, Status.ReceiveIsDisabled, "来自本节点的请求被禁止接收");
                            }
                            if (string.IsNullOrEmpty(credential.Password))
                            {
                                return new ProcessResult(false, Status.NotAuthorized, "签名不能为空");
                            }
                            switch (credential.CredentialType)
                            {
                                case CredentialType.Undefined:
                                    return new ProcessResult(false, Status.InvalidCredentialType, "未定义的证书类型");
                                case CredentialType.Token:// 证书类型是令牌
                                    var token = TokenObject.Create(credential.Password, clientId, credential.Ticks);
                                    if (!token.IsValid(node.Node.SecretKey))
                                    {
                                        return new ProcessResult(false, Status.NotAuthorized, "节点身份未验证通过");
                                    }
                                    break;
                                case CredentialType.Signature:// 证书类型是签名
                                    if (credential.SignatureMethod == SignatureMethod.Undefined)
                                    {
                                        return new ProcessResult(false, Status.NotAuthorized, "未指定签名算法，签名算法如：" + SignatureMethod.HMAC_SHA1.ToName());
                                    }
                                    if (!CredentialObject.Valid(request, node.Node.SecretKey, credential.SignatureMethod))
                                    {
                                        return new ProcessResult(false, Status.NotAuthorized, "签名可能被篡改，数据传输对象状态有变化时需重新签名。");
                                    }
                                    break;
                                case CredentialType.OAuth:
                                    if (credential.SignatureMethod == SignatureMethod.Undefined)
                                    {
                                        return new ProcessResult(false, Status.NotAuthorized, "未指定签名算法，签名算法如：" + SignatureMethod.HMAC_SHA1.ToName());
                                    }
                                    return new ProcessResult(false, Status.NotAuthorized, "暂不支持开放授权");
                                default:
                                    return new ProcessResult(false, Status.NotAuthorized, "暂不支持" + credential.CredentialType.ToName() + "证书类型");
                            }
                            break;
                        }
                    case ClientType.App:
                        return new ProcessResult(false, Status.InvalidClientType, "暂不支持");
                    case ClientType.Monitor:
                        return new ProcessResult(false, Status.InvalidClientType, "暂不支持");
                    default:
                        break;
                }
            }

            return new ProcessResult(true, Status.Ok, "身份认证通过"); ;
        }
        #endregion
    }
}
