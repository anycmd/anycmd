
namespace Anycmd.Logging
{
    using Model;
    using System;

    /// <summary>
    /// 系统来访日志基类.
    /// </summary>
    public abstract class VisitingLogBase : EntityObject
    {
        /// <summary>
        /// 账户表示
        /// </summary>
        public virtual Guid? AccountId { get; set; }
        /// <summary>
        /// Gets or sets 登录名.
        /// </summary>
        /// <value>登录名.</value>
        public virtual string LoginName { get; set; }
        /// <summary>
        /// Gets or sets the visit on.
        /// </summary>
        /// <value>The visit on.</value>
        public virtual DateTime VisitOn { get; set; }
        /// <summary>
        /// Gets or sets the visited on.
        /// </summary>
        /// <value>The visited on.</value>
        public virtual DateTime? VisitedOn { get; set; }
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public virtual string IpAddress { get; set; }
        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        /// <value>The state code.</value>
        public virtual int StateCode { get; set; }
        /// <summary>
        /// Gets or sets the reason phrase.
        /// </summary>
        /// <value>The reason phrase.</value>
        public virtual string ReasonPhrase { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description { get; set; }
    }
}
