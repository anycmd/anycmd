
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Info;
    using Model;
    using System;

    /// <summary>
    /// 命令抽象基类。命令实体不提供无参构造函数，不可序列化。
    /// </summary>
    public class MessageEntity : MessageBase, IEntity
    {
        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataTuple">数据项集合对</param>
        /// <param name="id">信息标识</param>
        public MessageEntity(MessageTypeKind type, Guid id, DataItemsTuple dataTuple)
            : base(type, id, dataTuple)
        {
        }
        #endregion
    }
}
