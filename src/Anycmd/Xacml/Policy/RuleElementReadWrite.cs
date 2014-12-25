using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write Rule defined in the policy document.
    /// </summary>
    public class RuleElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The Rule Id.
        /// </summary>
        private string _id = String.Empty;

        /// <summary>
        /// The rule description.
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// The condition that defines the rule.
        /// </summary>
        private ConditionElementReadWrite _condition;

        /// <summary>
        /// The effect of the Rule
        /// </summary>
        private Effect _effect;

        /// <summary>
        /// The target that must be satisfied in order to apply the rule.
        /// </summary>
        private TargetElementReadWrite _target;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Rule using the provided argument values.
        /// </summary>
        /// <param name="id">The rule id.</param>
        /// <param name="description">The rule description.</param>
        /// <param name="target">The target instance for this rule.</param>
        /// <param name="condition">The condition for this rule.</param>
        /// <param name="effect">The effect of this rule.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RuleElementReadWrite(string id, string description, TargetElementReadWrite target, ConditionElementReadWrite condition, Effect effect, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _id = id;
            _description = description;
            _target = target;
            _condition = condition;
            _effect = effect;
        }

        /// <summary>
        /// Creates a new Rule using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">THe XmlReader instance positioned at the Rule node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RuleElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema1.RuleElement.Rule &&
                ValidateSchema(reader, schemaVersion))
            {
                // Read the attributes
                _id = reader.GetAttribute(Consts.Schema1.RuleElement.RuleId);

                // The parsing should not fail because the document have been validated by an
                // Xsd schema.
                string temp = reader.GetAttribute(Consts.Schema1.RuleElement.Effect);
                _effect = (Effect)Enum.Parse(
                    typeof(Effect),
                    temp,
                    false);

                // Read the rule contents.
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema1.RuleElement.Description:
                            _description = reader.ReadElementString();
                            break;
                        case Consts.Schema1.RuleElement.Target:
                            _target = new TargetElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.Schema1.RuleElement.Condition:
                            _condition = new ConditionElementReadWrite(reader, schemaVersion);
                            break;
                    }
                    if (reader.LocalName == Consts.Schema1.RuleElement.Rule &&
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
        /// The Rule Id.
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The description of this rule.
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The condition of this rule.
        /// </summary>
        public virtual ConditionElementReadWrite Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        /// <summary>
        /// Whether the rule defines a condition.
        /// </summary>
        public bool HasCondition
        {
            get { return _condition != null; }
        }

        /// <summary>
        /// Whether the rule defines a target.
        /// </summary>
        public bool HasTarget
        {
            get { return _target != null; }
        }

        /// <summary>
        /// The target of the rule.
        /// </summary>
        public virtual TargetElementReadWrite Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// The effect of the rule.
        /// </summary>
        public virtual Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}
