
namespace Anycmd.Events
{
    using Model;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Util;

    /// <summary>
    /// 表示所有领域事件的基类。
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        private DateTime _timestamp;

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>DomainEvent</c> class.
        /// </summary>
        protected DomainEvent()
        {
            this._timestamp = DateTime.Now;
        }
        /// <summary>
        /// Initializes a new instace of <c>DomainEvent</c> class.
        /// </summary>
        /// <param name="source">The source entity which raises the domain event.</param>
        protected DomainEvent(IEntity source)
            : this()
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            this.Source = source;
        }

        protected DomainEvent(IUserSession userSession, IEntity source)
            : this(source)
        {
            this.UserSession = userSession;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the hash code for current domain event.
        /// </summary>
        /// <returns>The calculated hash code for the current domain event.</returns>
        public override int GetHashCode()
        {
            return ReflectionHelper.GetHashCode(this.Source.GetHashCode(),
                this.Branch.GetHashCode(),
                this.Id.GetHashCode(),
                this.Timestamp.GetHashCode(),
                this.Version.GetHashCode());
        }
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value indicating whether this instance is equal to a specified
        /// entity.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of the <see cref="ISourcedAggregateRoot"/> type and equals the value of this
        /// instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as DomainEvent;
            if ((object)other == (object)null)
                return false;
            return this.Id == other.Id;
        }
        #endregion

        [XmlIgnore]
        [SoapIgnore]
        [IgnoreDataMember]
        public IUserSession UserSession { get; private set; }

        #region IDomainEvent Members
        /// <summary>
        /// Gets or sets the source entity from which the domain event was generated.
        /// </summary>
        [XmlIgnore]
        [SoapIgnore]
        [IgnoreDataMember]
        public IEntity Source { get; internal set; }
        /// <summary>
        /// Gets or sets the version of the domain event.
        /// </summary>
        public virtual long Version { get; internal set; }
        /// <summary>
        /// Gets or sets the branch on which the current version of domain event exists.
        /// </summary>
        public virtual long Branch { get; internal set; }
        /// <summary>
        /// Gets or sets the assembly qualified type name of the event.
        /// </summary>
        public virtual string AssemblyQualifiedEventType { get; internal set; }
        #endregion

        #region IEvent Members

        /// <summary>
        /// Gets or sets the date and time on which the event was produced.
        /// </summary>
        /// <remarks>The format of this date/time value could be various between different
        /// systems. anycmd recommend system designer or architect uses the standard
        /// UTC date/time format.</remarks>
        public virtual DateTime Timestamp
        {
            get { return _timestamp; }
            internal set { _timestamp = value; }
        }
        #endregion

        #region IEntity Members
        /// <summary>
        /// Gets or sets the identifier of the domain event.
        /// </summary>
        public virtual Guid Id { get; internal set; }
        #endregion
    }
}
