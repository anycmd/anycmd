using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read-only Rule defined in the policy document.
    /// </summary>
    public class RuleElement : RuleElementReadWrite
    {
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
        public RuleElement(string id, string description, TargetElementReadWrite target, ConditionElementReadWrite condition, Effect effect, XacmlVersion schemaVersion)
            : base(id, description, target, condition, effect, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new Rule using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">THe XmlReader instance positioned at the Rule node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RuleElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The Rule Id.
        /// </summary>
        public override string Id
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The description of this rule.
        /// </summary>
        public override string Description
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The condition of this rule.
        /// </summary>
        public override ConditionElementReadWrite Condition
        {
            get {
                return base.Condition != null ? new ConditionElement(base.Condition.FunctionId, base.Condition.Arguments, this.SchemaVersion) : null;
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The target of the rule.
        /// </summary>
        public override TargetElementReadWrite Target
        {
            get
            {
                if (base.Target != null)
                    return new TargetElement(base.Target.Resources, base.Target.Subjects, base.Target.Actions,
                         base.Target.Environments, base.Target.SchemaVersion);
                else
                    return null;
            }
        }

        /// <summary>
        /// The effect of the rule.
        /// </summary>
        public override Effect Effect
        {
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
