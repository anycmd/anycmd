
namespace Anycmd.Edi.MessageServices
{
    using DataContracts;
    using Engine.Hecp;
    using ServiceModel.Operations;
    using System;

    /// <summary>
    /// 提供<see cref="HecpResponse"/>转化为<see cref="Message"/>的扩展方法。
    /// </summary>
    public static class HecpResponseExtension
    {
        /// <summary>
        /// 转化为命令请求。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static Message ToMessage(this HecpResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            var r = new Message
            {
                MessageType = response.MessageType,
                MessageId = response.MessageId,
                Verb = response.Verb,
                Credential = ((IMessageDto)response).Credential,
                Body = response.Body,
                IsDumb = response.IsDumb,
                Ontology = response.Ontology,
                TimeStamp = response.TimeStamp,
                Version = response.Version
            };

            return r;
        }
    }
}
