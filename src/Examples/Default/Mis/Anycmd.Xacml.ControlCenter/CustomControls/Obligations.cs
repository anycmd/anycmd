using System;

using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for PolicySet.
	/// </summary>
	public class Obligations : BaseControl
	{
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private pol.ObligationReadWriteCollection _obligations;
		private System.Windows.Forms.ListBox lstObligations;
		private System.Windows.Forms.GroupBox grpObligations;
		private System.Windows.Forms.GroupBox grpObligation;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.ComboBox cmbEffect;
		private System.Windows.Forms.GroupBox grpAssignment;
		private System.Windows.Forms.ListBox lstAttributeAssignments;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAttributeId;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtAttributeAssignemnt;
		private int index = -1;
		private int obligationIndex = -1;
		private System.Windows.Forms.Button btnAttributeAssignmentsAdd;
		private System.Windows.Forms.Button btnAttributeAssignmentsRemove;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obligations"></param>
		public Obligations( pol.ObligationReadWriteCollection obligations )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			LoadingData = true;

			_obligations = obligations;

			cmbEffect.Items.Add( pol.Effect.Deny );
			cmbEffect.Items.Add( pol.Effect.Permit );

			lstAttributeAssignments.DisplayMember = "AttributeId";

			lstObligations.DisplayMember = "ObligationId";
			foreach( pol.ObligationElementReadWrite obligation in obligations )
			{
				lstObligations.Items.Add( obligation );
			}

			if( obligations.Count != 0 )
			{
				lstObligations.SelectedIndex = 0;
			}

			LoadingData = false;
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
			this.grpObligations = new System.Windows.Forms.GroupBox();
			this.lstObligations = new System.Windows.Forms.ListBox();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.grpObligation = new System.Windows.Forms.GroupBox();
			this.grpAssignment = new System.Windows.Forms.GroupBox();
			this.txtAttributeAssignemnt = new System.Windows.Forms.TextBox();
			this.txtAttributeId = new System.Windows.Forms.TextBox();
			this.lstAttributeAssignments = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbEffect = new System.Windows.Forms.ComboBox();
			this.txtId = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnAttributeAssignmentsAdd = new System.Windows.Forms.Button();
			this.btnAttributeAssignmentsRemove = new System.Windows.Forms.Button();
			this.grpObligations.SuspendLayout();
			this.grpObligation.SuspendLayout();
			this.grpAssignment.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpObligations
			// 
			this.grpObligations.Controls.Add(this.lstObligations);
			this.grpObligations.Controls.Add(this.btnDown);
			this.grpObligations.Controls.Add(this.btnUp);
			this.grpObligations.Controls.Add(this.btnRemove);
			this.grpObligations.Controls.Add(this.btnAdd);
			this.grpObligations.Location = new System.Drawing.Point(8, 8);
			this.grpObligations.Name = "grpObligations";
			this.grpObligations.Size = new System.Drawing.Size(576, 160);
			this.grpObligations.TabIndex = 2;
			this.grpObligations.TabStop = false;
			this.grpObligations.Text = "Obligations";
			// 
			// lstObligations
			// 
			this.lstObligations.Location = new System.Drawing.Point(8, 16);
			this.lstObligations.Name = "lstObligations";
			this.lstObligations.Size = new System.Drawing.Size(472, 134);
			this.lstObligations.TabIndex = 5;
			this.lstObligations.SelectedIndexChanged += new System.EventHandler(this.lstObligations_SelectedIndexChanged);
			// 
			// btnDown
			// 
			this.btnDown.Location = new System.Drawing.Point(488, 120);
			this.btnDown.Name = "btnDown";
			this.btnDown.TabIndex = 4;
			this.btnDown.Text = "Down";
			// 
			// btnUp
			// 
			this.btnUp.Location = new System.Drawing.Point(488, 88);
			this.btnUp.Name = "btnUp";
			this.btnUp.TabIndex = 3;
			this.btnUp.Text = "Up";
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(488, 56);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(488, 24);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// grpObligation
			// 
			this.grpObligation.Controls.Add(this.grpAssignment);
			this.grpObligation.Controls.Add(this.cmbEffect);
			this.grpObligation.Controls.Add(this.txtId);
			this.grpObligation.Controls.Add(this.label2);
			this.grpObligation.Controls.Add(this.label1);
			this.grpObligation.Location = new System.Drawing.Point(8, 176);
			this.grpObligation.Name = "grpObligation";
			this.grpObligation.Size = new System.Drawing.Size(576, 272);
			this.grpObligation.TabIndex = 3;
			this.grpObligation.TabStop = false;
			this.grpObligation.Text = "Obligation";
			// 
			// grpAssignment
			// 
			this.grpAssignment.Controls.Add(this.btnAttributeAssignmentsRemove);
			this.grpAssignment.Controls.Add(this.btnAttributeAssignmentsAdd);
			this.grpAssignment.Controls.Add(this.txtAttributeAssignemnt);
			this.grpAssignment.Controls.Add(this.txtAttributeId);
			this.grpAssignment.Controls.Add(this.lstAttributeAssignments);
			this.grpAssignment.Controls.Add(this.label3);
			this.grpAssignment.Controls.Add(this.label4);
			this.grpAssignment.Location = new System.Drawing.Point(8, 80);
			this.grpAssignment.Name = "grpAssignment";
			this.grpAssignment.Size = new System.Drawing.Size(560, 184);
			this.grpAssignment.TabIndex = 5;
			this.grpAssignment.TabStop = false;
			this.grpAssignment.Text = "Attribute assignments";
			// 
			// txtAttributeAssignemnt
			// 
			this.txtAttributeAssignemnt.Location = new System.Drawing.Point(72, 136);
			this.txtAttributeAssignemnt.Multiline = true;
			this.txtAttributeAssignemnt.Name = "txtAttributeAssignemnt";
			this.txtAttributeAssignemnt.Size = new System.Drawing.Size(480, 40);
			this.txtAttributeAssignemnt.TabIndex = 6;
			this.txtAttributeAssignemnt.Text = "";
			// 
			// txtAttributeId
			// 
			this.txtAttributeId.Location = new System.Drawing.Point(72, 104);
			this.txtAttributeId.Name = "txtAttributeId";
			this.txtAttributeId.Size = new System.Drawing.Size(480, 20);
			this.txtAttributeId.TabIndex = 5;
			this.txtAttributeId.Text = "";
			// 
			// lstAttributeAssignments
			// 
			this.lstAttributeAssignments.Location = new System.Drawing.Point(8, 24);
			this.lstAttributeAssignments.Name = "lstAttributeAssignments";
			this.lstAttributeAssignments.Size = new System.Drawing.Size(464, 69);
			this.lstAttributeAssignments.TabIndex = 4;
			this.lstAttributeAssignments.SelectedIndexChanged += new System.EventHandler(this.lstAttributeAssignments_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Name = "label3";
			this.label3.TabIndex = 1;
			this.label3.Text = "Attribute id:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 136);
			this.label4.Name = "label4";
			this.label4.TabIndex = 1;
			this.label4.Text = "Contents:";
			// 
			// cmbEffect
			// 
			this.cmbEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbEffect.Location = new System.Drawing.Point(80, 48);
			this.cmbEffect.Name = "cmbEffect";
			this.cmbEffect.Size = new System.Drawing.Size(488, 21);
			this.cmbEffect.TabIndex = 3;
			this.cmbEffect.SelectedIndexChanged += cmbEffect_SelectedIndexChanged;
			// 
			// txtId
			// 
			this.txtId.Location = new System.Drawing.Point(80, 16);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(488, 20);
			this.txtId.TabIndex = 2;
			this.txtId.Text = "";
			this.txtId.TextChanged += txtId_TextChanged;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Fullfill on:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Obligation id:";
			// 
			// btnAttributeAssignmentsAdd
			// 
			this.btnAttributeAssignmentsAdd.Location = new System.Drawing.Point(480, 24);
			this.btnAttributeAssignmentsAdd.Name = "btnAttributeAssignmentsAdd";
			this.btnAttributeAssignmentsAdd.Size = new System.Drawing.Size(72, 23);
			this.btnAttributeAssignmentsAdd.TabIndex = 7;
			this.btnAttributeAssignmentsAdd.Text = "Add";
			this.btnAttributeAssignmentsAdd.Click += new System.EventHandler(this.btnAttributeAssignmentsAdd_Click);
			// 
			// btnAttributeAssignmentsRemove
			// 
			this.btnAttributeAssignmentsRemove.Location = new System.Drawing.Point(480, 56);
			this.btnAttributeAssignmentsRemove.Name = "btnAttributeAssignmentsRemove";
			this.btnAttributeAssignmentsRemove.Size = new System.Drawing.Size(72, 23);
			this.btnAttributeAssignmentsRemove.TabIndex = 8;
			this.btnAttributeAssignmentsRemove.Text = "Remove";
			this.btnAttributeAssignmentsRemove.Click += new System.EventHandler(this.btnAttributeAssignmentsRemove_Click);
			// 
			// Obligations
			// 
			this.Controls.Add(this.grpObligation);
			this.Controls.Add(this.grpObligations);
			this.Name = "Obligations";
			this.Size = new System.Drawing.Size(592, 456);
			this.grpObligations.ResumeLayout(false);
			this.grpObligation.ResumeLayout(false);
			this.grpAssignment.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void txtId_TextChanged(object sender, EventArgs e)
		{
			base.TextBox_TextChanged(sender, e);
		}

		private void cmbEffect_SelectedIndexChanged(object sender, EventArgs e)
		{
			base.ComboBox_SelectedIndexChanged(sender, e);
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstObligations_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( index != -1 )
			{
				pol.AttributeAssignmentElementReadWrite attrAux = lstAttributeAssignments.Items[index] as pol.AttributeAssignmentElementReadWrite;
				attrAux.AttributeId = txtAttributeId.Text;
				attrAux.Value = txtAttributeAssignemnt.Text;
			}
			if( obligationIndex != -1 && obligationIndex != lstObligations.SelectedIndex )
			{
				pol.ObligationElementReadWrite obliAux = lstObligations.Items[obligationIndex] as pol.ObligationElementReadWrite;
				obliAux.ObligationId = txtId.Text;
				if( cmbEffect.Text.Equals( pol.Effect.Deny.ToString()) )
				{
					obliAux.FulfillOn = pol.Effect.Deny;
				}
				else if( cmbEffect.Text.Equals( pol.Effect.Permit.ToString()) )
				{
					obliAux.FulfillOn = pol.Effect.Permit;
				}
				lstObligations.Items.RemoveAt( obligationIndex );
				lstObligations.Items.Insert( obligationIndex, obliAux );
			}
			obligationIndex = lstObligations.SelectedIndex;
			pol.ObligationElementReadWrite obligation = lstObligations.SelectedItem as pol.ObligationElementReadWrite;

			if(!LoadingData)
			{
				try
				{
					LoadingData = true;
			
					if( obligation != null )
					{
						txtId.Text = obligation.ObligationId;
						cmbEffect.SelectedIndex = cmbEffect.FindStringExact( obligation.FulfillOn.ToString() );

						lstAttributeAssignments.Items.Clear();
						foreach( pol.AttributeAssignmentElementReadWrite attr in obligation.AttributeAssignment )
						{
							lstAttributeAssignments.Items.Add( attr );
						}
						txtAttributeId.Text = string.Empty;
						txtAttributeAssignemnt.Text = string.Empty;
					}
					index = -1;
				}
				finally
				{
					LoadingData = false;
				}
			}
			else
			{
				obligationIndex = -1;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstAttributeAssignments_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( index != -1 && index != lstAttributeAssignments.SelectedIndex )
			{
				pol.AttributeAssignmentElementReadWrite attrAux = lstAttributeAssignments.Items[index] as pol.AttributeAssignmentElementReadWrite;
				attrAux.AttributeId = txtAttributeId.Text;
				attrAux.Value = txtAttributeAssignemnt.Text;
				lstAttributeAssignments.Items.RemoveAt( index );
				lstAttributeAssignments.Items.Insert( index, attrAux );
			}
			pol.AttributeAssignmentElementReadWrite attr = lstAttributeAssignments.SelectedItem as pol.AttributeAssignmentElementReadWrite;
			index = lstAttributeAssignments.SelectedIndex;

			try
			{
				LoadingData = true;
				if( attr != null )
				{
					txtAttributeId.Text = attr.AttributeId;
					txtAttributeAssignemnt.Text = (string)attr.GetTypedValue( rtm.DataTypeDescriptor.String, 0 );
				}
			}
			finally
			{
				LoadingData = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			LoadingData = true;
			txtId.Text = string.Empty;
			lstAttributeAssignments.Items.Clear();
			txtAttributeAssignemnt.Text = string.Empty;
			txtAttributeId.Text = string.Empty;
			index = -1;
			obligationIndex = -1;
			pol.ObligationElementReadWrite oObligation = new pol.ObligationElementReadWrite(null,Xacml.XacmlVersion.Version11);
			lstObligations.Items.Add(oObligation);
			_obligations.Add( oObligation );
			LoadingData = false;
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			index = -1;
			obligationIndex = -1;
			pol.ObligationElementReadWrite obligation = lstObligations.SelectedItem as pol.ObligationElementReadWrite;

			try
			{
				LoadingData = true;
				
				txtId.Text = string.Empty;

				lstAttributeAssignments.Items.Clear();
				txtAttributeAssignemnt.Text = string.Empty;
				txtAttributeId.Text = string.Empty;
				_obligations.RemoveAt(lstObligations.SelectedIndex);
				lstObligations.Items.RemoveAt(lstObligations.SelectedIndex);
			}
			finally
			{
				LoadingData = false;
			}
		}

		private void btnAttributeAssignmentsAdd_Click(object sender, System.EventArgs e)
		{
			pol.ObligationElementReadWrite obligation = lstObligations.SelectedItem as pol.ObligationElementReadWrite;
			pol.AttributeAssignmentElementReadWrite oAtt = new pol.AttributeAssignmentElementReadWrite("TODO: Add AttributeId",Consts.Schema1.InternalDataTypes.XsdString,
				string.Empty, obligation.SchemaVersion );
			obligation.AttributeAssignment.Add( oAtt );
			lstAttributeAssignments.Items.Add( oAtt );
		}

		private void btnAttributeAssignmentsRemove_Click(object sender, System.EventArgs e)
		{
			index = -1;
			pol.ObligationElementReadWrite obligation = lstObligations.SelectedItem as pol.ObligationElementReadWrite;
			pol.AttributeAssignmentElementReadWrite attribute = lstAttributeAssignments.SelectedItem as pol.AttributeAssignmentElementReadWrite;

			try
			{
				LoadingData = true;
				
				txtAttributeAssignemnt.Text = string.Empty;
				txtAttributeId.Text = string.Empty;

				obligation.AttributeAssignment.RemoveAt(lstAttributeAssignments.SelectedIndex);
				lstAttributeAssignments.Items.RemoveAt(lstAttributeAssignments.SelectedIndex);
			}
			finally
			{
				LoadingData = false;
			}
		}

		/// <summary>
		/// Gets the ReadWriteObligationCollection
		/// </summary>
		public pol.ObligationReadWriteCollection ObligationsElement
		{
			get
			{
				if( index != -1 )
				{
					pol.AttributeAssignmentElementReadWrite attrAux = lstAttributeAssignments.Items[index] as pol.AttributeAssignmentElementReadWrite;
					attrAux.AttributeId = txtAttributeId.Text;
					attrAux.Value = txtAttributeAssignemnt.Text;
				}
				if( obligationIndex != -1 )
				{
					pol.ObligationElementReadWrite obliAux = lstObligations.Items[obligationIndex] as pol.ObligationElementReadWrite;
					obliAux.ObligationId = txtId.Text;
					if( cmbEffect.Text.Equals( pol.Effect.Deny.ToString()) )
					{
						obliAux.FulfillOn = pol.Effect.Deny;
					}
					else if( cmbEffect.Text.Equals( pol.Effect.Permit.ToString()) )
					{
						obliAux.FulfillOn = pol.Effect.Permit;
					}
				}
				return _obligations; 
			}
		}
	}
}
