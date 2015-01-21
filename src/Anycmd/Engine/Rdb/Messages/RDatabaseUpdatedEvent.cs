using Anycmd.Events;

namespace Anycmd.Engine.Rdb.Messages
{
    public class RDatabaseUpdatedEvent : DomainEvent
    {
        #region Ctor
        public RDatabaseUpdatedEvent(RDatabase source)
            : base(source)
        {
        }
        #endregion
    }
}