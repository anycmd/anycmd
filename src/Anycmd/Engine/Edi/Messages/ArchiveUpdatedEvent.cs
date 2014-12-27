
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ArchiveUpdatedEvent : DomainEvent
    {
        #region Ctor
        public ArchiveUpdatedEvent(ArchiveBase source)
            : base(source)
        {
            this.DataSource = source.DataSource;
            this.FilePath = source.FilePath;
            this.NumberId = source.NumberId;
            this.UserId = source.UserId;
            this.Password = source.Password;
        }
        #endregion

        /// <summary>
        /// 源
        /// </summary>
        public string DataSource { get; private set; }
        /// <summary>
        /// 归档库路径
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// 数字标识
        /// </summary>
        public int NumberId { get; private set; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }
    }
}
