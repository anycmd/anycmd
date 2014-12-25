using System;

using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class DaytimeDurationEqual : BaseEqual
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.DaytimeDurationEqual; }
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
			try
			{
				if( GetDaytimeDurationArgument( args, 0 ).Equals( GetDaytimeDurationArgument( args, 1 ) ) )
				{
					return EvaluationValue.True;
				}
				else
				{
					return EvaluationValue.False;
				}
			}
			catch( EvaluationException e )
			{
				context.Trace(Consts.TraceError, e.Message); 
				return EvaluationValue.False;
			}
		}

		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType DataType
		{
			get{ return DataTypeDescriptor.DaytimeDuration; }
		}

		#endregion
	}
}
