using Anycmd.Events;

namespace Anycmd.Rdb.Events
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