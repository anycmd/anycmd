
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class BatchUpdatedEvent : DomainEvent
    {
        #region Ctor
        public BatchUpdatedEvent(IBatch source)
            : base(source)
        {
        }
        #endregion
    }
}
