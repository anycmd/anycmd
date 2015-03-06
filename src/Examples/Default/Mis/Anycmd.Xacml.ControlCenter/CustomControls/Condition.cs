using System;
using System.Reflection;
using System.Windows.Forms;

using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for Condition.
	/// </summary>
	public class Condition : BaseControl
	{
		private System.Windows.Forms.TreeView tvwCondition;
		private pol.ConditionElementReadWrite _condition;
		private System.Windows.Forms.GroupBox grpCondition;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDataType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.GroupBox grpElement;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem mniAdd;
		private System.Windows.Forms.MenuItem mniDelete;
		private System.Windows.Forms.ComboBox cmbInternalFunctions;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public Condition( pol.ConditionElementReadWrite condition )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_condition = condition;

			tvwCondition.Nodes.Add( new TreeNodes.FunctionExecution( condition ) );
			tvwCondition.ExpandAll();

			foreach( FieldInfo field in typeof(Consts.Schema1.InternalDataTypes).GetFields() )
			{
				cmbDataType.Items.Add( field.GetValue( null ) );
			}
			foreach( FieldInfo field in typeof(Consts.Schema1.InternalFunctions).GetFields() )
			{
				cmbInternalFunctions.Items.Add( field.GetValue( null ) );
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tvwCondition = new System.Windows.Forms.TreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.mniAdd = new System.Windows.Forms.MenuItem();
			this.mniDelete = new System.Windows.Forms.MenuItem();
			this.grpCondition = new System.Windows.Forms.GroupBox();
			this.grpElement = new System.Windows.Forms.GroupBox();
			this.cmbDataType = new System.Windows.Forms.ComboBox();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbInternalFunctions = new System.Windows.Forms.ComboBox();
			this.grpCondition.SuspendLayout();
			this.grpElement.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvwCondition
			// 
			this.tvwCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvwCondition.ContextMenu = this.contextMenu;
			this.tvwCondition.ImageIndex = -1;
			this.tvwCondition.Location = new System.Drawing.Point(8, 24);
			this.tvwCondition.Name = "tvwCondition";
			this.tvwCondition.SelectedImageIndex = -1;
			this.tvwCondition.Size = new System.Drawing.Size(632, 248);
			this.tvwCondition.TabIndex = 0;
			this.tvwCondition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvwCondition_MouseDown);
			this.tvwCondition.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCondition_AfterSelect);
			this.tvwCondition.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwCondition_BeforeSelect);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mniAdd,
																						this.mniDelete});
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// mniAdd
			// 
			this.mniAdd.Index = 0;
			this.mniAdd.Text = "Add";
			// 
			// mniDelete
			// 
			this.mniDelete.Index = 1;
			this.mniDelete.Text = "Delete";
			this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
			// 
			// grpCondition
			// 
			this.grpCondition.Controls.Add(this.grpElement);
			this.grpCondition.Controls.Add(this.tvwCondition);
			this.grpCondition.Location = new System.Drawing.Point(8, 8);
			this.grpCondition.Name = "grpCondition";
			this.grpCondition.Size = new System.Drawing.Size(656, 456);
			this.grpCondition.TabIndex = 1;
			this.grpCondition.TabStop = false;
			this.grpCondition.Text = "Condition";
			// 
			// grpElement
			// 
			this.grpElement.Controls.Add(this.cmbInternalFunctions);
			this.grpElement.Controls.Add(this.cmbDataType);
			this.grpElement.Controls.Add(this.txtValue);
			this.grpElement.Controls.Add(this.label1);
			this.grpElement.Controls.Add(this.label2);
			this.grpElement.Location = new System.Drawing.Point(8, 288);
			this.grpElement.Name = "grpElement";
			this.grpElement.Size = new System.Drawing.Size(632, 144);
			this.grpElement.TabIndex = 9;
			this.grpElement.TabStop = false;
			this.grpElement.Text = "Element";
			// 
			// cmbDataType
			// 
			this.cmbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbDataType.Location = new System.Drawing.Point(72, 40);
			this.cmbDataType.Name = "cmbDataType";
			this.cmbDataType.Size = new System.Drawing.Size(536, 21);
			this.cmbDataType.TabIndex = 6;
			// 
			// txtValue
			// 
			this.txtValue.Location = new System.Drawing.Point(72, 88);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(536, 20);
			this.txtValue.TabIndex = 8;
			this.txtValue.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Data type:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "Value:";
			// 
			// cmbInternalFunctions
			// 
			this.cmbInternalFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbInternalFunctions.Location = new System.Drawing.Point(72, 88);
			this.cmbInternalFunctions.Name = "cmbInternalFunctions";
			this.cmbInternalFunctions.Size = new System.Drawing.Size(536, 21);
			this.cmbInternalFunctions.TabIndex = 9;
			this.cmbInternalFunctions.Visible = false;
			// 
			// Condition
			// 
			this.Controls.Add(this.grpCondition);
			this.Name = "Condition";
			this.Size = new System.Drawing.Size(680, 488);
			this.grpCondition.ResumeLayout(false);
			this.grpElement.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void tvwCondition_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if( e.Node is TreeNodes.FunctionExecution )
			{
				grpElement.Text = "Function execution";
				TreeNodes.FunctionExecution node = (TreeNodes.FunctionExecution)e.Node;
				txtValue.Visible = false;
				cmbInternalFunctions.Visible = true;
				cmbInternalFunctions.SelectedIndex = cmbInternalFunctions.FindStringExact( node.ApplyBaseDefinition.FunctionId );
				label2.Text = "FunctionId:";
				cmbDataType.Enabled = false;
			}
			else if( e.Node is TreeNodes.FunctionParameter )
			{
				grpElement.Text = "Function parameter";
				TreeNodes.FunctionParameter node = (TreeNodes.FunctionParameter)e.Node;
				txtValue.Visible = false;
				cmbInternalFunctions.Visible = true;
				cmbInternalFunctions.SelectedIndex = cmbInternalFunctions.FindStringExact( node.FunctionDefinition.FunctionId );
				label2.Text = "FunctionId:";
				cmbDataType.Enabled = false;
			}
			else if( e.Node is TreeNodes.AttributeValue )
			{
				grpElement.Text = "Attribute value";
				TreeNodes.AttributeValue node = (TreeNodes.AttributeValue)e.Node;
				txtValue.Visible = true;
				cmbInternalFunctions.Visible = false;
				txtValue.Text = node.AttributeValueDefinition.Contents;
				label2.Text = "Value:";
				cmbDataType.Enabled = true;
				cmbDataType.SelectedIndex = cmbDataType.FindStringExact( node.AttributeValueDefinition.DataType );
			}
			else if( e.Node is TreeNodes.AttributeDesignator )
			{
				grpElement.Text = "Attribute designator";
				TreeNodes.AttributeDesignator node = (TreeNodes.AttributeDesignator)e.Node;
				txtValue.Visible = true;
				cmbInternalFunctions.Visible = false;
				txtValue.Text = node.AttributeDesignatorDefinition.AttributeId;
				label2.Text = "AttributeId:";
				cmbDataType.Enabled = true;
				cmbDataType.SelectedIndex = cmbDataType.FindStringExact( node.AttributeDesignatorDefinition.DataType );
			}
			else if( e.Node is TreeNodes.AttributeSelector )
			{
				grpElement.Text = "Attribute selector";
				TreeNodes.AttributeSelector node = (TreeNodes.AttributeSelector)e.Node;
				txtValue.Visible = true;
				cmbInternalFunctions.Visible = false;
				txtValue.Text = node.AttributeSelectorDefinition.RequestContextPath;
				label2.Text = "XPath:";
				cmbDataType.Enabled = false;
			}
		}

		private void tvwCondition_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Right )
			{
				tvwCondition.SelectedNode = tvwCondition.GetNodeAt( e.X, e.Y );
			}
		}

		private void tvwCondition_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			TreeNodes.NoBoldNode node = (TreeNodes.NoBoldNode)tvwCondition.SelectedNode;

			if( node is TreeNodes.FunctionExecution )
			{
				TreeNodes.FunctionExecution funcNode = ((TreeNodes.FunctionExecution)node);
				funcNode.ApplyBaseDefinition.FunctionId = cmbInternalFunctions.Text;
				tvwCondition.SelectedNode = funcNode;
				tvwCondition.SelectedNode.Text = "[" + "dataType" + "] " + funcNode.ApplyBaseDefinition.FunctionId;
			}
			else if( node is TreeNodes.FunctionParameter )
			{
				TreeNodes.FunctionParameter funcNode = ((TreeNodes.FunctionParameter)node);
				funcNode.FunctionDefinition.FunctionId = cmbInternalFunctions.Text;
				tvwCondition.SelectedNode = funcNode;
				tvwCondition.SelectedNode.Text = "Function: " + funcNode.FunctionDefinition.FunctionId;;
			}
			else if( node is TreeNodes.AttributeValue )
			{
				TreeNodes.AttributeValue attNode = ((TreeNodes.AttributeValue)node);
				attNode.AttributeValueDefinition.Value = txtValue.Text;
				attNode.AttributeValueDefinition.DataType = cmbDataType.Text;
				tvwCondition.SelectedNode = attNode;
				tvwCondition.SelectedNode.Text = "[" + attNode.AttributeValueDefinition.DataType + "] " + attNode.AttributeValueDefinition.Contents;
			}
			else if( node is TreeNodes.AttributeDesignator )
			{
				TreeNodes.AttributeDesignator attNode = ((TreeNodes.AttributeDesignator)node);
				attNode.AttributeDesignatorDefinition.AttributeId = txtValue.Text;
				attNode.AttributeDesignatorDefinition.DataType = cmbDataType.Text;
				tvwCondition.SelectedNode = attNode;
				tvwCondition.SelectedNode.Text = "[" + attNode.AttributeDesignatorDefinition.DataType + "]:" + attNode.AttributeDesignatorDefinition.AttributeId;
			}
			else if( node is TreeNodes.AttributeSelector )
			{
				TreeNodes.AttributeSelector attNode = ((TreeNodes.AttributeSelector)node);
				attNode.AttributeSelectorDefinition.RequestContextPath = txtValue.Text;
				tvwCondition.SelectedNode = attNode;
				tvwCondition.SelectedNode.Text = "XPath: " + attNode.AttributeSelectorDefinition.RequestContextPath;
			}
		}

		#region Context menu

		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			mniAdd.MenuItems.Clear();
			if( tvwCondition.SelectedNode == null)
			{
				if(tvwCondition.Nodes.Count == 0)
				{
					mniAdd.MenuItems.Add( "Function execution", new System.EventHandler( CreateFunctionExecution ) );
					mniAdd.MenuItems.Add( "Function parameter", new System.EventHandler( CreateFunctionParameter ) );
					mniAdd.MenuItems.Add( "Attribute value", new System.EventHandler( CreateAttributeValue ) );
					mniAdd.MenuItems.Add( "Action attribute designator", new System.EventHandler( CreateActionAttributeDesignator ) );
					mniAdd.MenuItems.Add( "Subject attribute designator", new System.EventHandler( CreateSubjectAttributeDesignator ) );
					mniAdd.MenuItems.Add( "Resource designator", new System.EventHandler( CreateResourceAttributeDesignator ) );
					mniAdd.MenuItems.Add( "Attribute selector", new System.EventHandler( CreateAttributeSelector ) );
				}
				else
				{
					mniDelete.Visible = false;
				}
			}
			else if( tvwCondition.SelectedNode is TreeNodes.FunctionExecution)
			{
				mniAdd.MenuItems.Add( "Function execution", new System.EventHandler( CreateFunctionExecutionFromFunction ) );
				mniAdd.MenuItems.Add( "Function parameter", new System.EventHandler( CreateFunctionParameterFromFunction ) );
				mniAdd.MenuItems.Add( "Attribute value", new System.EventHandler( CreateAttributeValueFromFunction ) );
				mniAdd.MenuItems.Add( "Action attribute designator", new System.EventHandler( CreateActionAttributeDesignatorFromFunction ) );
				mniAdd.MenuItems.Add( "Subject attribute designator", new System.EventHandler( CreateSubjectAttributeDesignatorFromFunction ) );
				mniAdd.MenuItems.Add( "Resource designator", new System.EventHandler( CreateResourceAttributeDesignatorFromFunction ) );
				mniAdd.MenuItems.Add( "Attribute selector", new System.EventHandler( CreateAttributeSelectorFromFunction ) );
			}

			if( mniAdd.MenuItems.Count == 0 )
			{
				mniAdd.Visible = false;
			}
			else
			{
				mniAdd.Visible = true;
			}
		}

		private void CreateFunctionExecution( object sender, EventArgs args )
		{
			pol.ApplyElement apply = new pol.ApplyElement("urn:new_function", new pol.ExpressionReadWriteCollection(), Xacml.XacmlVersion.Version11);
			TreeNodes.FunctionExecution node = new TreeNodes.FunctionExecution( apply );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( apply );
		}

		private void CreateFunctionExecutionFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.ApplyElement apply = new pol.ApplyElement("urn:new_function", new pol.ExpressionReadWriteCollection(), Xacml.XacmlVersion.Version11);
			TreeNodes.FunctionExecution node = new TreeNodes.FunctionExecution( apply );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( apply );
		}

		private void CreateFunctionParameter( object sender, EventArgs args )
		{
			pol.FunctionElementReadWrite function = new pol.FunctionElementReadWrite( "urn:new_function_param", Xacml.XacmlVersion.Version11 );
			TreeNodes.FunctionParameter node = new TreeNodes.FunctionParameter( function );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( function );
		}

		private void CreateFunctionParameterFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.FunctionElementReadWrite function = new pol.FunctionElementReadWrite( "urn:new_function_param", Xacml.XacmlVersion.Version11 );
			TreeNodes.FunctionParameter node = new TreeNodes.FunctionParameter( function );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( function );
		}

		private void CreateAttributeValueFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.AttributeValueElementReadWrite attr = new pol.AttributeValueElementReadWrite( Xacml.Consts.Schema1.InternalDataTypes.XsdString, "TODO: Add content", Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeValue node = new TreeNodes.AttributeValue( attr );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( attr );
		}

		private void CreateAttributeValue( object sender, EventArgs args )
		{
			pol.AttributeValueElementReadWrite attr = new pol.AttributeValueElementReadWrite( Xacml.Consts.Schema1.InternalDataTypes.XsdString, "TODO: Add content", Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeValue node = new TreeNodes.AttributeValue( attr );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( attr );
		}

		private void CreateActionAttributeDesignator( object sender, EventArgs args )
		{
			pol.ActionAttributeDesignatorElement att = new pol.ActionAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty ,Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( att );
		}

		private void CreateSubjectAttributeDesignator( object sender, EventArgs args )
		{
			pol.SubjectAttributeDesignatorElement att = new pol.SubjectAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty, string.Empty ,Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( att );
		}

		private void CreateResourceAttributeDesignator( object sender, EventArgs args )
		{
			pol.ResourceAttributeDesignatorElement att = new pol.ResourceAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty, Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( att );
		}

		private void CreateActionAttributeDesignatorFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.ActionAttributeDesignatorElement att = new pol.ActionAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty ,Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( att );
		}

		private void CreateSubjectAttributeDesignatorFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.SubjectAttributeDesignatorElement att = new pol.SubjectAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty, string.Empty ,Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( att );
		}

		private void CreateResourceAttributeDesignatorFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.ResourceAttributeDesignatorElement att = new pol.ResourceAttributeDesignatorElement( string.Empty, false, "TODO: Add attribute id", string.Empty, Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeDesignator node = new TreeNodes.AttributeDesignator( att );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( att );
		}

		private void CreateAttributeSelector( object sender, EventArgs args )
		{
			pol.AttributeSelectorElement attr = new pol.AttributeSelectorElement( string.Empty, false, "TODO: Add XPath", Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeSelector node = new TreeNodes.AttributeSelector( attr );

			tvwCondition.Nodes.Add( node );
			_condition.Arguments.Add( attr );
		}

		private void CreateAttributeSelectorFromFunction( object sender, EventArgs args )
		{
			TreeNodes.FunctionExecution func = (TreeNodes.FunctionExecution)tvwCondition.SelectedNode;
			pol.ApplyBaseReadWrite parentApply = func.ApplyBaseDefinition;

			pol.AttributeSelectorElement attr = new pol.AttributeSelectorElement( string.Empty, false, "TODO: Add XPath", Xacml.XacmlVersion.Version11 );
			TreeNodes.AttributeSelector node = new TreeNodes.AttributeSelector( attr );

			func.Nodes.Add( node );
			parentApply.Arguments.Add( attr );
		}

		private void mniDelete_Click(object sender, System.EventArgs e)
		{
			TreeNodes.FunctionExecution functionNode =  (TreeNodes.FunctionExecution)tvwCondition.SelectedNode.Parent;

			TreeNodes.NoBoldNode node = (TreeNodes.NoBoldNode)tvwCondition.SelectedNode;

			if( node is TreeNodes.FunctionExecution )
			{
				TreeNodes.FunctionExecution funcNode = ((TreeNodes.FunctionExecution)node);
				int index = functionNode.ApplyBaseDefinition.Arguments.GetIndex( (pol.ApplyElement)funcNode.ApplyBaseDefinition );
				functionNode.ApplyBaseDefinition.Arguments.RemoveAt( index );
				functionNode.Nodes.Remove( funcNode );
			}
			else if( node is TreeNodes.FunctionParameter )
			{
				TreeNodes.FunctionParameter funcNode = ((TreeNodes.FunctionParameter)node);
				int index = functionNode.ApplyBaseDefinition.Arguments.GetIndex( funcNode.FunctionDefinition );
				functionNode.ApplyBaseDefinition.Arguments.RemoveAt( index );
				functionNode.Nodes.Remove( funcNode );
			}
			else if( node is TreeNodes.AttributeValue )
			{
				TreeNodes.AttributeValue attNode = ((TreeNodes.AttributeValue)node);
				int index = functionNode.ApplyBaseDefinition.Arguments.GetIndex( attNode.AttributeValueDefinition );
				functionNode.ApplyBaseDefinition.Arguments.RemoveAt( index );
				functionNode.Nodes.Remove( attNode );
			}
			else if( node is TreeNodes.AttributeDesignator )
			{
				TreeNodes.AttributeDesignator attNode = ((TreeNodes.AttributeDesignator)node);
				int index = functionNode.ApplyBaseDefinition.Arguments.GetIndex( attNode.AttributeDesignatorDefinition );
				functionNode.ApplyBaseDefinition.Arguments.RemoveAt( index );
				functionNode.Nodes.Remove( attNode );
			}
			else if( node is TreeNodes.AttributeSelector )
			{
				TreeNodes.AttributeSelector attNode = ((TreeNodes.AttributeSelector)node);
				int index = functionNode.ApplyBaseDefinition.Arguments.GetIndex( attNode.AttributeSelectorDefinition );
				functionNode.ApplyBaseDefinition.Arguments.RemoveAt( index );
				functionNode.Nodes.Remove( attNode );
			}
		}
		
		#endregion
	}
}
