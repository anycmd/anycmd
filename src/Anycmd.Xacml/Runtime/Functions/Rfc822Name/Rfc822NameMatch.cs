using System;
using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;
using typ = Anycmd.Xacml.Runtime.DataTypes;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class Rfc822NameMatch : BaseEqual
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.Rfc822NameMatch; }
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
			if( ((typ.Rfc822Name)args[1].GetTypedValue( DataTypeDescriptor.Rfc822Name, 1 )).Matches( GetStringArgument( args, 0 ) ) )
			{
				return EvaluationValue.True;
			}
			else
			{
				return EvaluationValue.False;
			}
		}

		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType DataType
		{
			get{ return DataTypeDescriptor.Rfc822Name; }
		}

		#endregion
	}
}
