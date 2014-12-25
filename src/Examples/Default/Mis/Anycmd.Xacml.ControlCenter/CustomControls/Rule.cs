using System.Drawing;

using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for PolicySet.
	/// </summary>
	public class Rule : BaseControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPolicyId;
		private pol.RuleElementReadWrite _rule;
		private System.Windows.Forms.ComboBox cmbEffect;
		private System.Windows.Forms.Button btnApply;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public Rule( pol.RuleElementReadWrite rule )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_rule = rule;

			LoadingData = true;

			cmbEffect.Items.Add( pol.Effect.Deny );
			cmbEffect.Items.Add( pol.Effect.Permit );

			txtDescription.Text = _rule.Description;
			txtPolicyId.Text = _rule.Id;
			cmbEffect.SelectedIndex = cmbEffect.FindStringExact( _rule.Effect.ToString() );

			txtDescription.DataBindings.Add( "Text", _rule, "Description" );
			txtPolicyId.DataBindings.Add( "Text", _rule, "Id" );
			cmbEffect.DataBindings.Add( "SelectedValue", _rule, "Effect" );

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
			this.label1 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPolicyId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbEffect = new System.Windows.Forms.ComboBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 72);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Description:";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(80, 72);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(504, 64);
			this.txtDescription.TabIndex = 1;
			this.txtDescription.Text = "";
			this.txtDescription.TextChanged += TextBox_TextChanged;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.TabIndex = 0;
			this.label2.Text = "Rule Id:";
			// 
			// txtPolicyId
			// 
			this.txtPolicyId.Location = new System.Drawing.Point(80, 8);
			this.txtPolicyId.Name = "txtPolicyId";
			this.txtPolicyId.Size = new System.Drawing.Size(504, 20);
			this.txtPolicyId.TabIndex = 1;
			this.txtPolicyId.Text = "";
			this.txtPolicyId.TextChanged += TextBox_TextChanged;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "Effect:";
			// 
			// cmbEffect
			// 
			this.cmbEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbEffect.Location = new System.Drawing.Point(80, 40);
			this.cmbEffect.Name = "cmbEffect";
			this.cmbEffect.Size = new System.Drawing.Size(504, 21);
			this.cmbEffect.TabIndex = 3;
			this.cmbEffect.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			// 
			// button1
			// 
			this.btnApply.Location = new System.Drawing.Point(256, 144);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 4;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.button1_Click);
			// 
			// Rule
			// 
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cmbEffect);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPolicyId);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Name = "Rule";
			this.Size = new System.Drawing.Size(592, 176);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			_rule.Description = txtDescription.Text;
			if( cmbEffect.Text.Equals( pol.Effect.Deny.ToString()) )
				_rule.Effect = pol.Effect.Deny;
			else if( cmbEffect.Text.Equals( pol.Effect.Permit.ToString()) )
				_rule.Effect = pol.Effect.Permit;
			_rule.Id = txtPolicyId.Text;

			ModifiedValue = false;

			txtDescription.BackColor = Color.White;
			txtPolicyId.BackColor = Color.White;
			cmbEffect.BackColor = Color.White;

		}
		/// <summary>
		/// 
		/// </summary>
		public pol.RuleElementReadWrite RuleElement
		{
			get{ return _rule; }
		}

	}
}
