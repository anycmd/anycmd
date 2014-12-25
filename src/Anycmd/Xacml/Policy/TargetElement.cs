using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only Target node defined in the policy document.
	/// </summary>
	public class TargetElement : TargetElementReadWrite
	{

		#region Constructors

		/// <summary>
		/// Creates a new Target with the specified agumetns.
		/// </summary>
		/// <param name="resources">The resources for this target.</param>
		/// <param name="subjects">The subjects for this target.</param>
		/// <param name="actions">The actions for this target.</param>
		/// <param name="environments">The environments for this target.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public TargetElement( ResourcesElementReadWrite resources, SubjectsElementReadWrite subjects, ActionsElementReadWrite actions, EnvironmentsElementReadWrite environments, XacmlVersion schemaVersion )
			: base( resources, subjects, actions, environments, schemaVersion )
		{
		}

		/// <summary>
		/// Creates a new Target using the XmlReader instance provided.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Target node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public TargetElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The Resources defined in this target.
		/// </summary>
		public override ResourcesElementReadWrite Resources
		{
			get
			{
				if(base.Resources != null)
					return new ResourcesElement(base.Resources.IsAny, base.Resources.ItemsList,base.Resources.SchemaVersion) ; 
				else
					return null;
			}
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The Subjects defined in this target.
		/// </summary>
		public override SubjectsElementReadWrite Subjects
		{
			get
			{
				if(base.Subjects != null)
					return new SubjectsElement( base.Subjects.IsAny, base.Subjects.ItemsList,base.Subjects.SchemaVersion); 
				else
					return null;
			}
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The Actions defined in this target.
		/// </summary>
		public override ActionsElementReadWrite Actions
		{
			get
			{
				if(base.Actions != null)
					return new ActionsElement(base.Actions.IsAny, base.Actions.ItemsList,base.Actions.SchemaVersion) ;
				else
					return null;
			}
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The Environments defined in this target.
		/// </summary>
		public override EnvironmentsElementReadWrite Environments
		{
			get
			{
				if(base.Environments != null)
					return new EnvironmentsElement(base.Environments.IsAny, base.Environments.ItemsList,base.Environments.SchemaVersion) ; 
				else
					return null;
			}
			set{ throw new NotSupportedException(); }
		}

		#endregion

	}
}
