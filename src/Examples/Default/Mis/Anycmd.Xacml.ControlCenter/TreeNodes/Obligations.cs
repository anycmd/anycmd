
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Obligations : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.ObligationReadWriteCollection _obligations;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obligations"></param>
		public Obligations( pol.ObligationReadWriteCollection obligations )
		{
			_obligations = obligations;

			this.Text = "Obligations";
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.ObligationReadWriteCollection ObligationDefinition
		{
			get{ return _obligations; }
		}
	}
}
