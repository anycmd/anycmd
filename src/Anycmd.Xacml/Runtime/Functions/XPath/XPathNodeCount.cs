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
	public class XPathNodeCount : FunctionBase
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
			XmlNodeList list = doc.DocumentElement.SelectNodes( xPath, context.ContextDocument.XmlNamespaceManager );
			return new EvaluationValue( list.Count, DataTypeDescriptor.Integer );
		}

		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType Returns
		{
			get{ return DataTypeDescriptor.Integer; }
		}

		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override inf.IDataType[] Arguments
		{
			get{ return new inf.IDataType[]{ DataTypeDescriptor.String }; }
		}

		#endregion
	}
}
