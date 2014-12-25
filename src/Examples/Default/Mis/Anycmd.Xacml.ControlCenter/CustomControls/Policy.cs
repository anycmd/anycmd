using System.Drawing;

using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for PolicySet.
	/// </summary>
	public class Policy : BaseControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.GroupBox grpDefaults;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtXPathVersion;
		private System.Windows.Forms.TextBox txtPolicyId;
		private System.Windows.Forms.ComboBox cmbRuleCombiningAlgorithm;
		private pol.PolicyElementReadWrite _policy;
		private System.Windows.Forms.Button btnApply;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public Policy( pol.PolicyElementReadWrite policy )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			LoadingData = true;

			_policy = policy;

			cmbRuleCombiningAlgorithm.Items.Add( Consts.Schema1.RuleCombiningAlgorithms.DenyOverrides );
			cmbRuleCombiningAlgorithm.Items.Add( Consts.Schema1.RuleCombiningAlgorithms.PermitOverrides );
			cmbRuleCombiningAlgorithm.Items.Add( Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable );

			txtDescription.Text = _policy.Description;
			txtPolicyId.Text = _policy.Id;
			txtXPathVersion.Text = _policy.XPathVersion;
			cmbRuleCombiningAlgorithm.SelectedText = _policy.RuleCombiningAlgorithm;

			txtDescription.DataBindings.Add( "Text", _policy, "Description" );
			txtPolicyId.DataBindings.Add( "Text", _policy, "Id" );
			if(_policy.XPathVersion != null)
				txtXPathVersion.DataBindings.Add( "Text", _policy, "XPathVersion" );
			cmbRuleCombiningAlgorithm.DataBindings.Add( "SelectedValue", policy, "RuleCombiningAlgorithm" );

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
			this.grpDefaults = new System.Windows.Forms.GroupBox();
			this.txtXPathVersion = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPolicyId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbRuleCombiningAlgorithm = new System.Windows.Forms.ComboBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.grpDefaults.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Description:";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(80, 8);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(504, 64);
			this.txtDescription.TabIndex = 1;
			this.txtDescription.Text = "";
			this.txtDescription.TextChanged += TextBox_TextChanged;
			// 
			// grpDefaults
			// 
			this.grpDefaults.Controls.Add(this.txtXPathVersion);
			this.grpDefaults.Controls.Add(this.label4);
			this.grpDefaults.Location = new System.Drawing.Point(8, 144);
			this.grpDefaults.Name = "grpDefaults";
			this.grpDefaults.Size = new System.Drawing.Size(576, 56);
			this.grpDefaults.TabIndex = 2;
			this.grpDefaults.TabStop = false;
			this.grpDefaults.Text = "Policy Defaults";
			// 
			// txtXPathVersion
			// 
			this.txtXPathVersion.Location = new System.Drawing.Point(88, 24);
			this.txtXPathVersion.Name = "txtXPathVersion";
			this.txtXPathVersion.Size = new System.Drawing.Size(480, 20);
			this.txtXPathVersion.TabIndex = 1;
			this.txtXPathVersion.Text = "";
			this.txtXPathVersion.TextChanged += TextBox_TextChanged;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(176, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "XPath version:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.TabIndex = 0;
			this.label2.Text = "Policy Id:";
			// 
			// txtPolicyId
			// 
			this.txtPolicyId.Location = new System.Drawing.Point(80, 80);
			this.txtPolicyId.Name = "txtPolicyId";
			this.txtPolicyId.Size = new System.Drawing.Size(504, 20);
			this.txtPolicyId.TabIndex = 1;
			this.txtPolicyId.Text = "";
			this.txtPolicyId.TextChanged += TextBox_TextChanged;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "Rule Combining Algorithm:";
			// 
			// cmbRuleCombiningAlgorithm
			// 
			this.cmbRuleCombiningAlgorithm.Location = new System.Drawing.Point(152, 112);
			this.cmbRuleCombiningAlgorithm.Name = "cmbRuleCombiningAlgorithm";
			this.cmbRuleCombiningAlgorithm.Size = new System.Drawing.Size(432, 21);
			this.cmbRuleCombiningAlgorithm.TabIndex = 3;
			this.cmbRuleCombiningAlgorithm.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			// 
			// button1
			// 
			this.btnApply.Location = new System.Drawing.Point(264, 208);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 4;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.button1_Click);
			// 
			// Policy
			// 
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cmbRuleCombiningAlgorithm);
			this.Controls.Add(this.grpDefaults);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPolicyId);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Name = "Policy";
			this.Size = new System.Drawing.Size(592, 232);
			this.grpDefaults.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			_policy.Description = txtDescription.Text;
			_policy.Id = txtPolicyId.Text;
			_policy.XPathVersion = txtXPathVersion.Text;
			_policy.RuleCombiningAlgorithm = cmbRuleCombiningAlgorithm.SelectedText;

			ModifiedValue = false;

			txtDescription.BackColor = Color.White;
			txtPolicyId.BackColor = Color.White;
			txtXPathVersion.BackColor = Color.White;
			cmbRuleCombiningAlgorithm.BackColor = Color.White;

		}


		/// <summary>
		/// 
		/// </summary>
		public pol.PolicyElementReadWrite PolicyElement
		{
			get{ return _policy; }
		}
	}
}
