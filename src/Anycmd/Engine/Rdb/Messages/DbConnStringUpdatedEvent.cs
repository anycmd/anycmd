
namespace Anycmd.Engine.Rdb.Messages
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DbConnStringUpdatedEvent : DomainEvent
    {
        #region Ctor
        public DbConnStringUpdatedEvent(RDatabase source)
            : base(source)
        {
            this.CatalogName = source.CatalogName;
            this.DataSource = source.DataSource;
            this.Profile = source.Profile;
            this.UserId = source.UserId;
            this.Password = source.Password;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public string CatalogName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataSource { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Profile { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; private set; }
    }
}
