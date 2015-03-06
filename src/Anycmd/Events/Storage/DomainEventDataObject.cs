
namespace Anycmd.Events.Storage
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// 数据转移对象（DTO）。表示保持领域事件的数据的数据对象。
    /// </summary>
    /// <remarks>The <c>DomainEventDataObject</c> class implemented the Data Transfer Object pattern
    /// that was described in Martin Fowler's book, Patterns of Enterprise Application Architecture.
    /// For more information about Data Transfer Object pattern, please refer to http://martinfowler.com/eaaCatalog/dataTransferObject.html.
    /// </remarks>
    [Serializable]
    [XmlRoot]
    [DataContract]
    public class DomainEventDataObject
    {
        #region Ctor
        /// <summary>
        /// 初始化一个领域事件数据传输对象实例。
        /// </summary>
        public DomainEventDataObject()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 读取或设置一个存储当前领域事件对象的数据的字节数组 <see cref="System.Byte"/> 。
        /// </summary>
        [XmlElement]
        [DataMember]
        public byte[] Data { get; set; }
        /// <summary>
        /// 读取或设置当前领域事件对象的程序集级类型唯一名称。
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AssemblyQualifiedEventType
        {
            get;
            set;
        }
        /// <summary>
        /// 读取或设置该领域事件所在的分支。
        /// </summary>
        [XmlElement]
        [DataMember]
        public long Branch
        {
            get;
            set;
        }
        /// <summary>
        /// 读取或设置领域事件标识。
        /// </summary>
        /// <remarks>Note that since the <c>DomainEventDataObject</c> is the data
        /// presentation of the corresponding domain event object, this identifier value
        /// can also be considered to be the identifier for the <c>DomainEventDataObject</c> instance.</remarks>
        [XmlElement]
        [DataMember]
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the identifier of the aggregate root which holds the instance
        /// of the current domain event.
        /// </summary>
        [XmlElement]
        [DataMember]
        public Guid SourceId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the assembly qualified name of the type of the aggregate root.
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AssemblyQualifiedSourceType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the date and time on which the event was produced.
        /// </summary>
        /// <remarks>The format of this date/time value could be various between different
        /// systems. anycmd recommend system designer or architect uses the standard
        /// UTC date/time format.</remarks>
        [XmlElement]
        [DataMember]
        public DateTime Timestamp
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the version of the domain event data object.
        /// </summary>
        [XmlElement]
        [DataMember]
        public long Version
        {
            get;
            set;
        }
        #endregion
    }
}
