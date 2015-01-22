using System;

using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class OrFunction : FunctionBase
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.Or; }
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
			bool retVal = GetBooleanArgument( args, 0 );
			for( int i = 1; i < args.Length; i++ )
			{
				retVal |= GetBooleanArgument( args, i );
			}
			return new EvaluationValue( retVal, DataTypeDescriptor.Boolean );
		}

		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override inf.IDataType Returns
		{
			get{ return DataTypeDescriptor.Boolean; }
		}
		
		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType[] Arguments
		{
			get
			{
				return new Anycmd.Xacml.Interfaces.IDataType[]{ DataTypeDescriptor.Boolean, DataTypeDescriptor.Boolean };
			}
		}

		#endregion
	}
}
