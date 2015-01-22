using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only PolicySet defined within a policy document.
	/// </summary>
	public class PolicySetElement : PolicySetElementReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates a new policySet using the arguments provided.
		/// </summary>
		/// <param name="id">The policy set id.</param>
		/// <param name="description">The description of the policy set.</param>
		/// <param name="target">The target for this policy set.</param>
		/// <param name="policies">All the policies inside this policy set.</param>
		/// <param name="policyCombiningAlgorithm">The policy combining algorithm for this policy set.</param>
		/// <param name="obligations">The obligations.</param>
		/// <param name="xpathVersion">The XPath version supported.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public PolicySetElement( string id, string description, TargetElementReadWrite target, ArrayList policies, string policyCombiningAlgorithm, ObligationReadWriteCollection obligations, string xpathVersion, XacmlVersion schemaVersion )
			: base( id, description, target, policies, policyCombiningAlgorithm, obligations, xpathVersion, schemaVersion )
		{
		}

		/// <summary>
		/// Creates a new PolicySet using the XmlReader instance provided.
		/// </summary>
		/// <param name="reader">The XmlReder positioned at the PolicySet element.</param>
		/// <param name="schemaVersion">The version of the schema that will be used to validate.</param>
		public PolicySetElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The PolicySet Id.
		/// </summary>
		public override string Id
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The Target for this PolicySet
		/// </summary>
		public override TargetElementReadWrite Target
		{
			get
			{
				return new TargetElement(base.Target.Resources, base.Target.Subjects, base.Target.Actions,
					 base.Target.Environments, base.Target.SchemaVersion); 
			}
		}

		/// <summary>
		/// The policy set description.
		/// </summary>
		public override string Description
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The policy combining algorithm Id.
		/// </summary>
		public override string PolicyCombiningAlgorithm
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The list of obligations.
		/// </summary>
		public override ObligationReadWriteCollection Obligations
		{
			get{ return new ObligationCollection( base.Obligations ); }
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// All the policies defined in this PolicySet
		/// </summary>
		public override ArrayList Policies
		{
			set{ throw new NotSupportedException(); }
			get
			{
				ArrayList policies = new ArrayList();

				foreach( XacmlElement element in base.Policies)
				{
					if( element is PolicySetElementReadWrite )
					{
						PolicySetElementReadWrite tempElement = (PolicySetElementReadWrite)element;
						policies.Add( new PolicySetElement( tempElement.Id,tempElement.Description,
							tempElement.Target,tempElement.Policies,
							tempElement.PolicyCombiningAlgorithm,tempElement.Obligations,
							tempElement.XPathVersion,tempElement.SchemaVersion ));
					}
					else if( element is PolicyElementReadWrite )
					{
						PolicyElementReadWrite tempElement = (PolicyElementReadWrite)element;
						policies.Add( new PolicyElement( tempElement.Id,tempElement.Description,
							tempElement.Target,tempElement.Rules,
							tempElement.RuleCombiningAlgorithm,tempElement.Obligations,
							tempElement.XPathVersion,tempElement.CombinerParameters,
							tempElement.RuleCombinerParameters,tempElement.VariableDefinitions,
							tempElement.SchemaVersion) );
					}
					else if( element is /*ReadWrite*/PolicyIdReferenceElement )
					{
						PolicyIdReferenceElementReadWrite tempElement = (PolicyIdReferenceElementReadWrite)element;
						policies.Add( new PolicyIdReferenceElement( tempElement.PolicyId, tempElement.Version, tempElement.EarliestVersion,
							tempElement.LatestVersion, tempElement.SchemaVersion ) );
					}
					else if( element is PolicySetIdReferenceElementReadWrite )
					{
						PolicySetIdReferenceElementReadWrite tempElement = (PolicySetIdReferenceElementReadWrite)element;
						policies.Add( new PolicySetIdReferenceElement( tempElement.PolicySetId, tempElement.Version, tempElement.EarliestVersion,
							tempElement.LatestVersion, tempElement.SchemaVersion ) );
					}
				}
				return ArrayList.ReadOnly(policies);
			}
		}

		/// <summary>
		/// The XPath version supported.
		/// </summary>
		public override string XPathVersion
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion
	}
}
