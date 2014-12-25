
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class FunctionExecution : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.ApplyBaseReadWrite _apply;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="apply"></param>
		public FunctionExecution( pol.ApplyBaseReadWrite apply )
		{
			_apply = apply;

			//TODO: find the function data type.
			this.Text = "[" + "dataType" + "] " + apply.FunctionId; 
			
			foreach( inf.IExpression arg in apply.Arguments )
			{
				if( arg is pol.ApplyBaseReadWrite )
				{
					this.Nodes.Add( new FunctionExecution( (pol.ApplyBaseReadWrite)arg ) );
				}
				else if( arg is pol.FunctionElementReadWrite )
				{
					this.Nodes.Add( new FunctionParameter( (pol.FunctionElementReadWrite)arg ) );
				}
				else if( arg is pol.AttributeValueElementReadWrite )
				{
					this.Nodes.Add( new AttributeValue( (pol.AttributeValueElementReadWrite)arg ) );
				}
				else if( arg is pol.AttributeDesignatorBase )
				{
					this.Nodes.Add( new AttributeDesignator( (pol.AttributeDesignatorBase)arg ) );
				}
				else if( arg is pol.AttributeSelectorElement )
				{
					this.Nodes.Add( new AttributeSelector( (pol.AttributeSelectorElement)arg ) );
				}
			}
		}

		/// <summary>
		/// Gets the ApplyBaseReadWrite element
		/// </summary>
		public pol.ApplyBaseReadWrite ApplyBaseDefinition
		{
			get{ return _apply; }
		}
	}
}
