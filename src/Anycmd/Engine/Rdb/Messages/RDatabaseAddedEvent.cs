using Anycmd.Events;

namespace Anycmd.Engine.Rdb.Messages
{
    public class RDatabaseAddedEvent : DomainEvent
    {
        #region Ctor
        public RDatabaseAddedEvent(RDatabase source)
            : base(source)
        {
        }
        #endregion
    }
}