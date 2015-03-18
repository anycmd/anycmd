using Anycmd.Xacml.Interfaces;
using System;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// BAse class for all the implementations of the AtLeastOneMemberOf&gt;type&lt; functions.
	/// </summary>
	public abstract class BaseAtLeastOneMemberOf : FunctionBase, ITypeSpecificFunction
	{
		#region IFunction Members

		/// <summary>
		/// Evaluates the function.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="args">The function arguments.</param>
		/// <returns>The result value of the function evaluation.</returns>
		public override EvaluationValue Evaluate( EvaluationContext context, params IFunctionParameter[] args )
		{
			if (context == null) throw new ArgumentNullException("context");
			if (args == null) throw new ArgumentNullException("args");
			IFunction function = DataType.EqualFunction;
			foreach( object par1 in args[0].Elements )
			{
				foreach( object par2 in args[1].Elements )
				{
					EvaluationValue retVal = function.Evaluate( 
						context, 
						new EvaluationValue( par1, args[0].GetType( context ) ), 
						new EvaluationValue( par2, args[1].GetType( context ) ) );
					if( retVal.BoolValue )
					{
						return EvaluationValue.True;
					}
				}
			}
			return EvaluationValue.False;
		}

		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override IDataType Returns
		{
			get{ return DataTypeDescriptor.Boolean; }
		}

		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override IDataType[] Arguments
		{
			get
			{
				return new IDataType[]{ DataTypeDescriptor.Bag, DataTypeDescriptor.Bag };
			}
		}

		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		public abstract IDataType DataType { get; }

		#endregion
	}
}
