
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class FunctionParameter : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.FunctionElementReadWrite _function;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="function"></param>
		public FunctionParameter( pol.FunctionElementReadWrite function )
		{
			_function = function;

			this.Text = "Function: " + _function.FunctionId;
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.FunctionElementReadWrite FunctionDefinition
		{
			get{ return _function; }
		}
	}
}
