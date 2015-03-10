using System;
using System.Drawing;
using System.Reflection;
using Anycmd.Xacml.Policy.TargetItems;
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
    /// <summary>
    /// Summary description for PolicySet.
    /// </summary>
    public class Match : BaseControl
    {
        private string _targetItemName;
        private int _index;
        private TargetMatchBaseReadWrite _match;
        private System.Windows.Forms.GroupBox grpMatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFunctions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAttributeValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpAttributeValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpAttributeDesignator;
        private System.Windows.Forms.TextBox txtAttributeId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIssuer;
        private System.Windows.Forms.ComboBox cmbADDataType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSubjectCategory;
        private System.Windows.Forms.GroupBox grpAttributeSelector;
        private System.Windows.Forms.ComboBox cmbASDataType;
        private System.Windows.Forms.ComboBox cmbAVDataType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkADMustBePresent;
        private System.Windows.Forms.CheckBox chkASMustBePresent;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rdbAttributeDesignator;
        private System.Windows.Forms.RadioButton rdbAttributeSelector;
        private System.Windows.Forms.TextBox txtRequestContextPath;
        private System.Windows.Forms.Label lblSubjectCategory;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetItem"></param>
        /// <param name="match"></param>
        /// <param name="index"></param>
        public Match(TargetItemBaseReadWrite targetItem, TargetMatchBaseReadWrite match, int index)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            LoadingData = true;

            foreach (FieldInfo field in typeof(Consts.Schema1.InternalDataTypes).GetFields())
            {
                cmbASDataType.Items.Add(field.GetValue(null));
                cmbADDataType.Items.Add(field.GetValue(null));
                cmbAVDataType.Items.Add(field.GetValue(null));
            }

            foreach (FieldInfo field in typeof(Consts.Schema1.InternalFunctions).GetFields())
            {
                cmbFunctions.Items.Add(field.GetValue(null));
            }

            txtSubjectCategory.Visible = false;
            lblSubjectCategory.Visible = false;

            _match = match;
            _index = index;

            if (targetItem is ActionElementReadWrite)
            {
                _targetItemName = "Action";
            }
            else if (targetItem is SubjectElementReadWrite)
            {
                _targetItemName = "Subject";
                txtSubjectCategory.Visible = true;
                lblSubjectCategory.Visible = true;
            }
            else if (targetItem is ResourceElementReadWrite)
            {
                _targetItemName = "Resource";
            }

            grpMatch.Text = _targetItemName + "Match";

            cmbFunctions.SelectedIndex = cmbFunctions.FindStringExact(_match.MatchId);
            cmbFunctions.DataBindings.Add("SelectedItem", _match, "MatchId");

            cmbAVDataType.SelectedIndex = cmbAVDataType.FindStringExact(match.AttributeValue.DataType);
            cmbAVDataType.DataBindings.Add("SelectedItem", _match.AttributeValue, "DataType");

            txtAttributeValue.Text = match.AttributeValue.Contents;
            txtAttributeValue.DataBindings.Add("Text", _match.AttributeValue, "Contents");

            if (match.AttributeReference is pol.AttributeDesignatorBase)
            {
                rdbAttributeDesignator.Checked = true;
                rdbAttributeSelector.Checked = false;

                cmbADDataType.SelectedIndex = cmbADDataType.FindStringExact(match.AttributeReference.DataType);
                cmbADDataType.DataBindings.Add("SelectedItem", _match.AttributeReference, "DataType");

                chkADMustBePresent.Checked = match.AttributeReference.MustBePresent;
                chkADMustBePresent.DataBindings.Add("Checked", _match.AttributeReference, "MustBePresent");

                pol.AttributeDesignatorBase attributeDesignator = (pol.AttributeDesignatorBase)match.AttributeReference;
                txtAttributeId.Text = attributeDesignator.AttributeId;
                txtAttributeId.DataBindings.Add("Text", _match.AttributeReference, "AttributeId");

                txtIssuer.Text = attributeDesignator.Issuer;
                txtIssuer.DataBindings.Add("Text", _match.AttributeReference, "Issuer");
            }
            else if (match.AttributeReference is pol.AttributeSelectorElement)
            {
                rdbAttributeDesignator.Checked = false;
                rdbAttributeSelector.Checked = true;

                cmbASDataType.SelectedIndex = cmbASDataType.FindStringExact(match.AttributeReference.DataType);
                cmbASDataType.DataBindings.Add("SelectedItem", _match.AttributeReference, "DataType");

                chkASMustBePresent.Checked = match.AttributeReference.MustBePresent;
                chkASMustBePresent.DataBindings.Add("Checked", _match.AttributeReference, "MustBePresent");

                txtRequestContextPath.Text = ((pol.AttributeSelectorElement)match.AttributeReference).RequestContextPath;
                txtRequestContextPath.DataBindings.Add("Text", _match.AttributeReference, "RequestContextPath");
            }

            LoadingData = false;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpMatch = new System.Windows.Forms.GroupBox();
            this.rdbAttributeSelector = new System.Windows.Forms.RadioButton();
            this.grpAttributeValue = new System.Windows.Forms.GroupBox();
            this.txtAttributeValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAVDataType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbFunctions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbAttributeDesignator = new System.Windows.Forms.RadioButton();
            this.grpAttributeDesignator = new System.Windows.Forms.GroupBox();
            this.txtSubjectCategory = new System.Windows.Forms.TextBox();
            this.chkADMustBePresent = new System.Windows.Forms.CheckBox();
            this.cmbADDataType = new System.Windows.Forms.ComboBox();
            this.txtAttributeId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSubjectCategory = new System.Windows.Forms.Label();
            this.txtIssuer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpAttributeSelector = new System.Windows.Forms.GroupBox();
            this.txtRequestContextPath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkASMustBePresent = new System.Windows.Forms.CheckBox();
            this.cmbASDataType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grpMatch.SuspendLayout();
            this.grpAttributeValue.SuspendLayout();
            this.grpAttributeDesignator.SuspendLayout();
            this.grpAttributeSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMatch
            // 
            this.grpMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMatch.Controls.Add(this.rdbAttributeSelector);
            this.grpMatch.Controls.Add(this.grpAttributeValue);
            this.grpMatch.Controls.Add(this.cmbFunctions);
            this.grpMatch.Controls.Add(this.label1);
            this.grpMatch.Controls.Add(this.rdbAttributeDesignator);
            this.grpMatch.Controls.Add(this.grpAttributeDesignator);
            this.grpMatch.Controls.Add(this.grpAttributeSelector);
            this.grpMatch.Location = new System.Drawing.Point(8, 8);
            this.grpMatch.Name = "grpMatch";
            this.grpMatch.Size = new System.Drawing.Size(576, 360);
            this.grpMatch.TabIndex = 3;
            this.grpMatch.TabStop = false;
            // 
            // rdbAttributeSelector
            // 
            this.rdbAttributeSelector.Location = new System.Drawing.Point(128, 168);
            this.rdbAttributeSelector.Name = "rdbAttributeSelector";
            this.rdbAttributeSelector.Size = new System.Drawing.Size(120, 24);
            this.rdbAttributeSelector.TabIndex = 10;
            this.rdbAttributeSelector.Text = "Attribute selector";
            this.rdbAttributeSelector.CheckedChanged += new System.EventHandler(this.rdbAttributeDesignator_CheckedChanged);
            // 
            // grpAttributeValue
            // 
            this.grpAttributeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAttributeValue.Controls.Add(this.txtAttributeValue);
            this.grpAttributeValue.Controls.Add(this.label2);
            this.grpAttributeValue.Controls.Add(this.cmbAVDataType);
            this.grpAttributeValue.Controls.Add(this.label8);
            this.grpAttributeValue.Location = new System.Drawing.Point(8, 48);
            this.grpAttributeValue.Name = "grpAttributeValue";
            this.grpAttributeValue.Size = new System.Drawing.Size(560, 112);
            this.grpAttributeValue.TabIndex = 6;
            this.grpAttributeValue.TabStop = false;
            this.grpAttributeValue.Text = "Attribute value";
            // 
            // txtAttributeValue
            // 
            this.txtAttributeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttributeValue.Location = new System.Drawing.Point(72, 56);
            this.txtAttributeValue.Multiline = true;
            this.txtAttributeValue.Name = "txtAttributeValue";
            this.txtAttributeValue.Size = new System.Drawing.Size(480, 48);
            this.txtAttributeValue.TabIndex = 3;
            this.txtAttributeValue.Text = "";
            this.txtAttributeValue.TextChanged += txtAttributeValue_TextChanged;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.TabIndex = 2;
            this.label2.Text = "Value:";
            // 
            // cmbAVDataType
            // 
            this.cmbAVDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAVDataType.Location = new System.Drawing.Point(72, 24);
            this.cmbAVDataType.Name = "cmbAVDataType";
            this.cmbAVDataType.Size = new System.Drawing.Size(480, 21);
            this.cmbAVDataType.TabIndex = 5;
            this.cmbAVDataType.SelectedIndexChanged += cmbAVDataType_SelectedIndexChanged;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.TabIndex = 4;
            this.label8.Text = "Data type:";
            // 
            // cmbFunctions
            // 
            this.cmbFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFunctions.Location = new System.Drawing.Point(72, 16);
            this.cmbFunctions.Name = "cmbFunctions";
            this.cmbFunctions.Size = new System.Drawing.Size(496, 21);
            this.cmbFunctions.TabIndex = 1;
            this.cmbFunctions.SelectedIndexChanged += cmbAVDataType_SelectedIndexChanged;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.TabIndex = 0;
            this.label1.Text = "Match Id:";
            // 
            // rdbAttributeDesignator
            // 
            this.rdbAttributeDesignator.Location = new System.Drawing.Point(8, 168);
            this.rdbAttributeDesignator.Name = "rdbAttributeDesignator";
            this.rdbAttributeDesignator.Size = new System.Drawing.Size(120, 24);
            this.rdbAttributeDesignator.TabIndex = 9;
            this.rdbAttributeDesignator.Text = "Attribute designator";
            this.rdbAttributeDesignator.CheckedChanged += new System.EventHandler(this.rdbAttributeDesignator_CheckedChanged);
            // 
            // grpAttributeDesignator
            // 
            this.grpAttributeDesignator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAttributeDesignator.Controls.Add(this.txtSubjectCategory);
            this.grpAttributeDesignator.Controls.Add(this.chkADMustBePresent);
            this.grpAttributeDesignator.Controls.Add(this.cmbADDataType);
            this.grpAttributeDesignator.Controls.Add(this.txtAttributeId);
            this.grpAttributeDesignator.Controls.Add(this.label3);
            this.grpAttributeDesignator.Controls.Add(this.label5);
            this.grpAttributeDesignator.Controls.Add(this.lblSubjectCategory);
            this.grpAttributeDesignator.Controls.Add(this.txtIssuer);
            this.grpAttributeDesignator.Controls.Add(this.label7);
            this.grpAttributeDesignator.Location = new System.Drawing.Point(8, 200);
            this.grpAttributeDesignator.Name = "grpAttributeDesignator";
            this.grpAttributeDesignator.Size = new System.Drawing.Size(560, 152);
            this.grpAttributeDesignator.TabIndex = 7;
            this.grpAttributeDesignator.TabStop = false;
            this.grpAttributeDesignator.Text = "Attribute designator";
            // 
            // txtSubjectCategory
            // 
            this.txtSubjectCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubjectCategory.Location = new System.Drawing.Point(96, 120);
            this.txtSubjectCategory.Name = "txtSubjectCategory";
            this.txtSubjectCategory.Size = new System.Drawing.Size(344, 20);
            this.txtSubjectCategory.TabIndex = 8;
            this.txtSubjectCategory.Text = "";
            this.txtSubjectCategory.TextChanged += txtAttributeValue_TextChanged;
            // 
            // chkADMustBePresent
            // 
            this.chkADMustBePresent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkADMustBePresent.BackColor = System.Drawing.SystemColors.Control;
            this.chkADMustBePresent.Location = new System.Drawing.Point(448, 120);
            this.chkADMustBePresent.Name = "chkADMustBePresent";
            this.chkADMustBePresent.TabIndex = 7;
            this.chkADMustBePresent.Text = "Must be present";
            this.chkADMustBePresent.CheckedChanged += chkADMustBePresent_CheckedChanged;
            // 
            // cmbADDataType
            // 
            this.cmbADDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbADDataType.Location = new System.Drawing.Point(96, 56);
            this.cmbADDataType.Name = "cmbADDataType";
            this.cmbADDataType.Size = new System.Drawing.Size(456, 21);
            this.cmbADDataType.TabIndex = 6;
            this.cmbADDataType.SelectedIndexChanged += cmbAVDataType_SelectedIndexChanged;
            // 
            // txtAttributeId
            // 
            this.txtAttributeId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttributeId.Location = new System.Drawing.Point(96, 24);
            this.txtAttributeId.Name = "txtAttributeId";
            this.txtAttributeId.Size = new System.Drawing.Size(456, 20);
            this.txtAttributeId.TabIndex = 5;
            this.txtAttributeId.Text = "";
            this.txtAttributeId.TextChanged += txtAttributeValue_TextChanged;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Attribute id:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Data type:";
            // 
            // lblSubjectCategory
            // 
            this.lblSubjectCategory.Location = new System.Drawing.Point(8, 120);
            this.lblSubjectCategory.Name = "lblSubjectCategory";
            this.lblSubjectCategory.Size = new System.Drawing.Size(96, 23);
            this.lblSubjectCategory.TabIndex = 4;
            this.lblSubjectCategory.Text = "Subject category:";
            // 
            // txtIssuer
            // 
            this.txtIssuer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIssuer.Location = new System.Drawing.Point(96, 88);
            this.txtIssuer.Name = "txtIssuer";
            this.txtIssuer.Size = new System.Drawing.Size(456, 20);
            this.txtIssuer.TabIndex = 5;
            this.txtIssuer.Text = "";
            this.txtIssuer.TextChanged += txtAttributeValue_TextChanged;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 23);
            this.label7.TabIndex = 4;
            this.label7.Text = "Issuer:";
            // 
            // grpAttributeSelector
            // 
            this.grpAttributeSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAttributeSelector.Controls.Add(this.txtRequestContextPath);
            this.grpAttributeSelector.Controls.Add(this.label9);
            this.grpAttributeSelector.Controls.Add(this.chkASMustBePresent);
            this.grpAttributeSelector.Controls.Add(this.cmbASDataType);
            this.grpAttributeSelector.Controls.Add(this.label4);
            this.grpAttributeSelector.Location = new System.Drawing.Point(8, 200);
            this.grpAttributeSelector.Name = "grpAttributeSelector";
            this.grpAttributeSelector.Size = new System.Drawing.Size(560, 152);
            this.grpAttributeSelector.TabIndex = 11;
            this.grpAttributeSelector.TabStop = false;
            this.grpAttributeSelector.Text = "Attribute selector";
            // 
            // txtRequestContextPath
            // 
            this.txtRequestContextPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestContextPath.Location = new System.Drawing.Point(72, 52);
            this.txtRequestContextPath.Multiline = true;
            this.txtRequestContextPath.Name = "txtRequestContextPath";
            this.txtRequestContextPath.Size = new System.Drawing.Size(480, 60);
            this.txtRequestContextPath.TabIndex = 8;
            this.txtRequestContextPath.Text = "";
            this.txtRequestContextPath.TextChanged += txtAttributeValue_TextChanged;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 44);
            this.label9.TabIndex = 7;
            this.label9.Text = "Request context path:";
            // 
            // chkASMustBePresent
            // 
            this.chkASMustBePresent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkASMustBePresent.Location = new System.Drawing.Point(448, 120);
            this.chkASMustBePresent.Name = "chkASMustBePresent";
            this.chkASMustBePresent.TabIndex = 6;
            this.chkASMustBePresent.Text = "Must be present";
            this.chkASMustBePresent.CheckedChanged += chkADMustBePresent_CheckedChanged;
            // 
            // cmbASDataType
            // 
            this.cmbASDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbASDataType.Location = new System.Drawing.Point(72, 24);
            this.cmbASDataType.Name = "cmbASDataType";
            this.cmbASDataType.Size = new System.Drawing.Size(480, 21);
            this.cmbASDataType.TabIndex = 5;
            this.cmbASDataType.SelectedIndexChanged += cmbAVDataType_SelectedIndexChanged;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 24);
            this.label4.Name = "label4";
            this.label4.TabIndex = 4;
            this.label4.Text = "Data type:";
            // 
            // Match
            // 
            this.Controls.Add(this.grpMatch);
            this.Name = "Match";
            this.Size = new System.Drawing.Size(592, 376);
            this.grpMatch.ResumeLayout(false);
            this.grpAttributeValue.ResumeLayout(false);
            this.grpAttributeDesignator.ResumeLayout(false);
            this.grpAttributeSelector.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void chkADMustBePresent_CheckedChanged(object sender, EventArgs e)
        {
            base.CheckBox_CheckedChanged(sender, e);
        }

        private void cmbAVDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.ComboBox_SelectedIndexChanged(sender, e);
        }

        private void txtAttributeValue_TextChanged(object sender, EventArgs e)
        {
            base.TextBox_TextChanged(sender, e);
        }
        #endregion

        private void rdbAttributeDesignator_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!LoadingData)
            {
                rdbAttributeDesignator.BackColor = Color.GreenYellow;
                rdbAttributeSelector.BackColor = Color.GreenYellow;
            }
            if (rdbAttributeDesignator.Checked)
            {
                grpAttributeDesignator.Visible = true;
                grpAttributeSelector.Visible = false;
            }
            if (rdbAttributeSelector.Checked)
            {
                grpAttributeDesignator.Visible = false;
                grpAttributeSelector.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TargetMatchBaseReadWrite MatchElement
        {
            get
            {
                LoadingData = true;

                _match.MatchId = cmbFunctions.Text;
                cmbFunctions.BackColor = Color.White;

                _match.AttributeValue.DataType = cmbAVDataType.Text;
                cmbAVDataType.BackColor = Color.White;

                _match.AttributeValue.Contents = txtAttributeValue.Text;
                txtAttributeValue.BackColor = Color.White;

                if (rdbAttributeDesignator.Checked)
                {
                    if (_match is ActionMatchElementReadWrite)
                        _match.AttributeReference = new ActionAttributeDesignatorElement(cmbADDataType.Text, chkADMustBePresent.Checked,
                            txtAttributeId.Text, txtIssuer.Text, _match.SchemaVersion);
                    else if (_match is ResourceMatchElementReadWrite)
                        _match.AttributeReference = new ResourceAttributeDesignatorElement(cmbADDataType.Text, chkADMustBePresent.Checked,
                            txtAttributeId.Text, txtIssuer.Text, _match.SchemaVersion);
                    else if (_match is EnvironmentMatchElementReadWrite)
                        _match.AttributeReference = new EnvironmentAttributeDesignatorElement(cmbADDataType.Text, chkADMustBePresent.Checked,
                            txtAttributeId.Text, txtIssuer.Text, _match.SchemaVersion);
                    else if (_match is SubjectMatchElementReadWrite)
                        _match.AttributeReference = new SubjectAttributeDesignatorElement(cmbADDataType.Text, chkADMustBePresent.Checked,
                            txtAttributeId.Text, txtIssuer.Text, txtSubjectCategory.Text, _match.SchemaVersion);
                    txtAttributeId.BackColor = Color.White;
                    cmbADDataType.BackColor = Color.White;
                    rdbAttributeDesignator.BackColor = System.Windows.Forms.Control.DefaultBackColor;
                    chkADMustBePresent.BackColor = System.Windows.Forms.Control.DefaultBackColor;
                    txtIssuer.BackColor = Color.White;
                }
                else if (rdbAttributeSelector.Checked)
                {
                    _match.AttributeReference = new pol.AttributeSelectorElement(cmbASDataType.Text, chkASMustBePresent.Checked,
                        txtRequestContextPath.Text, _match.SchemaVersion);
                    rdbAttributeSelector.BackColor = System.Windows.Forms.Control.DefaultBackColor;
                    cmbASDataType.BackColor = Color.White;
                    chkASMustBePresent.BackColor = System.Windows.Forms.Control.DefaultBackColor;
                    txtRequestContextPath.BackColor = Color.White;
                }

                LoadingData = false;

                return _match;

            }
        }
    }
}
