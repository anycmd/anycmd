using System;
using System.Xml;
using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class XPathNodeMatch : FunctionBase
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.AnyUriEqual; }
		}

		/// <summary>
		/// Evaluates the function.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="args">The function arguments.</param>
		/// <returns>The result value of the function evaluation.</returns>
		public override Anycmd.Xacml.Runtime.EvaluationValue Evaluate( rtm.EvaluationContext context, params inf.IFunctionParameter[] args )
		{
			if (context == null) throw new ArgumentNullException("context");
			if (args == null) throw new ArgumentNullException("args");
			XmlDocument doc = context.ContextDocument.XmlDocument;

			if( context.ContextDocument.XmlNamespaceManager == null )
			{
				context.ContextDocument.AddNamespaces( context.PolicyDocument.Namespaces );
			}

			string xPath = GetStringArgument( args, 0 );
			XmlNodeList firstList = doc.DocumentElement.SelectNodes( xPath, context.ContextDocument.XmlNamespaceManager );

			xPath = GetStringArgument( args, 1 );
			XmlNodeList secondList = doc.DocumentElement.SelectNodes( xPath, context.ContextDocument.XmlNamespaceManager );

			foreach( XmlNode firstNode in firstList )
			{
				foreach( XmlNode secondNode in secondList )
				{
					if( firstNode == secondNode )
					{
						return EvaluationValue.True;
					}
					if( firstNode.Attributes != null && secondNode.Attributes != null )
					{
						foreach( XmlNode firstAttrib in firstNode.Attributes )
						{
							foreach( XmlNode secondAttrib in secondNode.Attributes )
							{
								if( firstAttrib == secondAttrib )
								{
									return EvaluationValue.True;
								}
							}
						}
					}
					if( CompareChildNodes( firstNode, secondNode ) )
					{
						return EvaluationValue.True;
					}
				}
			}
			return EvaluationValue.False;
		}

		/// <summary>
		/// Recursively compares the child nodes of the first element with the second element.
		/// </summary>
		/// <param name="firstNode">The first node to search recursively.</param>
		/// <param name="secondNode">The second node to compare to.</param>
		/// <returns>true, if some of the childs of the first node is equals to the secondNode.</returns>
		private bool CompareChildNodes( XmlNode firstNode, XmlNode secondNode )
		{
			if( firstNode.ChildNodes != null )
			{
				foreach( XmlNode firstChild in firstNode.ChildNodes )
				{
					if( firstChild == secondNode || 
						CompareChildNodes( firstChild, secondNode ) )
					{
						return true;
					}
				}
			}
			return false;
		}
	
		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType Returns
		{
			get{ return DataTypeDescriptor.Boolean; }
		}

		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override inf.IDataType[] Arguments
		{
			get{ return new inf.IDataType[]{ DataTypeDescriptor.String, DataTypeDescriptor.String }; }
		}

		#endregion
	}
}
