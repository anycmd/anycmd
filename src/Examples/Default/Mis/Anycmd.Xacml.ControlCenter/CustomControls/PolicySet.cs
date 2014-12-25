using System.Drawing;

using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for PolicySet.
	/// </summary>
	public class PolicySet : BaseControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.GroupBox grpDefaults;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPolicySetId;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbPolicyCombiningAlgorithm;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtXPathVersion;
		private pol.PolicySetElementReadWrite _policySet;
		private System.Windows.Forms.Button btnApply;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Creates a new control that points to the policy set element specified.
		/// </summary>
		/// <param name="policySet"></param>
		public PolicySet( pol.PolicySetElementReadWrite policySet )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_policySet = policySet;

			LoadingData = true;

			cmbPolicyCombiningAlgorithm.Items.Add( Consts.Schema1.PolicyCombiningAlgorithms.DenyOverrides );
			cmbPolicyCombiningAlgorithm.Items.Add( Consts.Schema1.PolicyCombiningAlgorithms.PermitOverrides );
			cmbPolicyCombiningAlgorithm.Items.Add( Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable );
			cmbPolicyCombiningAlgorithm.Items.Add( Consts.Schema1.PolicyCombiningAlgorithms.OnlyOneApplicable );

			txtDescription.Text = _policySet.Description;
			txtPolicySetId.Text = _policySet.Id;
			txtXPathVersion.Text = _policySet.XPathVersion;
			cmbPolicyCombiningAlgorithm.SelectedText = _policySet.PolicyCombiningAlgorithm;

			txtDescription.DataBindings.Add( "Text", _policySet, "Description" );
			txtPolicySetId.DataBindings.Add( "Text", _policySet, "Id" );
			if(_policySet.XPathVersion != null)
				txtXPathVersion.DataBindings.Add( "Text", _policySet, "XPathVersion" );
			cmbPolicyCombiningAlgorithm.DataBindings.Add( "SelectedValue", policySet, "PolicyCombiningAlgorithm" );

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
			this.txtPolicySetId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbPolicyCombiningAlgorithm = new System.Windows.Forms.ComboBox();
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
			this.grpDefaults.Text = "PolicySet Defaults";
			// 
			// txtXPathVersion
			// 
			this.txtXPathVersion.Location = new System.Drawing.Point(80, 24);
			this.txtXPathVersion.Name = "txtXPathVersion";
			this.txtXPathVersion.Size = new System.Drawing.Size(488, 20);
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
			this.label2.Text = "PolicySet Id:";
			// 
			// txtPolicySetId
			// 
			this.txtPolicySetId.Location = new System.Drawing.Point(80, 80);
			this.txtPolicySetId.Name = "txtPolicySetId";
			this.txtPolicySetId.Size = new System.Drawing.Size(504, 20);
			this.txtPolicySetId.TabIndex = 1;
			this.txtPolicySetId.Text = "";
			this.txtPolicySetId.TextChanged += TextBox_TextChanged;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "Policy Combining Algorithm:";
			// 
			// cmbPolicyCombiningAlgorithm
			// 
			this.cmbPolicyCombiningAlgorithm.Location = new System.Drawing.Point(152, 112);
			this.cmbPolicyCombiningAlgorithm.Name = "cmbPolicyCombiningAlgorithm";
			this.cmbPolicyCombiningAlgorithm.Size = new System.Drawing.Size(432, 21);
			this.cmbPolicyCombiningAlgorithm.TabIndex = 3;
			this.cmbPolicyCombiningAlgorithm.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			// 
			// button1
			// 
			this.btnApply.Location = new System.Drawing.Point(248, 208);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 4;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.button1_Click);
			// 
			// PolicySet
			// 
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cmbPolicyCombiningAlgorithm);
			this.Controls.Add(this.grpDefaults);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPolicySetId);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Name = "PolicySet";
			this.Size = new System.Drawing.Size(592, 232);
			this.grpDefaults.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			_policySet.Description = txtDescription.Text;
			_policySet.Id = txtPolicySetId.Text;
			_policySet.XPathVersion = txtXPathVersion.Text;
			_policySet.PolicyCombiningAlgorithm = cmbPolicyCombiningAlgorithm.SelectedText;

			ModifiedValue = false;

			txtDescription.BackColor = Color.White;
			txtPolicySetId.BackColor = Color.White;
			txtXPathVersion.BackColor = Color.White;
			cmbPolicyCombiningAlgorithm.BackColor = Color.White;
		}
		/// <summary>
		/// 
		/// </summary>
		public pol.PolicySetElementReadWrite PolicySetElement
		{
			get{ return _policySet; }
		}

	}
}
