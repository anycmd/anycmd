using System;
using System.Xml;
using Anycmd.Xacml.Policy.TargetItems;

namespace Anycmd.Xacml.Policy
{
    using Interfaces;

    /// <summary>
    /// Represents a RuleCombinerParameter defined in the policy document.
    /// </summary>
    public class VariableDefinitionElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The parameter name.
        /// </summary>
        private readonly string _id;

        /// <summary>
        /// The expression for the variable definition.
        /// </summary>
        private readonly IExpression _expression;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new RuleCombinerParameter using the provided argument values.
        /// </summary>
        /// <param name="id">The variable id.</param>
        /// <param name="expression">The expression for the variable definition.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public VariableDefinitionElement(string id, IExpression expression, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _id = id;
            _expression = expression;
        }

        /// <summary>
        /// Creates a new RuleCombinerParameter using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public VariableDefinitionElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader.LocalName == Consts.Schema2.VariableDefinitionElement.VariableDefinition &&
                ValidateSchema(reader, schemaVersion))
            {
                // Read the attributes
                if (reader.HasAttributes)
                {
                    // Load all the attributes
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.LocalName == Consts.Schema2.VariableDefinitionElement.VariableId)
                        {
                            _id = reader.GetAttribute(Consts.Schema2.VariableDefinitionElement.VariableId);
                        }
                    }
                    reader.MoveToElement();
                }

                // Read the rule contents.
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema1.AttributeSelectorElement.AttributeSelector:
                            _expression = new AttributeSelectorElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.SubjectAttributeDesignatorElement.SubjectAttributeDesignator:
                            _expression = new SubjectAttributeDesignatorElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.ActionAttributeDesignatorElement.ActionAttributeDesignator:
                            _expression = new ActionAttributeDesignatorElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.ResourceAttributeDesignatorElement.ResourceAttributeDesignator:
                            _expression = new ResourceAttributeDesignatorElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.EnvironmentAttributeDesignatorElement.EnvironmentAttributeDesignator:
                            _expression = new EnvironmentAttributeDesignatorElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.AttributeValueElement.AttributeValue:
                            _expression = new AttributeValueElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.Schema1.FunctionElement.Function:
                            _expression = new FunctionElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.Schema1.ApplyElement.Apply:
                            _expression = new ApplyElement(reader, schemaVersion);
                            break;
                        case Consts.Schema2.VariableReferenceElement.VariableReference:
                            _expression = new VariableReferenceElement(reader, schemaVersion);
                            break;
                    }
                    if (reader.LocalName == Consts.Schema2.VariableDefinitionElement.VariableDefinition &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }

        }

        #endregion

        #region Public properties

        /// <summary>
        /// The variable id
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Returns the expression definition.
        /// </summary>
        public IExpression Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return true; }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            writer.WriteStartElement(Consts.Schema2.VariableDefinitionElement.VariableDefinition);
            writer.WriteAttributeString(Consts.Schema2.VariableDefinitionElement.VariableId, this._id);

            this._expression.WriteDocument(writer);

            writer.WriteEndElement();
        }

        #endregion

    }
}
