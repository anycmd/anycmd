using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read-only Actions node defined in the policy document. This class extends the 
    /// abstract base class TargetItems which defines the elements of the Resources, Actions and
    /// Subjets nodes.
    /// </summary>
    public class ActionsElement : ActionsElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates a new Actions with the specified aguments.
        /// </summary>
        /// <param name="anyItem">Whether the target item is defined for any item</param>
        /// <param name="items">The taregt items.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public ActionsElement(bool anyItem, TargetItemReadWriteCollection items, XacmlVersion version)
            : base(anyItem, items, version)
        {
        }

        /// <summary>
        /// Creates an instance of the Actions class using the XmlReader instance provided.
        /// </summary>
        /// <remarks>
        /// This constructor is also calling the base class constructor specifying the XmlReader
        /// and the node names that defines this TargetItmes extended class.
        /// </remarks>
        /// <param name="reader">The XmlReader positioned at the Actions node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public ActionsElement(XmlReader reader, XacmlVersion version)
            : base(reader, version)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates an instance of the containing element of the Actions class. This method is 
        /// called by the TargetItems base class when an element that identifies a Action is 
        /// found.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Action node.</param>
        /// <returns>A new instance of the Action class.</returns>
        protected override TargetItemBaseReadWrite CreateTargetItem(XmlReader reader)
        {
            return new ActionElement(reader, SchemaVersion);
        }

        #endregion

        #region Public properties
        /// <summary>
        /// The list of "target item"s defined in the "target item list" node.
        /// </summary>
        public override TargetItemReadWriteCollection ItemsList
        {
            get { return new TargetItemCollection(base.ItemsList); }
            set { throw new NotSupportedException(); }
        }
        #endregion
    }
}
