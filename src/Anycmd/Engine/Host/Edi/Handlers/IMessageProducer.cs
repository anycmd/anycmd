
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;
    using System.Collections.Generic;

    /// <summary>
    /// 命令生产者
    /// </summary>
    public interface IMessageProducer : IWfResource
    {
        /// <summary>
        /// 根据给定的命令和上下文节点登记的关心元素、权限创建待分发命令。
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns></returns>
        IList<MessageEntity> Produce(MessageTuple tuple);

        /// <summary>
        /// 面向给定的节点构建待分发命令。
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="toNode"></param>
        /// <returns></returns>
        IList<MessageEntity> Produce(MessageTuple tuple, NodeDescriptor toNode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="toNode"></param>
        /// <returns></returns>
        bool IsProduce(MessageContext context, NodeDescriptor toNode);
    }
}
