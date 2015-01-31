

namespace Anycmd.Edi.ViewModels.MessageViewModels
{
    using Engine.Edi;
    using Engine.Host.Edi.Handlers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 承载命令提供程序扩展方法的静态类
    /// </summary>
    public static class MessageProviderExetension
    {
        /// <summary>
        /// 分页查询命令展示对象
        /// </summary>
        /// <typeparam name="T">命令展示模型类型参数</typeparam>
        /// <param name="messageProvider">命令提供程序</param>
        /// <param name="messageTypeKind"></param>
        /// <param name="ontology">本体</param>
        /// <param name="catalogCode">目录码</param>
        /// <param name="actionCode">动作码，空值表示忽略本查询条件</param>
        /// <param name="nodeId">节点标识，空值表示忽略本查询条件</param>
        /// <param name="localEntityId"></param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IList<MessageTr> GetPlistCommandTrs(
            this IMessageProvider messageProvider, MessageTypeKind messageTypeKind, OntologyDescriptor ontology, string catalogCode, string actionCode
            , Guid? nodeId, string localEntityId, int pageIndex, int pageSize
            , string sortField, string sortOrder, out Int64 total)
        {
            IList<MessageEntity> commands = messageProvider.GetPlistCommands(messageTypeKind,
                    ontology, catalogCode, actionCode, nodeId, localEntityId, pageIndex, pageSize
                    , sortField, sortOrder, out total);
            IList<MessageTr> list = new List<MessageTr>();
            foreach (var command in commands)
            {
                list.Add(MessageTr.Create(ontology.Host, command));
            }

            return list;
        }

        /// <summary>
        /// 根据Id查询命令详细信息
        /// </summary>
        /// <typeparam name="T">命令详细信息展示模型类型参数</typeparam>
        /// <param name="messageProvider">命令提供程序</param>
        /// <param name="messageTypeKind"></param>
        /// <param name="ontology">本体</param>
        /// <param name="id">命令标识</param>
        /// <returns></returns>
        public static MessageTr GetCommandInfo(
            this IMessageProvider messageProvider, MessageTypeKind messageTypeKind, OntologyDescriptor ontology, Guid id)
        {
            var command = messageProvider.GetCommand(messageTypeKind, ontology, id);
            return MessageTr.Create(ontology.Host, command);
        }
    }
}
