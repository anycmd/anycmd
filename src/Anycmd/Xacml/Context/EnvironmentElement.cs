using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents an Envirnonment node found in the context document. This class extends the abstract base class 
    /// TargetItem which loads the "target item" definition.
    /// </summary>
    public class EnvironmentElement : EnvironmentElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates an Environment using the specified arguments.
        /// </summary>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public EnvironmentElement(AttributeCollection attributes, XacmlVersion schemaVersion)
            : base(attributes, schemaVersion)
        {
        }

        /// <summary>
        /// Creates an instance of the Environment class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Environment node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public EnvironmentElement(XmlReader reader, XacmlVersion schemaVersion) :
            base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties
        /// <summary>
        /// 
        /// </summary>
        public override AttributeReadWriteCollection Attributes
        {
            get
            {
                return new AttributeCollection(base.Attributes);
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
