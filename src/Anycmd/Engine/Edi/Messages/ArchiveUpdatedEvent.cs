
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ArchiveUpdatedEvent : DomainEvent
    {
        public ArchiveUpdatedEvent(IAcSession acSession, ArchiveBase source)
            : base(acSession, source)
        {
            this.DataSource = source.DataSource;
            this.FilePath = source.FilePath;
            this.NumberId = source.NumberId;
            this.UserId = source.UserId;
            this.Password = source.Password;
        }

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
