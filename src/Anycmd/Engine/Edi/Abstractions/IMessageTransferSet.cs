
namespace Anycmd.Engine.Edi.Abstractions
{
    using Host.Edi.Handlers;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 命令命令转移器集合
    /// </summary>
    public interface IMessageTransferSet : IEnumerable<IMessageTransfer>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId"></param>
        /// <returns></returns>
        IMessageTransfer this[Guid transferId] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId"></param>
        /// <param name="sendStrategy"></param>
        /// <returns></returns>
        bool TryGetTransfer(Guid transferId, out IMessageTransfer sendStrategy);
    }
}
