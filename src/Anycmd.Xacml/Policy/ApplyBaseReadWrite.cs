using System;
using System.Xml;
using Anycmd.Xacml.Policy.TargetItems;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// The ApplyBase class is used to define the common data used in the Apply and Condition read/write nodes.
    /// </summary>
    public abstract class ApplyBaseReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The id of the function that will be executed in this node.
        /// </summary>
        private string _functionId = string.Empty;

        /// <summary>
        /// All the arguments that will be pased to the function.
        /// </summary>
        private ExpressionReadWriteCollection _arguments = new ExpressionReadWriteCollection();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a ConditionElement with the given parameters
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="arguments"></param>
        /// <param name="schemaVersion"></param>
        protected ApplyBaseReadWrite(string functionId, ExpressionReadWriteCollection arguments, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _functionId = functionId;
            _arguments = arguments;
        }

        /// <summary>
        /// Creates an instance of the ApplyBase using the XmlReader positioned in the node and the node name
        /// specifyed by the derived class in the constructor.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the "nodeName" node.</param>
        /// <param name="nodeName">The name of the node specifies by the derived class.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected ApplyBaseReadWrite(XmlReader reader, string nodeName, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.LocalName == nodeName &&
                    ValidateSchema(reader, schemaVersion))
            {
                // Get the id of function. It will be resolved in evaluation time.
                _functionId = reader.GetAttribute(Consts.Schema1.ConditionElement.FunctionId);

                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema1.ApplyElement.Apply:
                            // Must validate if the Apply node is not an EndElement because there is a child node
                            // with the same name as the parent node.
                            if (!reader.IsEmptyElement && reader.NodeType != XmlNodeType.EndElement)
                            {
                                _arguments.Add(new ApplyElement(reader, schemaVersion));
                            }
                            break;
                        case Consts.Schema1.FunctionElement.Function:
                            _arguments.Add(new FunctionElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.Schema1.AttributeValueElement.AttributeValue:
                            _arguments.Add(new AttributeValueElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.Schema1.SubjectAttributeDesignatorElement.SubjectAttributeDesignator:
                            _arguments.Add(new SubjectAttributeDesignatorElement(reader, schemaVersion));
                            break;
                        case Consts.Schema1.ResourceAttributeDesignatorElement.ResourceAttributeDesignator:
                            _arguments.Add(new ResourceAttributeDesignatorElement(reader, schemaVersion));
                            break;
                        case Consts.Schema1.ActionAttributeDesignatorElement.ActionAttributeDesignator:
                            _arguments.Add(new ActionAttributeDesignatorElement(reader, schemaVersion));
                            break;
                        case Consts.Schema1.EnvironmentAttributeDesignatorElement.EnvironmentAttributeDesignator:
                            _arguments.Add(new EnvironmentAttributeDesignatorElement(reader, schemaVersion));
                            break;
                        case Consts.Schema1.AttributeSelectorElement.AttributeSelector:
                            _arguments.Add(new AttributeSelectorElement(reader, schemaVersion));
                            break;
                        case Consts.Schema2.VariableReferenceElement.VariableReference:
                            _arguments.Add(new VariableReferenceElement(reader, schemaVersion));
                            break;
                    }
                    if (reader.LocalName == nodeName &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        reader.Read();
                        break;
                    }
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The id of the function used in this element.
        /// </summary>
        public virtual string FunctionId
        {
            set { _functionId = value; }
            get { return _functionId; }
        }

        /// <summary>
        /// The arguments of the condition (or apply)
        /// </summary>
        public virtual ExpressionReadWriteCollection Arguments
        {
            set { _arguments = value; }
            get { return _arguments; }
        }
        #endregion

        #region Abstract methods

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public abstract void WriteDocument(XmlWriter writer);

        #endregion
    }
}
