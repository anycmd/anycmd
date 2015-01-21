using Anycmd.Events;

namespace Anycmd.Engine.Rdb.Messages
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