using System;
using System.Xml;

namespace Anycmd.Xacml
{
    /// <summary>
    /// 所有xacml元素的基类型。
    /// </summary>
    public abstract class XacmlElement
    {
        #region Private members

        private readonly XacmlVersion _schemaVersion;
        private readonly XacmlSchema _schema;

        #endregion

        #region Constructor

        /// <summary>
        /// 默认初始化器。
        /// </summary>
        /// <param name="schema">用来验证文档的枚举</param>
        /// <param name="schemaVersion">用于加载xacml元素的模式的版本</param>
        protected XacmlElement(XacmlSchema schema, XacmlVersion schemaVersion)
        {
            _schema = schema;
            _schemaVersion = schemaVersion;
        }

        /// <summary>
        /// 空构造器。
        /// </summary>
        protected XacmlElement()
        {
        }
        #endregion

        #region Public properties

        /// <summary>
        /// 用来验证文档的枚举。
        /// </summary>
        public XacmlVersion SchemaVersion
        {
            get { return _schemaVersion; }
        }

        /// <summary>
        /// 定义了xacml元素的模式（Schema）。
        /// </summary>
        public XacmlSchema Schema
        {
            get { return _schema; }
        }

        /// <summary>
        /// 查看当前实例是否是只读版本。
        /// </summary>
        public abstract bool IsReadOnly
        {
            get;
        }

        /// <summary>
        /// 返回根据当前模式和版本号指定的命名空间字符串。
        /// </summary>
        internal protected string XmlDocumentSchema
        {
            get
            {
                switch (_schema)
                {
                    case XacmlSchema.Context:
                        switch (this.SchemaVersion)
                        {
                            case XacmlVersion.Version11:
                            case XacmlVersion.Version10:
                                return Consts.Schema1.Namespaces.Context;
                                break;
                            case XacmlVersion.Version20:
                                return Consts.Schema2.Namespaces.Context;
                                break;
                            default:
                                throw new EvaluationException("意外的版本号" + SchemaVersion);
                        }
                        break;
                    case XacmlSchema.Policy:
                        switch (this.SchemaVersion)
                        {
                            case XacmlVersion.Version11:
                            case XacmlVersion.Version10:
                                return Consts.Schema1.Namespaces.Policy;
                                break;
                            case XacmlVersion.Version20:
                                return Consts.Schema2.Namespaces.Policy;
                                break;
                            default:
                                throw new EvaluationException("意外的版本号" + SchemaVersion);
                        }
                        break;
                    default:
                        throw new EvaluationException("意外的模式" + _schema);
                }
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 根据给定的版本号验证给定的模式。
        /// </summary>
        /// <param name="reader">用于读取命名空间</param>
        /// <param name="version">用于验证版本号</param>
        /// <returns><c>true</c>, 如果命名空间正确</returns>
        protected bool ValidateSchema(XmlReader reader, XacmlVersion version)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            switch (_schema)
            {
                case XacmlSchema.Policy:
                    switch (version)
                    {
                        case XacmlVersion.Version10:
                        case XacmlVersion.Version11:
                            return (reader.NamespaceURI == Consts.Schema1.Namespaces.Policy);
                        case XacmlVersion.Version20:
                            return (reader.NamespaceURI == Consts.Schema2.Namespaces.Policy);
                        default:
                            throw new EvaluationException("意外的版本号" + version);
                    }
                    break;
                case XacmlSchema.Context:
                    switch (version)
                    {
                        case XacmlVersion.Version10:
                        case XacmlVersion.Version11:
                            return (reader.NamespaceURI == Consts.Schema1.Namespaces.Context);
                        case XacmlVersion.Version20:
                            return (reader.NamespaceURI == Consts.Schema2.Namespaces.Context);
                        default:
                            throw new EvaluationException("意外的版本号" + version);
                    }
                    break;
                default:
                    throw new EvaluationException("意外的模式" + _schema);
            }
        }

        #endregion
    }
}
