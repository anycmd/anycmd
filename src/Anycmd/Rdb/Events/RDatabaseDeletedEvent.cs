using Anycmd.Events;

namespace Anycmd.Rdb.Events
{
    public class RDatabaseDeletedEvent : DomainEvent
    {
        #region Ctor
        public RDatabaseDeletedEvent(RDatabase source)
            : base(source)
        {
        }
        #endregion
    }
}