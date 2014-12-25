using System;

using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class NofFunction : FunctionBase
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.Nof; }
		}

		/// <summary>
		/// Method called by the EvaluationEngine to evaluate the function.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="args">The IFuctionParameters that will be used as arguments to the function.</param>
		/// <returns></returns>
		public override Anycmd.Xacml.Runtime.EvaluationValue Evaluate( rtm.EvaluationContext context, params inf.IFunctionParameter[] args )
		{
			if (context == null) throw new ArgumentNullException("context");
			if (args == null) throw new ArgumentNullException("args");
			int argCount = GetIntegerArgument( args, 0 );
			if( args.Length - 1 < argCount )
			{
				return EvaluationValue.Indeterminate;
			}
			else if( argCount == 0 )
			{
				return EvaluationValue.True;
			}
			else
			{
				int count = 0;
				for( int i = 1; i < args.Length; i++ )
				{
					if( GetBooleanArgument( args, i ) )
					{
						count++;
					}
					if( count == argCount )
					{
						return EvaluationValue.True;
					}
				}
				return EvaluationValue.False;
			}
		}

		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override inf.IDataType Returns
		{
			get{ return DataTypeDescriptor.Boolean; }
		}

		/// <summary>
		/// Whether the function defines variable arguments. The data type of the variable arguments will be the 
		/// data type of the last parameter.
		/// </summary>
		public override bool VarArgs
		{ 
			get{ return true; } 
		}

		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType[] Arguments
		{
			get
			{
				return new Anycmd.Xacml.Interfaces.IDataType[]{ DataTypeDescriptor.Integer, DataTypeDescriptor.Boolean };
			}
		}

		#endregion
	}
}
