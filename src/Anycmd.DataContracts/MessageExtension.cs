
namespace Anycmd.DataContracts
{
    using System.Text;

    public static class MessageExtension
    {
        /// <summary>
        /// 将给定的命令消息转化为原始签名字符串
        /// </summary>
        /// <param name="message">命令消息</param>
        /// <param name="credential"></param>
        /// <returns></returns>
        public static string ToOrignalString(this IMessageDto message, ICredentialData credential)
        {
            if (message == null)
            {
                return string.Empty;
            }
            if (credential == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            // 证书
            sb.Append("CredentialType=").Append(credential.CredentialType);
            sb.Append("&SignatureMethod=").Append(credential.SignatureMethod);
            sb.Append("&ClientId=").Append(credential.ClientId);
            sb.Append("&ClientType=").Append(credential.ClientType);
            sb.Append("&UserName=").Append(credential.UserName);
            sb.Append("&UserType=").Append(credential.UserType);
            sb.Append("&Ticks=").Append(credential.Ticks);
            // 命令
            sb.Append("&Version=").Append(message.Version);
            sb.Append("&MessageId=").Append(message.MessageId);
            sb.Append("&MessageType=").Append(message.MessageType);
            sb.Append("&Verb=").Append(message.Verb);
            sb.Append("&Ontology=").Append(message.Ontology);
            if (message.Body != null)
            {
                sb.Append("&InfoId=");
                if (message.Body.InfoId != null)
                {
                    foreach (var item in message.Body.InfoId)
                    {
                        if (item != null)
                        {
                            sb.Append("&").Append("InfoId_").Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                sb.Append("&InfoValue=");
                if (message.Body.InfoValue != null)
                {
                    foreach (var item in message.Body.InfoValue)
                    {
                        if (item != null)
                        {
                            sb.Append("&").Append("InfoValue_").Append(item.Key).Append("=").Append(item.Value);
                        }
                    }
                }
                if (message.Body.QueryList != null)
                {
                    string resultItem = string.Empty;
                    int l = resultItem.Length;
                    foreach (var item in message.Body.QueryList)
                    {
                        if (resultItem.Length != l)
                        {
                            resultItem += ",";
                        }
                        resultItem += item;
                    }
                }
                if (message.Body.Event != null)
                {
                    sb.Append("&Event_SourceType=").Append(message.Body.Event.SourceType);
                    sb.Append("&Event_Subject=").Append(message.Body.Event.Subject);
                    sb.Append("&Event_Status=").Append(message.Body.Event.Status);
                    sb.Append("&Event_ReasonPhrase=").Append(message.Body.Event.ReasonPhrase);
                }
            }
            sb.Append("&TimeStamp=").Append(message.TimeStamp);
            sb.Append("&IsDumb=").Append(message.IsDumb);

            // 忽略大小写
            return sb.ToString().ToLower();
        }
    }
}
