using Anycmd.Xacml.Policy.TargetItems;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Anycmd.Xacml.ControlCenter
{
	using ContextTreeNodes;
	using Policy;
	using System.Xml;
	using TreeNodes;
	using Xacml;
	using con = Context;
	using pol = Policy;

	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : Form
	{
		private MenuItem _menuItemFile;
		private TreeView _mainTree;
		private ImageList _mainImageList;
		private MenuItem _menuItemOpenPolicy;
		private Panel _panel1;
		private Splitter _splitter1;
		private ContextMenu _contextMenu;
		private Panel _mainPanel;
		private MainMenu _mainMenu;
		private MenuItem _menuItemHelp;
		private MenuItem _menuItemAbout;
		private MenuItem _mniCreateNew;
		private MenuItem _mniDelete;
		private OpenFileDialog _openFileDialog;
		private MenuItem _menuItemSaveAs;
		private SaveFileDialog _saveFileDialog;
		private System.ComponentModel.IContainer components;
		private MenuItem _menuItemClose;
		private MenuItem _menuItemOpenRequest;
		private string _path = string.Empty;
		private MenuItem _menuItemContext;
		private MenuItem _menuItemRunPolicy;
		private MenuItem _menuItemRunRequest;
		private MenuItem _menuItemSave;
		private DocumentType _docType;

		/// <summary>
		/// 
		/// </summary>
		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._mainTree = new System.Windows.Forms.TreeView();
			this._contextMenu = new System.Windows.Forms.ContextMenu();
			this._mniCreateNew = new System.Windows.Forms.MenuItem();
			this._mniDelete = new System.Windows.Forms.MenuItem();
			this._mainImageList = new System.Windows.Forms.ImageList(this.components);
			this._mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this._menuItemFile = new System.Windows.Forms.MenuItem();
			this._menuItemOpenPolicy = new System.Windows.Forms.MenuItem();
			this._menuItemOpenRequest = new System.Windows.Forms.MenuItem();
			this._menuItemSave = new System.Windows.Forms.MenuItem();
			this._menuItemSaveAs = new System.Windows.Forms.MenuItem();
			this._menuItemClose = new System.Windows.Forms.MenuItem();
			this._menuItemContext = new System.Windows.Forms.MenuItem();
			this._menuItemRunPolicy = new System.Windows.Forms.MenuItem();
			this._menuItemRunRequest = new System.Windows.Forms.MenuItem();
			this._menuItemHelp = new System.Windows.Forms.MenuItem();
			this._menuItemAbout = new System.Windows.Forms.MenuItem();
			this._panel1 = new System.Windows.Forms.Panel();
			this._mainPanel = new System.Windows.Forms.Panel();
			this._splitter1 = new System.Windows.Forms.Splitter();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _mainTree
			// 
			this._mainTree.AllowDrop = true;
			this._mainTree.ContextMenu = this._contextMenu;
			this._mainTree.Dock = System.Windows.Forms.DockStyle.Left;
			this._mainTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._mainTree.FullRowSelect = true;
			this._mainTree.HotTracking = true;
			this._mainTree.ImageIndex = 0;
			this._mainTree.ImageList = this._mainImageList;
			this._mainTree.Location = new System.Drawing.Point(0, 0);
			this._mainTree.Name = "_mainTree";
			this._mainTree.SelectedImageIndex = 0;
			this._mainTree.Size = new System.Drawing.Size(342, 625);
			this._mainTree.TabIndex = 0;
			this._mainTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.mainTree_BeforeSelect);
			this._mainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTree_AfterSelect);
			this._mainTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainTree_MouseDown);
			// 
			// _contextMenu
			// 
			this._contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this._mniCreateNew,
			this._mniDelete});
			this._contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// _mniCreateNew
			// 
			this._mniCreateNew.Index = 0;
			this._mniCreateNew.Text = "Create new";
			// 
			// _mniDelete
			// 
			this._mniDelete.Index = 1;
			this._mniDelete.Text = "Delete";
			this._mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
			// 
			// _mainImageList
			// 
			this._mainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this._mainImageList.ImageSize = new System.Drawing.Size(16, 16);
			this._mainImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _mainMenu
			// 
			this._mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this._menuItemFile,
			this._menuItemContext,
			this._menuItemHelp});
			// 
			// _menuItemFile
			// 
			this._menuItemFile.Index = 0;
			this._menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this._menuItemOpenPolicy,
			this._menuItemOpenRequest,
			this._menuItemSave,
			this._menuItemSaveAs,
			this._menuItemClose});
			this._menuItemFile.Text = "File";
			// 
			// _menuItemOpenPolicy
			// 
			this._menuItemOpenPolicy.Index = 0;
			this._menuItemOpenPolicy.Text = "Open Policy...";
			this._menuItemOpenPolicy.Click += new System.EventHandler(this.menuItemOpenPolicy_Click);
			// 
			// _menuItemOpenRequest
			// 
			this._menuItemOpenRequest.Index = 1;
			this._menuItemOpenRequest.Text = "Open Request...";
			this._menuItemOpenRequest.Click += new System.EventHandler(this.menuItemOpenRequest_Click);
			// 
			// _menuItemSave
			// 
			this._menuItemSave.Enabled = false;
			this._menuItemSave.Index = 2;
			this._menuItemSave.Text = "Save";
			this._menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
			// 
			// _menuItemSaveAs
			// 
			this._menuItemSaveAs.Enabled = false;
			this._menuItemSaveAs.Index = 3;
			this._menuItemSaveAs.Text = "Save as...";
			this._menuItemSaveAs.Click += new System.EventHandler(this.menuItemSaveAs_Click);
			// 
			// _menuItemClose
			// 
			this._menuItemClose.Index = 4;
			this._menuItemClose.Text = "Close";
			this._menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
			// 
			// _menuItemContext
			// 
			this._menuItemContext.Index = 1;
			this._menuItemContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this._menuItemRunPolicy,
			this._menuItemRunRequest});
			this._menuItemContext.Text = "Context";
			// 
			// _menuItemRunPolicy
			// 
			this._menuItemRunPolicy.Enabled = false;
			this._menuItemRunPolicy.Index = 0;
			this._menuItemRunPolicy.Text = "Run with policy...";
			this._menuItemRunPolicy.Click += new System.EventHandler(this.menuItemRunPolicy_Click);
			// 
			// _menuItemRunRequest
			// 
			this._menuItemRunRequest.Enabled = false;
			this._menuItemRunRequest.Index = 1;
			this._menuItemRunRequest.Text = "Run with request...";
			this._menuItemRunRequest.Click += new System.EventHandler(this.menuItemRunRequest_Click);
			// 
			// _menuItemHelp
			// 
			this._menuItemHelp.Index = 2;
			this._menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this._menuItemAbout});
			this._menuItemHelp.Text = "Help";
			// 
			// _menuItemAbout
			// 
			this._menuItemAbout.Index = 0;
			this._menuItemAbout.Text = "About";
			// 
			// _panel1
			// 
			this._panel1.Controls.Add(this._mainPanel);
			this._panel1.Controls.Add(this._splitter1);
			this._panel1.Controls.Add(this._mainTree);
			this._panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panel1.Location = new System.Drawing.Point(0, 0);
			this._panel1.Name = "_panel1";
			this._panel1.Size = new System.Drawing.Size(964, 625);
			this._panel1.TabIndex = 3;
			// 
			// _mainPanel
			// 
			this._mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._mainPanel.Location = new System.Drawing.Point(346, 0);
			this._mainPanel.Name = "_mainPanel";
			this._mainPanel.Size = new System.Drawing.Size(618, 625);
			this._mainPanel.TabIndex = 4;
			// 
			// _splitter1
			// 
			this._splitter1.Location = new System.Drawing.Point(342, 0);
			this._splitter1.Name = "_splitter1";
			this._splitter1.Size = new System.Drawing.Size(4, 625);
			this._splitter1.TabIndex = 3;
			this._splitter1.TabStop = false;
			// 
			// _openFileDialog
			// 
			this._openFileDialog.DefaultExt = "xml";
			this._openFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
			this._openFileDialog.InitialDirectory = ".";
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.CreatePrompt = true;
			this._saveFileDialog.DefaultExt = "xml";
			this._saveFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(964, 625);
			this.Controls.Add(this._panel1);
			this.Menu = this._mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Xacml±à¼­Æ÷";
			this._panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItemOpenPolicy_Click(object sender, System.EventArgs e)
		{
			_openFileDialog.Filter = @"Policy Files|*.xml|All Files|*.*";
			if (_openFileDialog.ShowDialog() == DialogResult.OK)
			{
				System.IO.Stream stream = _openFileDialog.OpenFile();
				pol.PolicyDocumentReadWrite doc = PolicyLoader.LoadPolicyDocument(stream, XacmlVersion.Version11, DocumentAccess.ReadWrite);
				_path = _openFileDialog.FileName;
				_mainTree.Nodes.Add(new TreeNodes.PolicyDocument(doc));
				_docType = DocumentType.Policy;
				_menuItemSaveAs.Enabled = true;
				_menuItemSave.Enabled = true;
				_menuItemOpenPolicy.Enabled = false;
				_menuItemOpenRequest.Enabled = false;
				_menuItemRunRequest.Enabled = true;
				_menuItemRunPolicy.Enabled = false;
				stream.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			// Clear the main panel
			_mainPanel.Controls.Clear();

			// If the control have been instantiated before use it, otherwise create the control.
			if (e.Node.Tag != null)
			{
				_mainPanel.Controls.Add((Control)e.Node.Tag);
			}
			else
			{
				// Create the control depending on the node type.
				var policySet = e.Node as PolicySet;
				if (policySet != null)
				{
					_mainPanel.Controls.Add(new CustomControls.PolicySet(policySet.PolicySetDefinition));
				}
				else
				{
					var policy = e.Node as Policy;
					if (policy != null)
					{
						_mainPanel.Controls.Add(new CustomControls.Policy(policy.PolicyDefinition));
					}
					else if (e.Node is PolicyIdReference)
					{
					}
					else if (e.Node is PolicySetIdReference)
					{
					}
					else if (e.Node is Target)
					{
					}
					else
					{
						var obligations = e.Node as Obligations;
						if (obligations != null)
						{
							_mainPanel.Controls.Add(new CustomControls.Obligations(obligations.ObligationDefinition));
						}
						else
						{
							var targetItem = e.Node as TargetItem;
							if (targetItem != null)
							{
								_mainPanel.Controls.Add(new CustomControls.TargetItem(targetItem.TargetItemDefinition));
							}
							else
							{
								var rule = e.Node as Rule;
								if (rule != null)
								{
									_mainPanel.Controls.Add(new CustomControls.Rule(rule.RuleDefinition));
								}
								else
								{
									var condition = e.Node as Condition;
									if (condition != null)
									{
										_mainPanel.Controls.Add(new CustomControls.Condition(condition.ConditionDefinition));
									}
									else
									{
										var attribute = e.Node as Attribute;
										if (attribute != null)
										{
											_mainPanel.Controls.Add(new ContextCustomControls.Attribute(attribute.AttributeDefinition));
										}
										else
										{
											var resource = e.Node as Resource;
											if (resource != null)
											{
												_mainPanel.Controls.Add(new ContextCustomControls.Resource(resource.ResourceDefinition));
											}
										}
									}
								}
							}
						}
					}
				}

				// If the control was created and added successfully, Dock it and keep the 
				// instance in the tree node.
				if (_mainPanel.Controls.Count != 0)
				{
					_mainPanel.Controls[0].Dock = DockStyle.Fill;
					e.Node.Tag = _mainPanel.Controls[0];
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenu_Popup(object sender, EventArgs e)
		{
			_mniCreateNew.MenuItems.Clear();
			if (_mainTree.SelectedNode == null)
			{
				if (_mainTree.Nodes.Count == 0)
				{
					_mniCreateNew.MenuItems.Add("PolicyDocument", new EventHandler(CreatePolicyDocument));
					_mniCreateNew.MenuItems.Add("ContextDocument", new EventHandler(CreateContextDocument));
				}
			}
			else
			{
				var policyDocument = _mainTree.SelectedNode as TreeNodes.PolicyDocument;
				if (policyDocument != null)
				{
					if (policyDocument.PolicyDocumentDefinition.Policy == null &&
						policyDocument.PolicyDocumentDefinition.PolicySet == null)
					{
						_mniCreateNew.MenuItems.Add("Policy", CreatePolicyFromDocument);
						_mniCreateNew.MenuItems.Add("PolicySet", CreatePolicySetFromDocument);
					}
				}
				else
				{
					var policySet = _mainTree.SelectedNode as PolicySet;
					if (policySet != null)
					{
						_mniCreateNew.MenuItems.Add("Policy", CreatePolicy);
						_mniCreateNew.MenuItems.Add("PolicySet", CreatePolicySet);
						if (policySet.PolicySetDefinition.Target == null)
						{
							_mniCreateNew.MenuItems.Add("Target", CreateTarget);
						}
						if (policySet.PolicySetDefinition.Obligations == null)
						{
							_mniCreateNew.MenuItems.Add("Obligations", CreateObligationsFromPolicySet);
						}
					}
					else
					{
						var policy = _mainTree.SelectedNode as Policy;
						if (policy != null)
						{
							_mniCreateNew.MenuItems.Add("Rule", CreateRule);
							if (policy.PolicyDefinition.Target == null)
							{
								_mniCreateNew.MenuItems.Add("Target", CreateTarget);
							}
							if (policy.PolicyDefinition.Obligations == null)
							{
								_mniCreateNew.MenuItems.Add("Obligations", CreateObligationsFromPolicy);
							}
						}
						else
						{
							var rule = _mainTree.SelectedNode as Rule;
							if (rule != null)
							{
								if (rule.RuleDefinition.Condition == null)
								{
									_mniCreateNew.MenuItems.Add("Condition", CreateCondition);
								}
								if (rule.RuleDefinition.Target == null)
								{
									_mniCreateNew.MenuItems.Add("Target", CreateTarget);
								}
							}
							else if (_mainTree.SelectedNode is PolicyIdReference)
							{
							}
							else if (_mainTree.SelectedNode is PolicySetIdReference)
							{
							}
							else if (_mainTree.SelectedNode is Obligations)
							{
							}
							else if (_mainTree.SelectedNode is Target)
							{

							}
							else if (_mainTree.SelectedNode is TargetItem)
							{
							}
							else if (_mainTree.SelectedNode is AnyTarget)
							{
								_mniCreateNew.MenuItems.Add("Target", CreateTarget);
							}
							else if (_mainTree.SelectedNode is AnySubject)
							{
								_mniCreateNew.MenuItems.Add("Subject", CreateTargetItem);
							}
							else if (_mainTree.SelectedNode is AnyAction)
							{
								_mniCreateNew.MenuItems.Add("Action", CreateTargetItem);
							}
							else if (_mainTree.SelectedNode is AnyResource)
							{
								_mniCreateNew.MenuItems.Add("Resource", CreateTargetItem);
							}
							else if (_mainTree.SelectedNode is Condition)
							{
							}
							else
							{
								var request = _mainTree.SelectedNode as Request;
								if (request != null)
								{
									if (request.RequestDefinition.Action == null)
									{
										_mniCreateNew.MenuItems.Add("Action", CreateContextActionElement);
									}
									_mniCreateNew.MenuItems.Add("Resource", CreateContextResourceElement);
									_mniCreateNew.MenuItems.Add("Subject", CreateContextSubjectElement);
								}
								else if (_mainTree.SelectedNode is Action || _mainTree.SelectedNode is Resource ||
										 _mainTree.SelectedNode is Subject)
								{
									_mniCreateNew.MenuItems.Add("Attribute", CreateContextAttributeElement);
								}
								else
								{
									var context = _mainTree.SelectedNode as Context;
									if (context != null)
									{
										if (context.ContextDefinition.Request == null)
										{
											_mniCreateNew.MenuItems.Add("Request", CreateContextRequest);
										}
									}
								}
							}
						}
					}
				}
			}

			if (_mniCreateNew.MenuItems.Count == 0)
			{
				_mniCreateNew.Visible = false;
			}
			else
			{
				_mniCreateNew.Visible = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_mainTree.SelectedNode = _mainTree.GetNodeAt(e.X, e.Y);
			}
		}

		private void CreateContextSubjectElement(object sender, EventArgs args)
		{
			var newSubject = new con.SubjectElementReadWrite(string.Empty, new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			var requestNode = (Request)_mainTree.SelectedNode;
			var newNode = new Subject(newSubject);
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			if (request.Subjects == null)
			{
				request.Subjects = new con.SubjectReadWriteCollection();
			}
			request.Subjects.Add(newSubject);
			requestNode.Nodes.Add(newNode);
		}

		private void CreateContextAttributeElement(object sender, EventArgs args)
		{
			var node = (NoBoldNode)_mainTree.SelectedNode;

			if (node is Action)
			{
				var actionNode = (Action)_mainTree.SelectedNode;
				var action = actionNode.ActionDefinition;
				var attribute = new con.AttributeElementReadWrite(string.Empty, string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				var attributeNode = new Attribute(attribute);

				action.Attributes.Add(attribute);
				actionNode.Nodes.Add(attributeNode);
			}
			else if (node is Resource)
			{
				var resourceNode = (Resource)_mainTree.SelectedNode;
				con.ResourceElementReadWrite resource = resourceNode.ResourceDefinition;
				var attribute = new con.AttributeElementReadWrite(string.Empty, string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				var attributeNode = new Attribute(attribute);

				resource.Attributes.Add(attribute);
				resourceNode.Nodes.Add(attributeNode);
			}
			else if (node is Subject)
			{
				var subjectNode = (Subject)_mainTree.SelectedNode;
				con.SubjectElementReadWrite subject = subjectNode.SubjectDefinition;
				var attribute = new con.AttributeElementReadWrite("urn:new_attribute", string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				var attributeNode = new Attribute(attribute);

				subject.Attributes.Add(attribute);
				subjectNode.Nodes.Add(attributeNode);
			}
		}

		private void CreateContextActionElement(object sender, EventArgs args)
		{
			var newAction = new con.ActionElementReadWrite(new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			var requestNode = (Request)_mainTree.SelectedNode;
			var newNode = new Action(newAction);
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			request.Action = newAction;
			requestNode.Nodes.Add(newNode);
		}
		private void CreateContextResourceElement(object sender, EventArgs args)
		{
			var newResource = new con.ResourceElementReadWrite(null, con.ResourceScope.Immediate, new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			var requestNode = (Request)_mainTree.SelectedNode;
			var newNode = new Resource(newResource);
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			if (request.Resources == null)
			{
				request.Resources = new con.ResourceReadWriteCollection();
			}
			request.Resources.Add(newResource);
			requestNode.Nodes.Add(newNode);
		}

		/// <summary>
		/// Creates a new context document
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">THe arguements for the event.</param>
		private void CreateContextDocument(object sender, EventArgs args)
		{
			// Create a new policydocument
			var newContext = new con.ContextDocumentReadWrite(); //TODO: check version

			newContext.Namespaces.Add(string.Empty, Consts.Schema1.Namespaces.Context);
			newContext.Namespaces.Add("xsi", Consts.Schema1.Namespaces.Xsi);
			var newNode = new Context(newContext);
			_mainTree.Nodes.Add(newNode);
			_docType = DocumentType.Request;
			newNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
			_menuItemOpenPolicy.Enabled = false;
			_menuItemOpenRequest.Enabled = false;
			_menuItemSaveAs.Enabled = true;
			_menuItemSave.Enabled = true;
			_menuItemRunPolicy.Enabled = true;
			_menuItemRunRequest.Enabled = false;
		}

		private void CreateContextRequest(object sender, EventArgs args)
		{
			var newRequest = new con.RequestElementReadWrite(null, null, null, null, XacmlVersion.Version11);
			var contextNode = (Context)_mainTree.SelectedNode;
			var requestNode = new Request(newRequest);
			con.ContextDocumentReadWrite context = contextNode.ContextDefinition;

			context.Request = newRequest;
			contextNode.Nodes.Add(requestNode);
		}

		/// <summary>
		/// Creates a new policy document
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicyDocument(object sender, EventArgs args)
		{
			// Create a new policydocument
			var newPolicyDoc = new PolicyDocumentReadWrite(XacmlVersion.Version11); //TODO: check version

			newPolicyDoc.Namespaces.Add(string.Empty, Consts.Schema1.Namespaces.Policy);
			newPolicyDoc.Namespaces.Add("xsi", Consts.Schema1.Namespaces.Xsi);
			var newNode = new TreeNodes.PolicyDocument(newPolicyDoc);
			_mainTree.Nodes.Add(newNode);
			_docType = DocumentType.Policy;

			newNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
			_menuItemOpenPolicy.Enabled = false;
			_menuItemOpenRequest.Enabled = false;
			_menuItemSaveAs.Enabled = true;
			_menuItemSave.Enabled = true;
			_menuItemRunRequest.Enabled = true;
			_menuItemRunPolicy.Enabled = false;
		}

		/// <summary>
		/// Creates a new policy for the policy set selected.
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicy(object sender, EventArgs args)
		{
			var policySetNode = (PolicySet)_mainTree.SelectedNode;
			PolicySetElementReadWrite policySet = policySetNode.PolicySetDefinition;

			// Create a new policy
			var newPolicy = new PolicyElementReadWrite(
				"urn:newpolicy", "[TODO: add a description]",
				null,
				new RuleReadWriteCollection(),
				Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable,
				new ObligationReadWriteCollection(),
				string.Empty,
				null,
				null,
				null,
				Xacml.XacmlVersion.Version11); //TODO: check version

			// Add the policy to the policySet.
			policySet.Policies.Add(newPolicy);

			// Create a new node
			var policyNode = new Policy(newPolicy);

			// Add the tree node.
			policySetNode.Nodes.Add(policyNode);

			// Set the font so the user knows the item was changed
			policyNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		/// <summary>
		/// Creates a new policy for the policy document selected.
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicyFromDocument(object sender, EventArgs args)
		{
			var policyDocumentNode = (TreeNodes.PolicyDocument)_mainTree.SelectedNode;
			var policyDocument = policyDocumentNode.PolicyDocumentDefinition;

			// Create a new policy
			var newPolicy = new PolicyElementReadWrite(
				"urn:newpolicy", "[TODO: add a description]", null,
				new RuleReadWriteCollection(),
				Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable,
				new ObligationReadWriteCollection(),
				string.Empty,
				null,
				null,
				null,
				XacmlVersion.Version11); //TODO: check version


			policyDocument.Policy = newPolicy;

			// Create a new node
			var policyNode = new Policy(newPolicy);

			// Add the tree node.
			policyDocumentNode.Nodes.Add(policyNode);

			// Set the font so the user knows the item was changed
			policyNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreatePolicySet(object sender, EventArgs args)
		{
			var policySetNode = (PolicySet)_mainTree.SelectedNode;
			var policySet = policySetNode.PolicySetDefinition;

			// Create a new policy
			var newPolicySet = new PolicySetElementReadWrite(
				"urn:newpolicy", "[TODO: add a description]",
				null,
				new ArrayList(),
				Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable,
				new ObligationReadWriteCollection(),
				null,
				XacmlVersion.Version11); //TODO: check version

			// Add the policy to the policySet.
			policySet.Policies.Add(newPolicySet);

			// Create a new node.
			var newPolicySetNode = new PolicySet(newPolicySet);

			// Add the tree node.
			policySetNode.Nodes.Add(newPolicySetNode);

			// Set the font so the user knows the item was changed
			newPolicySetNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreatePolicySetFromDocument(object sender, EventArgs args)
		{
			var policyDocumentNode = (TreeNodes.PolicyDocument)_mainTree.SelectedNode;
			PolicyDocumentReadWrite policyDoc = policyDocumentNode.PolicyDocumentDefinition;

			// Create a new policy
			var newPolicySet = new pol.PolicySetElementReadWrite(
				"urn:newpolicy", "[TODO: add a description]",
				null,
				new ArrayList(),
				Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable,
				new ObligationReadWriteCollection(),
				null,
				XacmlVersion.Version11); //TODO: check version

			policyDoc.PolicySet = newPolicySet;

			// Create a new node.
			var newPolicySetNode = new PolicySet(newPolicySet);

			// Add the tree node.
			policyDocumentNode.Nodes.Add(newPolicySetNode);

			// Set the font so the user knows the item was changed
			newPolicySetNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateTarget(object sender, EventArgs args)
		{
			Target newTargetNode = null;
			TreeNode parentNode = null;

			// Create the target
			var target = new TargetElementReadWrite(
				new ResourcesElementReadWrite(true, new TargetItemReadWriteCollection(), XacmlVersion.Version11), //TODO: check version
				new SubjectsElementReadWrite(true, new TargetItemReadWriteCollection(), XacmlVersion.Version11), //TODO: check version 
				new ActionsElementReadWrite(true, new TargetItemReadWriteCollection(), XacmlVersion.Version11), //TODO: check version
				new EnvironmentsElementReadWrite(true, new TargetItemReadWriteCollection(), XacmlVersion.Version11), //TODO: check version
				XacmlVersion.Version11); //TODO: check version

			// Create the node
			newTargetNode = new Target(target);

			if (_mainTree.SelectedNode is PolicySet)
			{
				parentNode = _mainTree.SelectedNode;
				PolicySetElementReadWrite policySet = ((PolicySet)parentNode).PolicySetDefinition;

				// Set the target
				policySet.Target = target;
			}
			else if (_mainTree.SelectedNode is Policy)
			{
				parentNode = _mainTree.SelectedNode;
				PolicyElementReadWrite policy = ((Policy)parentNode).PolicyDefinition;

				// Set the target
				policy.Target = target;
			}
			else if (_mainTree.SelectedNode is Rule)
			{
				parentNode = _mainTree.SelectedNode;
				RuleElementReadWrite rule = ((Rule)parentNode).RuleDefinition;

				// Set the target
				rule.Target = target;
			}
			else if (_mainTree.SelectedNode is AnyTarget)
			{
				parentNode = _mainTree.SelectedNode.Parent;

				// Set the target
				var policySet = parentNode as PolicySet;
				if (policySet != null)
				{
					policySet.PolicySetDefinition.Target = target;
				}
				else
				{
					var policy = parentNode as Policy;
					if (policy != null)
					{
						policy.PolicyDefinition.Target = target;
					}
					else
					{
						var rule = parentNode as Rule;
						if (rule != null)
						{
							rule.RuleDefinition.Target = target;
						}
					}
				}
			}

			if (parentNode != null)
			{
				int idx = -1;

				// Search the previous node
				foreach (TreeNode node in parentNode.Nodes)
				{
					if (node is AnyTarget)
					{
						idx = parentNode.Nodes.IndexOf(node);
						break;
					}
				}

				if (idx != -1)
				{
					// Set the font to the node
					newTargetNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);

					// Remove the previous target node
					parentNode.Nodes.RemoveAt(idx);

					// Add the node to the node.
					parentNode.Nodes.Insert(idx, newTargetNode);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateRule(object sender, EventArgs args)
		{
			var policyNode = (Policy)_mainTree.SelectedNode;
			PolicyElementReadWrite policy = policyNode.PolicyDefinition;

			var rule = new RuleElementReadWrite(
				"urn:new_rule",
				"[TODO: add rule description]",
				null,
				null,
				Effect.Permit,
				XacmlVersion.Version11);  //TODO: check version

			policy.Rules.Add(rule);

			var ruleNode = new Rule(rule);

			policyNode.Nodes.Add(ruleNode);

			ruleNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		private void CreateObligationsFromPolicy(object sender, EventArgs args)
		{
			var policyNode = (Policy)_mainTree.SelectedNode;
			PolicyElementReadWrite policy = policyNode.PolicyDefinition;

			var obligations = new ObligationReadWriteCollection();  //TODO: check version

			policy.Obligations = obligations;

			var obligationsNode = new Obligations(obligations);

			policyNode.Nodes.Add(obligationsNode);

			obligationsNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		private void CreateObligationsFromPolicySet(object sender, EventArgs args)
		{
			var policySetNode = (PolicySet)_mainTree.SelectedNode;
			PolicySetElementReadWrite policySet = policySetNode.PolicySetDefinition;

			var obligations = new ObligationReadWriteCollection();  //TODO: check version

			policySet.Obligations = obligations;

			var obligationsNode = new Obligations(obligations);

			policySetNode.Nodes.Add(obligationsNode);

			obligationsNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateCondition(object sender, EventArgs args)
		{
			Condition newConditionNode = null;
			TreeNode parentNode = null;

			var condition = new ConditionElementReadWrite("urn:new_function", new ExpressionReadWriteCollection(), XacmlVersion.Version11);

			newConditionNode = new Condition(condition);

			parentNode = _mainTree.SelectedNode;

			RuleElementReadWrite rule = ((Rule)parentNode).RuleDefinition;

			rule.Condition = condition;


			parentNode.Nodes.Add(newConditionNode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateTargetItem(object sender, EventArgs args)
		{
			if (_mainTree.SelectedNode is AnySubject)
			{
				var anyNode = (AnySubject)_mainTree.SelectedNode;
				var targetNode = (Target)anyNode.Parent;

				int idx = targetNode.Nodes.IndexOf(anyNode);
				targetNode.Nodes.RemoveAt(idx);

				var matchCollection = new TargetMatchReadWriteCollection();
				matchCollection.Add(
					new SubjectMatchElementReadWrite(
						Consts.Schema1.InternalFunctions.StringEqual,
						new AttributeValueElementReadWrite(Consts.Schema1.InternalDataTypes.XsdString, "Somebody", XacmlVersion.Version11),  //TODO: check version
						new SubjectAttributeDesignatorElement(Consts.Schema1.InternalDataTypes.XsdString, false, Consts.Schema1.SubjectElement.ActionSubjectId, "", "", XacmlVersion.Version11), XacmlVersion.Version11));  //TODO: check version
				var targetItem = new SubjectElementReadWrite(matchCollection, XacmlVersion.Version11);  //TODO: check version

				var targetItemNode = new TargetItem(targetItem);

				targetNode.Nodes.Insert(idx, targetItemNode);
				targetNode.TargetDefinition.Subjects.IsAny = false;
				targetNode.TargetDefinition.Subjects.ItemsList.Add(targetItem);
				targetItemNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);

				_mainTree.SelectedNode = targetItemNode;
			}
			else if (_mainTree.SelectedNode is AnyAction)
			{
				var anyActionNode = (AnyAction)_mainTree.SelectedNode;
				var targetNode = (Target)anyActionNode.Parent;

				int idx = targetNode.Nodes.IndexOf(anyActionNode);
				targetNode.Nodes.RemoveAt(idx);

				var matchCollection = new TargetMatchReadWriteCollection();
				matchCollection.Add(
					new ActionMatchElementReadWrite(
						Consts.Schema1.InternalFunctions.StringEqual,
						new pol.AttributeValueElementReadWrite(Consts.Schema1.InternalDataTypes.XsdString, "DoSomething", XacmlVersion.Version11),  //TODO: check version
						new ActionAttributeDesignatorElement(Consts.Schema1.InternalDataTypes.XsdString, false, Consts.Schema1.ActionElement.ActionId, "", XacmlVersion.Version11), XacmlVersion.Version11)); //TODO: check version
				var action = new ActionElementReadWrite(matchCollection, XacmlVersion.Version11); //TODO: check version

				var actionNode = new TargetItem(action);

				targetNode.Nodes.Insert(idx, actionNode);
				targetNode.TargetDefinition.Actions.IsAny = false;
				targetNode.TargetDefinition.Actions.ItemsList.Add(action);
				actionNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);

				_mainTree.SelectedNode = actionNode;
			}
			else if (_mainTree.SelectedNode is AnyResource)
			{
				var anyNode = (AnyResource)_mainTree.SelectedNode;
				var targetNode = (Target)anyNode.Parent;

				int idx = targetNode.Nodes.IndexOf(anyNode);
				targetNode.Nodes.RemoveAt(idx);

				var matchCollection = new TargetMatchReadWriteCollection
				{
					new ResourceMatchElementReadWrite(
						Consts.Schema1.InternalFunctions.StringEqual,
						new AttributeValueElementReadWrite(Consts.Schema1.InternalDataTypes.XsdString, "Something",
							XacmlVersion.Version11), //TODO: check version
						new ResourceAttributeDesignatorElement(Consts.Schema1.InternalDataTypes.XsdString, false,
							Consts.Schema1.ResourceElement.ResourceId, "", XacmlVersion.Version11),
						XacmlVersion.Version11)
				};
				var targetItem = new ResourceElementReadWrite(matchCollection, XacmlVersion.Version11); //TODO: check version

				var targetItemNode = new TargetItem(targetItem);

				targetNode.Nodes.Insert(idx, targetItemNode);
				targetNode.TargetDefinition.Resources.IsAny = false;
				targetNode.TargetDefinition.Resources.ItemsList.Add(targetItem);
				targetItemNode.NodeFont = new Font(_mainTree.Font, FontStyle.Bold);

				_mainTree.SelectedNode = targetItemNode;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			// Check if the control have been modified
			if (_mainPanel.Controls.Count != 0)
			{
				if (!(_mainPanel.Controls[0] is ContextCustomControls.XmlViewer))
				{
					var baseControl = _mainPanel.Controls[0] as CustomControls.BaseControl;

					_mainTree.SelectedNode.NodeFont = new Font(_mainTree.Font, FontStyle.Regular);
					NoBoldNode oNode = null;
					var policySet = baseControl as CustomControls.PolicySet;
					if (policySet != null)
					{
						oNode = new PolicySet(policySet.PolicySetElement);
					}
					else
					{
						var rule = baseControl as CustomControls.Rule;
						if (rule != null)
						{
							oNode = new Rule(rule.RuleElement);
						}
						else
						{
							var targetItem = baseControl as CustomControls.TargetItem;
							if (targetItem != null)
							{
								TargetItemBaseReadWrite element = targetItem.TargetItemBaseElement;
								oNode = new TargetItem(element);
							}
							else
							{
								var obligations = baseControl as CustomControls.Obligations;
								if (obligations != null)
								{
									oNode = new Obligations(obligations.ObligationsElement);
								}
								else
								{
									var attribute = baseControl as ContextCustomControls.Attribute;
									if (attribute != null)
									{
										oNode = new Attribute(attribute.AttributeElement);
									}
									else
									{
										var resource = baseControl as ContextCustomControls.Resource;
										if (resource != null)
										{
											oNode = new Resource(resource.ResourceElement);
										}
									}
								}
							}
						}
					}

					if (oNode != null)
					{
						_mainTree.SelectedNode = oNode;
						_mainTree.SelectedNode.Text = oNode.Text;
					}
				}
			}
		}

		private void mniDelete_Click(object sender, System.EventArgs e)
		{
			if (_mainTree.SelectedNode != null)
			{
				_mainPanel.Controls.Clear();
				if (_mainTree.SelectedNode.Parent != null)
				{
					var node = (NoBoldNode)_mainTree.SelectedNode.Parent;
					if (node is PolicySet)
					{
						DeleteFromPolicySet((NoBoldNode)_mainTree.SelectedNode);
					}
					else if (node is Policy)
					{
						DeleteFromPolicy((NoBoldNode)_mainTree.SelectedNode);
					}
					else if (node is Target)
					{
						DeleteFromTarget((TargetItem)_mainTree.SelectedNode);
					}
					else if (node is Request)
					{
						DeleteFromRequest((NoBoldNode)_mainTree.SelectedNode);
					}
					if (_mainTree.SelectedNode is Attribute)
					{
						DeleteContextAttribute((NoBoldNode)_mainTree.SelectedNode.Parent);
						_mainTree.SelectedNode.Remove();
					}
					else
					{
						node.Nodes.Remove(_mainTree.SelectedNode);
					}
				}
				else
				{
					_menuItemOpenRequest.Enabled = true;
					_menuItemOpenPolicy.Enabled = true;
					_menuItemSaveAs.Enabled = false;
					_menuItemSave.Enabled = false;
					_mainTree.Nodes.Clear();
				}
			}
		}

		private void DeleteFromRequest(NoBoldNode childNode)
		{
			var parentNode = (Request)_mainTree.SelectedNode.Parent;
			if (childNode is Action)
			{
				parentNode.RequestDefinition.Action = null;
			}
			else
			{
				var node = childNode as Resource;
				if (node != null)
				{
					con.ResourceElementReadWrite resource = node.ResourceDefinition;

					int index = parentNode.RequestDefinition.Resources.GetIndex(resource);
					parentNode.RequestDefinition.Resources.RemoveAt(index);
				}
				else if (childNode is Subject)
				{
					con.SubjectElementReadWrite subject = ((Subject)childNode).SubjectDefinition;

					int index = parentNode.RequestDefinition.Subjects.GetIndex(subject);
					parentNode.RequestDefinition.Subjects.RemoveAt(index);
				}
			}
		}

		private void DeleteContextAttribute(NoBoldNode parentNode)
		{
			var attributeNode = (Attribute)_mainTree.SelectedNode;
			con.AttributeElementReadWrite attribute = attributeNode.AttributeDefinition;

			var node = parentNode as Action;
			if (node != null)
			{
				con.ActionElementReadWrite action = node.ActionDefinition;

				int index = action.Attributes.GetIndex(attribute);
				action.Attributes.RemoveAt(index);
			}
			else
			{
				var resource1 = parentNode as Resource;
				if (resource1 != null)
				{
					con.ResourceElementReadWrite resource = resource1.ResourceDefinition;

					int index = resource.Attributes.GetIndex(attribute);
					resource.Attributes.RemoveAt(index);
				}
				else
				{
					var subject1 = parentNode as Subject;
					if (subject1 != null)
					{
						con.SubjectElementReadWrite subject = subject1.SubjectDefinition;

						int index = subject.Attributes.GetIndex(attribute);
						subject.Attributes.RemoveAt(index);
					}
				}
			}
		}

		private void DeleteFromTarget(TargetItem childNode)
		{
			var parentNode = (Target)_mainTree.SelectedNode.Parent;

			TargetItemBaseReadWrite element = childNode.TargetItemDefinition;

			if (element is ActionElementReadWrite)
			{
				var anyAction = new AnyAction();
				parentNode.Nodes.Add(anyAction);
				parentNode.TargetDefinition.Actions.ItemsList = null;
				parentNode.TargetDefinition.Actions.IsAny = true;
			}
			else if (element is ResourceElementReadWrite)
			{
				var anyResource = new AnyResource();
				parentNode.Nodes.Add(anyResource);
				parentNode.TargetDefinition.Resources.ItemsList = null;
				parentNode.TargetDefinition.Resources.IsAny = true;
			}
			else if (element is SubjectElementReadWrite)
			{
				var anySubject = new AnySubject();
				parentNode.Nodes.Add(anySubject);
				parentNode.TargetDefinition.Subjects.ItemsList = null;
				parentNode.TargetDefinition.Subjects.IsAny = true;
			}
		}
		private void DeleteFromPolicySet(NoBoldNode childNode)
		{
			var parentNode = (PolicySet)_mainTree.SelectedNode.Parent;
			var policy = childNode as Policy;
			if (policy != null)
			{
				var policyNode = policy;
				parentNode.PolicySetDefinition.Policies.Remove(policyNode.PolicyDefinition);
			}
			else
			{
				var policySet = childNode as PolicySet;
				if (policySet != null)
				{
					var policySetNode = policySet;
					parentNode.PolicySetDefinition.Policies.Remove(policySetNode.PolicySetDefinition);
				}
				else if (childNode is Obligations)
				{
					parentNode.PolicySetDefinition.Obligations = null;
				}
				else if (childNode is Target)
				{
					parentNode.PolicySetDefinition.Target = null;
				}
			}
		}

		private void DeleteFromPolicy(NoBoldNode childNode)
		{
			var parentNode = (Policy)_mainTree.SelectedNode.Parent;
			var node = childNode as Rule;
			if (node != null)
			{
				var rule = node.RuleDefinition;
				int index = parentNode.PolicyDefinition.Rules.GetIndex(rule);
				parentNode.PolicyDefinition.Rules.RemoveAt(index);
			}
			else if (childNode is Obligations)
			{
				parentNode.PolicyDefinition.Obligations = null;
			}
			else if (childNode is Target)
			{
				parentNode.PolicyDefinition.Target = null;
			}
		}

		private void menuItemSaveAs_Click(object sender, System.EventArgs e)
		{
			if (_saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				var writer = new XmlTextWriter(_saveFileDialog.FileName, System.Text.Encoding.UTF8)
				{
					Namespaces = true,
					Formatting = Formatting.Indented
				};

				if (_docType == DocumentType.Request)
				{
					con.ContextDocumentReadWrite oCon = ((Context)_mainTree.TopNode).ContextDefinition;
					oCon.WriteRequestDocument(writer);
				}
				else if (_docType == DocumentType.Policy)
				{
					PolicyDocumentReadWrite oPol = ((TreeNodes.PolicyDocument)_mainTree.TopNode).PolicyDocumentDefinition;
					oPol.WriteDocument(writer);
				}

				writer.Close();
			}
		}

		private void menuItemClose_Click(object sender, System.EventArgs e)
		{
			_mainTree.Nodes.Clear();
			_mainPanel.Controls.Clear();
			_path = string.Empty;
			_menuItemOpenPolicy.Enabled = true;
			_menuItemOpenRequest.Enabled = true;
			_menuItemSaveAs.Enabled = false;
			_menuItemSave.Enabled = false;
			_menuItemRunRequest.Enabled = false;
			_menuItemRunPolicy.Enabled = false;
		}

		private void menuItemOpenRequest_Click(object sender, System.EventArgs e)
		{
			_openFileDialog.Filter = @"Request Files|*.xml|All Files|*.*";
			if (_openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Stream stream = _openFileDialog.OpenFile();
				_path = _openFileDialog.FileName;
				_docType = DocumentType.Request;
				con.ContextDocumentReadWrite doc = ContextLoader.LoadContextDocument(stream, XacmlVersion.Version11, DocumentAccess.ReadWrite);
				_mainTree.Nodes.Add(new Context(doc));
				_menuItemSaveAs.Enabled = true;
				_menuItemSave.Enabled = true;
				_menuItemOpenRequest.Enabled = false;
				_menuItemOpenPolicy.Enabled = false;
				_menuItemRunPolicy.Enabled = true;
				_menuItemRunRequest.Enabled = false;
				stream.Close();
			}
		}

		private void menuItemRunPolicy_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show(this, @"The request will be saved. Do you want to proceed?", @"Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				//Loads the policy
				_openFileDialog.Filter = @"Policy Files|*.xml|All Files|*.*";
				if (_openFileDialog.ShowDialog() == DialogResult.OK)
				{
					menuItemSave_Click(sender, e);
					PolicyDocumentReadWrite oPol = PolicyLoader.LoadPolicyDocument(_openFileDialog.OpenFile(), XacmlVersion.Version11);
					//Gets the context from the TreeView
					Stream stream = new FileStream(_path, FileMode.Open);
					con.ContextDocumentReadWrite oCon = ContextLoader.LoadContextDocument(stream, XacmlVersion.Version11);

					stream.Close();

					//Evaluates the request
					var engine = new Runtime.EvaluationEngine();
					con.ResponseElement res = engine.Evaluate((pol.PolicyDocument)oPol, (con.ContextDocument)oCon);

					_mainPanel.Controls.Clear();
					//Creates the xml
					string path = Path.GetTempFileName();
					XmlWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
					res.WriteDocument(writer);
					writer.Close();

					_mainPanel.Controls.Add(new ContextCustomControls.XmlViewer(path, Consts.ContextSchema.ResponseElement.Response));
				}
			}
		}

		private void menuItemRunRequest_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, @"The policy will be saved. Do you want to proceed?", @"Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				//Loads the request
				_openFileDialog.Filter = @"Request Files|*.xml|All Files|*.*";
				if (_openFileDialog.ShowDialog() == DialogResult.OK)
				{
					menuItemSave_Click(sender, e);
					con.ContextDocumentReadWrite oCon = ContextLoader.LoadContextDocument(_openFileDialog.OpenFile(), XacmlVersion.Version11);
					//Gets the policy from the TreeView
					Stream stream = new FileStream(_path, FileMode.Open);
					PolicyDocumentReadWrite oPol = PolicyLoader.LoadPolicyDocument(stream, XacmlVersion.Version11);

					stream.Close();

					//Evaluates the request
					var engine = new Runtime.EvaluationEngine();
					con.ResponseElement res = engine.Evaluate((pol.PolicyDocument)oPol, (con.ContextDocument)oCon);

					//Creates the xml
					string path = Path.GetTempFileName();
					XmlWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
					res.WriteDocument(writer);
					writer.Close();

					_mainPanel.Controls.Clear();

					_mainPanel.Controls.Add(new ContextCustomControls.XmlViewer(path, Consts.ContextSchema.ResponseElement.Response));
				}
			}
		}

		private void menuItemSave_Click(object sender, EventArgs e)
		{
			var writer = new XmlTextWriter(_path, System.Text.Encoding.UTF8)
			{
				Namespaces = true,
				Formatting = Formatting.Indented
			};

			if (_docType == DocumentType.Request)
			{
				con.ContextDocumentReadWrite oCon = ((Context)_mainTree.TopNode).ContextDefinition;
				oCon.WriteRequestDocument(writer);
			}
			else if (_docType == DocumentType.Policy)
			{
				PolicyDocumentReadWrite oPol = ((TreeNodes.PolicyDocument)_mainTree.TopNode).PolicyDocumentDefinition;
				oPol.WriteDocument(writer);
			}

			writer.Close();
		}
	}
}
