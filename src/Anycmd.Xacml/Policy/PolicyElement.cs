using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only policy node defined in the Policy document.
	/// </summary>
	public class PolicyElement : PolicyElementReadWrite
	{

		#region Constructors

		/// <summary>
		/// Creates a new Policy with the specified arguments.
		/// </summary>
		/// <param name="id">The policy id.</param>
		/// <param name="description">THe policy description.</param>
		/// <param name="target">THe policy target.</param>
		/// <param name="rules">THe rules for this policy.</param>
		/// <param name="ruleCombiningAlgorithm">THe rule combining algorithm.</param>
		/// <param name="obligations">The Obligations for this policy.</param>
		/// <param name="xpathVersion">The XPath version supported.</param>
		/// <param name="combinerParameters">The combiner parameters in this policy.</param>
		/// <param name="ruleCombinerParameters">The rule parameters in this policy.</param>
		/// <param name="variableDefinitions">The variable definitions of this policy.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public PolicyElement( string id, string description, TargetElementReadWrite target, RuleReadWriteCollection rules, 
			string ruleCombiningAlgorithm, ObligationReadWriteCollection obligations, string xpathVersion, ArrayList combinerParameters, 
			ArrayList ruleCombinerParameters, IDictionary variableDefinitions, XacmlVersion schemaVersion )
			: base( id, description, target, rules, ruleCombiningAlgorithm, obligations, xpathVersion, 
			combinerParameters, ruleCombinerParameters, variableDefinitions, schemaVersion )
		{
		}

		/// <summary>
		/// Creates a new Policy using the XmlReader instance specified.
		/// </summary>
		/// <param name="reader">The XmlReader instance positioned at the Policy node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public PolicyElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets the Id of the policy.
		/// </summary>
		public override string Id
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the description of the policy.
		/// </summary>
		public override string Description
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the rule combining algorithm name
		/// </summary>
		public override string RuleCombiningAlgorithm
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the list of rules.
		/// </summary>
		public override RuleReadWriteCollection Rules
		{
			get{ return new RuleCollection( base.Rules ); }
			set{ throw new NotSupportedException(); }
		}
		/// <summary>
		/// Gets the target instance.
		/// </summary>
		public override TargetElementReadWrite Target
		{
			get{ return new TargetElement(base.Target.Resources, base.Target.Subjects, base.Target.Actions,
					 base.Target.Environments, base.Target.SchemaVersion); 
			}
		}
		/// <summary>
		/// Gets the list of obligations.
		/// </summary>
		public override ObligationReadWriteCollection Obligations
		{
			get{ return new ObligationCollection( base.Obligations ); }
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The XPath version supported.
		/// </summary>
		public override string XPathVersion
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The variable definitions.
		/// </summary>
		public override IDictionary VariableDefinitions
		{
			set{ throw new NotSupportedException(); }
		}
		/// <summary>
		/// The combiner parameters
		/// </summary>
		public override ArrayList CombinerParameters
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The rule combiner parameters
		/// </summary>
		public override ArrayList RuleCombinerParameters
		{
			set{ throw new NotSupportedException(); }
		}
		#endregion
	}
}
