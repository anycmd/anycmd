using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only PolicySetIdReference defined in the policy document.
	/// </summary>
	public class PolicySetIdReferenceElement : PolicySetIdReferenceElementReadWrite
	{
		#region Constructor

		/// <summary>
		/// Creates a policy set id reference using the XmlReader instance provided.
		/// </summary>
		/// <param name="reader">The XmlReader instance positioned at the "PolicySetIdReference" 
		/// node</param>
		/// <param name="schemaVersion">The version of the schema that will be used to validate.</param>
		public PolicySetIdReferenceElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		/// <summary>
		/// Creates an instance of the element, with the provided values
		/// </summary>
		/// <param name="policySetIdReference"></param>
		/// <param name="version"></param>
		/// <param name="earliestVersion"></param>
		/// <param name="latestVersion"></param>
		/// <param name="schemaVersion"></param>
		public PolicySetIdReferenceElement( string policySetIdReference, string version, string earliestVersion, string latestVersion, 
			XacmlVersion schemaVersion ) : base( policySetIdReference, version, earliestVersion, latestVersion,  schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The Id of the referenced policy set.
		/// </summary>
		public override string PolicySetId
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The referenced Policy version.
		/// </summary>
		public override string Version
		{
			set{ throw new NotSupportedException(); }
		}


		/// <summary>
		/// The referenced Policy earliest version.
		/// </summary>
		public override string EarliestVersion
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The referenced Policy latest version.
		/// </summary>
		public override string LatestVersion
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion
	}
}
