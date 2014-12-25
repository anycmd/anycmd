using Anycmd.Events;

namespace Anycmd.Rdb.Events
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