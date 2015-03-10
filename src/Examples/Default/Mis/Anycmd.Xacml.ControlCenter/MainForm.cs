using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Anycmd.Xacml.Policy.TargetItems;

namespace Anycmd.Xacml.ControlCenter
{
	using Anycmd.Xacml;
	using Anycmd.Xacml.ControlCenter.ContextTreeNodes;
	using Anycmd.Xacml.ControlCenter.TreeNodes;
	using Anycmd.Xacml.Policy;
	using System.Xml;
	using con = Anycmd.Xacml.Context;
	using pol = Anycmd.Xacml.Policy;

	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// 
		/// </summary>
		public static readonly System.Drawing.Font DEFAULT_FONT = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.TreeView mainTree;
		private System.Windows.Forms.ImageList mainImageList;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem mniCreateNew;
		private System.Windows.Forms.MenuItem mniDelete;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private string _path = string.Empty;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private DocumentType docType;

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainTree = new System.Windows.Forms.TreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.mniCreateNew = new System.Windows.Forms.MenuItem();
			this.mniDelete = new System.Windows.Forms.MenuItem();
			this.mainImageList = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTree
			// 
			this.mainTree.AllowDrop = true;
			this.mainTree.ContextMenu = this.contextMenu;
			this.mainTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.mainTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mainTree.FullRowSelect = true;
			this.mainTree.HotTracking = true;
			this.mainTree.ImageIndex = 0;
			this.mainTree.ImageList = this.mainImageList;
			this.mainTree.Location = new System.Drawing.Point(0, 0);
			this.mainTree.Name = "mainTree";
			this.mainTree.SelectedImageIndex = 0;
			this.mainTree.Size = new System.Drawing.Size(342, 625);
			this.mainTree.TabIndex = 0;
			this.mainTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.mainTree_BeforeSelect);
			this.mainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTree_AfterSelect);
			this.mainTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainTree_MouseDown);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.mniCreateNew,
			this.mniDelete});
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// mniCreateNew
			// 
			this.mniCreateNew.Index = 0;
			this.mniCreateNew.Text = "Create new";
			// 
			// mniDelete
			// 
			this.mniDelete.Index = 1;
			this.mniDelete.Text = "Delete";
			this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
			// 
			// mainImageList
			// 
			this.mainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainImageList.ImageStream")));
			this.mainImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.mainImageList.Images.SetKeyName(0, "");
			this.mainImageList.Images.SetKeyName(1, "");
			this.mainImageList.Images.SetKeyName(2, "");
			this.mainImageList.Images.SetKeyName(3, "");
			this.mainImageList.Images.SetKeyName(4, "");
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem1,
			this.menuItem6,
			this.menuItem10});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem2,
			this.menuItem5,
			this.menuItem9,
			this.menuItem3,
			this.menuItem4});
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Open Policy...";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "Open Request...";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Enabled = false;
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "Save";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Enabled = false;
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "Save as...";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "Close";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem7,
			this.menuItem8});
			this.menuItem6.Text = "Context";
			// 
			// menuItem7
			// 
			this.menuItem7.Enabled = false;
			this.menuItem7.Index = 0;
			this.menuItem7.Text = "Run with policy...";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Enabled = false;
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "Run with request...";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 2;
			this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem11});
			this.menuItem10.Text = "Help";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 0;
			this.menuItem11.Text = "About";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.mainPanel);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.mainTree);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(964, 625);
			this.panel1.TabIndex = 3;
			// 
			// mainPanel
			// 
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(346, 0);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(618, 625);
			this.mainPanel.TabIndex = 4;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(342, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 625);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "xml";
			this.openFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
			this.openFileDialog.InitialDirectory = ".";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.CreatePrompt = true;
			this.saveFileDialog.DefaultExt = "xml";
			this.saveFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(964, 625);
			this.Controls.Add(this.panel1);
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Xacml Control Center";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			openFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				System.IO.Stream stream = openFileDialog.OpenFile();
				pol.PolicyDocumentReadWrite doc = PolicyLoader.LoadPolicyDocument( stream, XacmlVersion.Version11, DocumentAccess.ReadWrite );
				_path = openFileDialog.FileName;
				mainTree.Nodes.Add( new TreeNodes.PolicyDocument( doc ) );
				docType = DocumentType.Policy;
				menuItem3.Enabled = true;
				menuItem9.Enabled = true;
				menuItem2.Enabled = false;
				menuItem5.Enabled = false;
				menuItem8.Enabled = true;
				menuItem7.Enabled = false;
				stream.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			// Clear the main panel
			mainPanel.Controls.Clear();

			// If the control have been instantiated before use it, otherwise create the control.
			if( e.Node.Tag != null )
			{
				mainPanel.Controls.Add( (Control)e.Node.Tag );
			}
			else
			{
				// Create the control depending on the node type.
				if( e.Node is TreeNodes.PolicySet )
				{
					mainPanel.Controls.Add( new CustomControls.PolicySet( ((TreeNodes.PolicySet)e.Node).PolicySetDefinition ) );
				}
				else if( e.Node is TreeNodes.Policy )
				{
					mainPanel.Controls.Add( new CustomControls.Policy( ((TreeNodes.Policy)e.Node).PolicyDefinition ) );
				}
				else if( e.Node is TreeNodes.PolicyIdReference )
				{
				}
				else if( e.Node is TreeNodes.PolicySetIdReference )
				{
				}
				else if( e.Node is TreeNodes.Target )
				{
				}
				else if( e.Node is TreeNodes.Obligations )
				{
					mainPanel.Controls.Add( new CustomControls.Obligations( ((TreeNodes.Obligations)e.Node).ObligationDefinition ) );
				}
				else if( e.Node is TreeNodes.TargetItem )
				{
					mainPanel.Controls.Add( new CustomControls.TargetItem( ((TreeNodes.TargetItem)e.Node).TargetItemDefinition ) );
				}
				else if( e.Node is TreeNodes.Rule )
				{
					mainPanel.Controls.Add( new CustomControls.Rule( ((TreeNodes.Rule)e.Node).RuleDefinition ) );
				}
				else if( e.Node is TreeNodes.Condition )
				{
					mainPanel.Controls.Add( new CustomControls.Condition( ((TreeNodes.Condition)e.Node).ConditionDefinition ) );
				}
				else if( e.Node is ContextTreeNodes.Attribute )
				{
					mainPanel.Controls.Add( new ContextCustomControls.Attribute( ((ContextTreeNodes.Attribute)e.Node).AttributeDefinition ) );
				}
				else if( e.Node is ContextTreeNodes.Resource )
				{
					mainPanel.Controls.Add( new ContextCustomControls.Resource( ((ContextTreeNodes.Resource)e.Node).ResourceDefinition ) );
				}

				// If the control was created and added successfully, Dock it and keep the 
				// instance in the tree node.
				if( mainPanel.Controls.Count != 0 )
				{
					mainPanel.Controls[0].Dock = DockStyle.Fill;
					e.Node.Tag = mainPanel.Controls[0];
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenu_Popup(object sender, System.EventArgs e)
		{
			mniCreateNew.MenuItems.Clear();
			if( mainTree.SelectedNode == null)
			{
				if(mainTree.Nodes.Count == 0)
				{
					mniCreateNew.MenuItems.Add( "PolicyDocument", new EventHandler( CreatePolicyDocument ) );
					mniCreateNew.MenuItems.Add( "ContextDocument", new EventHandler( CreateContextDocument ) );
				}
			}
			else if( mainTree.SelectedNode is TreeNodes.PolicyDocument)
			{
				if( ((TreeNodes.PolicyDocument)mainTree.SelectedNode).PolicyDocumentDefinition.Policy == null &&
					((TreeNodes.PolicyDocument)mainTree.SelectedNode).PolicyDocumentDefinition.PolicySet == null)
				{
					mniCreateNew.MenuItems.Add( "Policy", new EventHandler( CreatePolicyFromDocument ) );
					mniCreateNew.MenuItems.Add( "PolicySet", new EventHandler( CreatePolicySetFromDocument ) );
				}
			}
			else if( mainTree.SelectedNode is TreeNodes.PolicySet )
			{
				mniCreateNew.MenuItems.Add( "Policy", new EventHandler( CreatePolicy ) );
				mniCreateNew.MenuItems.Add( "PolicySet", new EventHandler( CreatePolicySet ) );
				if( ((TreeNodes.PolicySet)mainTree.SelectedNode).PolicySetDefinition.Target == null )
				{
					mniCreateNew.MenuItems.Add( "Target", new EventHandler( CreateTarget ) );
				}
				if( ((TreeNodes.PolicySet)mainTree.SelectedNode).PolicySetDefinition.Obligations == null )
				{
					mniCreateNew.MenuItems.Add( "Obligations", new EventHandler( CreateObligationsFromPolicySet ) );
				}
			}
			else if( mainTree.SelectedNode is TreeNodes.Policy )
			{
				mniCreateNew.MenuItems.Add( "Rule", new EventHandler( CreateRule ) );
				if( ((TreeNodes.Policy)mainTree.SelectedNode).PolicyDefinition.Target == null )
				{
					mniCreateNew.MenuItems.Add( "Target", new EventHandler( CreateTarget ) );
				}
				if( ((TreeNodes.Policy)mainTree.SelectedNode).PolicyDefinition.Obligations == null )
				{
					mniCreateNew.MenuItems.Add( "Obligations", new EventHandler( CreateObligationsFromPolicy ) );
				}
			}
			else if( mainTree.SelectedNode is TreeNodes.Rule )
			{
				if( ((TreeNodes.Rule)mainTree.SelectedNode).RuleDefinition.Condition == null )
				{
					mniCreateNew.MenuItems.Add( "Condition", new EventHandler( CreateCondition ) );
				}
				if( ((TreeNodes.Rule)mainTree.SelectedNode).RuleDefinition.Target == null )
				{
					mniCreateNew.MenuItems.Add( "Target", new EventHandler( CreateTarget ) );
				}
			}
			else if( mainTree.SelectedNode is TreeNodes.PolicyIdReference )
			{
			}
			else if( mainTree.SelectedNode is TreeNodes.PolicySetIdReference )
			{
			}
			else if( mainTree.SelectedNode is TreeNodes.Obligations )
			{
			}
			else if( mainTree.SelectedNode is TreeNodes.Target )
			{
				
			}
			else if( mainTree.SelectedNode is TreeNodes.TargetItem )
			{
			}
			else if( mainTree.SelectedNode is TreeNodes.AnyTarget )
			{
				mniCreateNew.MenuItems.Add( "Target", new EventHandler( CreateTarget ) );
			}
			else if( mainTree.SelectedNode is TreeNodes.AnySubject )
			{
				mniCreateNew.MenuItems.Add( "Subject", new EventHandler( CreateTargetItem ) );
			}
			else if( mainTree.SelectedNode is TreeNodes.AnyAction )
			{
				mniCreateNew.MenuItems.Add( "Action", new EventHandler( CreateTargetItem ) );
			}
			else if( mainTree.SelectedNode is TreeNodes.AnyResource )
			{
				mniCreateNew.MenuItems.Add( "Resource", new EventHandler( CreateTargetItem ) );
			}
			else if( mainTree.SelectedNode is TreeNodes.Condition )
			{
			}
			else if( mainTree.SelectedNode is ContextTreeNodes.Request )
			{
				if( ((ContextTreeNodes.Request)mainTree.SelectedNode).RequestDefinition.Action == null )
				{
					mniCreateNew.MenuItems.Add( "Action", new EventHandler( CreateContextActionElement ) );
				}
				mniCreateNew.MenuItems.Add( "Resource", new EventHandler( CreateContextResourceElement ) );
				mniCreateNew.MenuItems.Add( "Subject", new EventHandler( CreateContextSubjectElement ) );
			}
			else if( mainTree.SelectedNode is ContextTreeNodes.Action || mainTree.SelectedNode is ContextTreeNodes.Resource ||
				mainTree.SelectedNode is ContextTreeNodes.Subject)
			{
				mniCreateNew.MenuItems.Add( "Attribute", new EventHandler( CreateContextAttributeElement ) );
			}
			else if( mainTree.SelectedNode is ContextTreeNodes.Context )
			{
				if( ((ContextTreeNodes.Context)mainTree.SelectedNode).ContextDefinition.Request == null )
				{
					mniCreateNew.MenuItems.Add( "Request", new EventHandler( CreateContextRequest ) );
				}
			}

			if( mniCreateNew.MenuItems.Count == 0 )
			{
				mniCreateNew.Visible = false;
			}
			else
			{
				mniCreateNew.Visible = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Right )
			{
				mainTree.SelectedNode = mainTree.GetNodeAt( e.X, e.Y );
			}
		}

		private void CreateContextSubjectElement( object sender, EventArgs args)
		{
			con.SubjectElementReadWrite newSubject = new con.SubjectElementReadWrite(string.Empty, new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			ContextTreeNodes.Request requestNode = (ContextTreeNodes.Request)mainTree.SelectedNode;
			ContextTreeNodes.Subject newNode = new Subject( newSubject );
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			if( request.Subjects == null )
			{
				request.Subjects = new con.SubjectReadWriteCollection();
			}
			request.Subjects.Add( newSubject );
			requestNode.Nodes.Add( newNode );
		}

		private void CreateContextAttributeElement( object sender, EventArgs args )
		{
			TreeNodes.NoBoldNode node = (TreeNodes.NoBoldNode)mainTree.SelectedNode;

			if( node is ContextTreeNodes.Action )
			{
				ContextTreeNodes.Action actionNode = (ContextTreeNodes.Action)mainTree.SelectedNode;
				con.ActionElementReadWrite action = actionNode.ActionDefinition;
				con.AttributeElementReadWrite attribute = new con.AttributeElementReadWrite( string.Empty, string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				ContextTreeNodes.Attribute attributeNode = new ContextTreeNodes.Attribute( attribute );

				action.Attributes.Add( attribute );
				actionNode.Nodes.Add( attributeNode );
			}
			else if( node is ContextTreeNodes.Resource )
			{
				ContextTreeNodes.Resource resourceNode = (ContextTreeNodes.Resource)mainTree.SelectedNode;
				con.ResourceElementReadWrite resource = resourceNode.ResourceDefinition;
				con.AttributeElementReadWrite attribute = new con.AttributeElementReadWrite( string.Empty, string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				ContextTreeNodes.Attribute attributeNode = new ContextTreeNodes.Attribute( attribute );

				resource.Attributes.Add( attribute );
				resourceNode.Nodes.Add( attributeNode );
			}
			else if( node is ContextTreeNodes.Subject )
			{
				ContextTreeNodes.Subject subjectNode = (ContextTreeNodes.Subject)mainTree.SelectedNode;
				con.SubjectElementReadWrite subject = subjectNode.SubjectDefinition;
				con.AttributeElementReadWrite attribute = new con.AttributeElementReadWrite( "urn:new_attribute", string.Empty, string.Empty,
					string.Empty, "TODO: Add value", XacmlVersion.Version11);
				ContextTreeNodes.Attribute attributeNode = new ContextTreeNodes.Attribute( attribute );

				subject.Attributes.Add( attribute );
				subjectNode.Nodes.Add( attributeNode );
			}
		}

		private void CreateContextActionElement( object sender, EventArgs args)
		{
			con.ActionElementReadWrite newAction = new con.ActionElementReadWrite(new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			ContextTreeNodes.Request requestNode = (ContextTreeNodes.Request)mainTree.SelectedNode;
			ContextTreeNodes.Action newNode = new Action( newAction );
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			request.Action = newAction;
			requestNode.Nodes.Add( newNode );
		}
		private void CreateContextResourceElement( object sender, EventArgs args)
		{
			con.ResourceElementReadWrite newResource = new con.ResourceElementReadWrite(null, con.ResourceScope.Immediate, new con.AttributeReadWriteCollection(), XacmlVersion.Version11);
			ContextTreeNodes.Request requestNode = (ContextTreeNodes.Request)mainTree.SelectedNode;
			ContextTreeNodes.Resource newNode = new Resource( newResource );
			con.RequestElementReadWrite request = requestNode.RequestDefinition;

			if( request.Resources == null )
			{
				request.Resources = new con.ResourceReadWriteCollection();
			}
			request.Resources.Add( newResource );
			requestNode.Nodes.Add( newNode );
		}

		/// <summary>
		/// Creates a new context document
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">THe arguements for the event.</param>
		private void CreateContextDocument( object sender, EventArgs args )
		{
			// Create a new policydocument
			con.ContextDocumentReadWrite newContext = new con.ContextDocumentReadWrite( ); //TODO: check version

			newContext.Namespaces.Add(string.Empty, Consts.Schema1.Namespaces.Context);
			newContext.Namespaces.Add("xsi", Consts.Schema1.Namespaces.Xsi);
			ContextTreeNodes.Context newNode = new ContextTreeNodes.Context(newContext);
			mainTree.Nodes.Add(newNode);
			docType = DocumentType.Request;
			newNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
			menuItem2.Enabled = false;
			menuItem5.Enabled = false;
			menuItem3.Enabled = true;
			menuItem9.Enabled = true;
			menuItem7.Enabled = true;
			menuItem8.Enabled = false;
		}

		private void CreateContextRequest( object sender, EventArgs args )
		{
			con.RequestElementReadWrite newRequest = new con.RequestElementReadWrite(null,null,null,null,XacmlVersion.Version11);
			ContextTreeNodes.Context contextNode = (ContextTreeNodes.Context)mainTree.SelectedNode;
			ContextTreeNodes.Request requestNode = new ContextTreeNodes.Request( newRequest );
			con.ContextDocumentReadWrite context = contextNode.ContextDefinition;

			context.Request = newRequest;
			contextNode.Nodes.Add( requestNode );
		}

		/// <summary>
		/// Creates a new policy document
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicyDocument( object sender, EventArgs args )
		{
			// Create a new policydocument
			pol.PolicyDocumentReadWrite newPolicyDoc = new pol.PolicyDocumentReadWrite(Xacml.XacmlVersion.Version11 ); //TODO: check version

			newPolicyDoc.Namespaces.Add(string.Empty, Consts.Schema1.Namespaces.Policy);
			newPolicyDoc.Namespaces.Add("xsi", Consts.Schema1.Namespaces.Xsi);
			TreeNodes.PolicyDocument newNode = new TreeNodes.PolicyDocument(newPolicyDoc);
			mainTree.Nodes.Add(newNode);
			docType = DocumentType.Policy;
			
			newNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
			menuItem2.Enabled = false;
			menuItem5.Enabled = false;
			menuItem3.Enabled = true;
			menuItem9.Enabled = true;
			menuItem8.Enabled = true;
			menuItem7.Enabled = false;
		}

		/// <summary>
		/// Creates a new policy for the policy set selected.
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicy( object sender, EventArgs args )
		{
			TreeNodes.PolicySet policySetNode = (TreeNodes.PolicySet)mainTree.SelectedNode;
			pol.PolicySetElementReadWrite policySet = policySetNode.PolicySetDefinition;

			// Create a new policy
			pol.PolicyElementReadWrite newPolicy = new pol.PolicyElementReadWrite( 
				"urn:newpolicy", "[TODO: add a description]", 
				null,
				new pol.RuleReadWriteCollection(), 
				Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable, 
				new pol.ObligationReadWriteCollection(), 
				string.Empty,
				null,
				null,
				null,
				Xacml.XacmlVersion.Version11 ); //TODO: check version

			// Add the policy to the policySet.
			policySet.Policies.Add( newPolicy );

			// Create a new node
			TreeNodes.Policy policyNode = new TreeNodes.Policy( newPolicy );

			// Add the tree node.
			policySetNode.Nodes.Add( policyNode );

			// Set the font so the user knows the item was changed
			policyNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		/// <summary>
		/// Creates a new policy for the policy document selected.
		/// </summary>
		/// <param name="sender">The mainTree control.</param>
		/// <param name="args">The arguements for the event.</param>
		private void CreatePolicyFromDocument( object sender, EventArgs args )
		{
			TreeNodes.PolicyDocument policyDocumentNode = (TreeNodes.PolicyDocument)mainTree.SelectedNode;
			pol.PolicyDocumentReadWrite policyDocument = policyDocumentNode.PolicyDocumentDefinition;

			// Create a new policy
			pol.PolicyElementReadWrite newPolicy = new pol.PolicyElementReadWrite( 
				"urn:newpolicy", "[TODO: add a description]", null,
				new pol.RuleReadWriteCollection(), 
				Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable, 
				new pol.ObligationReadWriteCollection(), 
				string.Empty,
				null,
				null,
				null,
				Xacml.XacmlVersion.Version11 ); //TODO: check version

			
			policyDocument.Policy = newPolicy ;

			// Create a new node
			TreeNodes.Policy policyNode = new TreeNodes.Policy( newPolicy );

			// Add the tree node.
			policyDocumentNode.Nodes.Add( policyNode );

			// Set the font so the user knows the item was changed
			policyNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreatePolicySet( object sender, EventArgs args )
		{
			TreeNodes.PolicySet policySetNode = (TreeNodes.PolicySet)mainTree.SelectedNode;
			pol.PolicySetElementReadWrite policySet = policySetNode.PolicySetDefinition;

			// Create a new policy
			pol.PolicySetElementReadWrite newPolicySet = new pol.PolicySetElementReadWrite( 
				"urn:newpolicy", "[TODO: add a description]", 
				null, 
				new ArrayList(), 
				Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable, 
				new pol.ObligationReadWriteCollection(), 
				null,
				Xacml.XacmlVersion.Version11 ); //TODO: check version

			// Add the policy to the policySet.
			policySet.Policies.Add( newPolicySet );

			// Create a new node.
			TreeNodes.PolicySet newPolicySetNode = new TreeNodes.PolicySet( newPolicySet );

			// Add the tree node.
			policySetNode.Nodes.Add( newPolicySetNode );

			// Set the font so the user knows the item was changed
			newPolicySetNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreatePolicySetFromDocument( object sender, EventArgs args )
		{
			TreeNodes.PolicyDocument policyDocumentNode = (TreeNodes.PolicyDocument)mainTree.SelectedNode;
			pol.PolicyDocumentReadWrite policyDoc = policyDocumentNode.PolicyDocumentDefinition;

			// Create a new policy
			pol.PolicySetElementReadWrite newPolicySet = new pol.PolicySetElementReadWrite( 
				"urn:newpolicy", "[TODO: add a description]", 
				null, 
				new ArrayList(), 
				Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable, 
				new pol.ObligationReadWriteCollection(), 
				null,
				Xacml.XacmlVersion.Version11 ); //TODO: check version

			policyDoc.PolicySet = newPolicySet;

			// Create a new node.
			TreeNodes.PolicySet newPolicySetNode = new TreeNodes.PolicySet( newPolicySet );

			// Add the tree node.
			policyDocumentNode.Nodes.Add( newPolicySetNode );

			// Set the font so the user knows the item was changed
			newPolicySetNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateTarget( object sender, EventArgs args )
		{
			TreeNodes.Target newTargetNode = null;
			TreeNode parentNode = null;

			// Create the target
			pol.TargetElementReadWrite target = new pol.TargetElementReadWrite( 
				new ResourcesElementReadWrite( true, new TargetItemReadWriteCollection(), Xacml.XacmlVersion.Version11 ), //TODO: check version
				new SubjectsElementReadWrite( true, new TargetItemReadWriteCollection(), Xacml.XacmlVersion.Version11 ), //TODO: check version 
				new ActionsElementReadWrite( true, new TargetItemReadWriteCollection(), Xacml.XacmlVersion.Version11 ), //TODO: check version
				new EnvironmentsElementReadWrite( true, new TargetItemReadWriteCollection(), Xacml.XacmlVersion.Version11 ), //TODO: check version
				Xacml.XacmlVersion.Version11 ); //TODO: check version

			// Create the node
			newTargetNode = new TreeNodes.Target( target );

			if( mainTree.SelectedNode is TreeNodes.PolicySet )
			{
				parentNode = mainTree.SelectedNode;
				pol.PolicySetElementReadWrite policySet = ((TreeNodes.PolicySet)parentNode).PolicySetDefinition;

				// Set the target
				policySet.Target = target;
			}
			else if ( mainTree.SelectedNode is TreeNodes.Policy )
			{
				parentNode = mainTree.SelectedNode;
				pol.PolicyElementReadWrite policy = ((TreeNodes.Policy)parentNode).PolicyDefinition;

				// Set the target
				policy.Target = target;
			}
			else if ( mainTree.SelectedNode is TreeNodes.Rule )
			{
				parentNode = mainTree.SelectedNode;
				pol.RuleElementReadWrite rule = ((TreeNodes.Rule)parentNode).RuleDefinition;

				// Set the target
				rule.Target = target;
			}
			else if ( mainTree.SelectedNode is TreeNodes.AnyTarget )
			{
				parentNode = mainTree.SelectedNode.Parent;

				// Set the target
				if( parentNode is TreeNodes.PolicySet )
				{
					((TreeNodes.PolicySet)parentNode).PolicySetDefinition.Target = target;
				}
				else if ( parentNode is TreeNodes.Policy )
				{
					((TreeNodes.Policy)parentNode).PolicyDefinition.Target = target;
				}
				else if ( parentNode is TreeNodes.Rule )
				{
					((TreeNodes.Rule)parentNode).RuleDefinition.Target = target;
				}
			}

			if( newTargetNode != null && parentNode != null )
			{
				int idx = -1;
				
				// Search the previous node
				foreach( TreeNode node in parentNode.Nodes )
				{
					if( node is TreeNodes.AnyTarget )
					{
						idx = parentNode.Nodes.IndexOf( node );
						break;
					}
				}

				if( idx != -1 )
				{
					// Set the font to the node
					newTargetNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );

					// Remove the previous target node
					parentNode.Nodes.RemoveAt( idx );

					// Add the node to the node.
					parentNode.Nodes.Insert( idx, newTargetNode );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateRule( object sender, EventArgs args )
		{
			TreeNodes.Policy policyNode = (TreeNodes.Policy)mainTree.SelectedNode;
			pol.PolicyElementReadWrite policy = policyNode.PolicyDefinition;

			pol.RuleElementReadWrite rule = new pol.RuleElementReadWrite(
				"urn:new_rule",
				"[TODO: add rule description]",
				null,
				null, 
				pol.Effect.Permit,
				Xacml.XacmlVersion.Version11 );  //TODO: check version

			policy.Rules.Add( rule );

			TreeNodes.Rule ruleNode = new TreeNodes.Rule( rule );

			policyNode.Nodes.Add( ruleNode );

			ruleNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		private void CreateObligationsFromPolicy(object sender, EventArgs args )
		{
			TreeNodes.Policy policyNode = (TreeNodes.Policy)mainTree.SelectedNode;
			pol.PolicyElementReadWrite policy = policyNode.PolicyDefinition;

			pol.ObligationReadWriteCollection obligations = new pol.ObligationReadWriteCollection();  //TODO: check version

			policy.Obligations = obligations;

			TreeNodes.Obligations obligationsNode = new TreeNodes.Obligations( obligations );

			policyNode.Nodes.Add( obligationsNode );

			obligationsNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		private void CreateObligationsFromPolicySet(object sender, EventArgs args )
		{
			TreeNodes.PolicySet policySetNode = (TreeNodes.PolicySet)mainTree.SelectedNode;
			pol.PolicySetElementReadWrite policySet = policySetNode.PolicySetDefinition;

			pol.ObligationReadWriteCollection obligations = new pol.ObligationReadWriteCollection();  //TODO: check version

			policySet.Obligations = obligations;

			TreeNodes.Obligations obligationsNode = new TreeNodes.Obligations( obligations );

			policySetNode.Nodes.Add( obligationsNode );

			obligationsNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateCondition( object sender, EventArgs args )
		{
			TreeNodes.Condition newConditionNode = null;
			TreeNode parentNode = null;

			pol.ConditionElementReadWrite condition = new pol.ConditionElementReadWrite( "urn:new_function", new ExpressionReadWriteCollection(), XacmlVersion.Version11 );

			newConditionNode = new TreeNodes.Condition( condition );

			parentNode = mainTree.SelectedNode;

			pol.RuleElementReadWrite rule = ((TreeNodes.Rule)parentNode).RuleDefinition;

			rule.Condition = condition;


			parentNode.Nodes.Add( newConditionNode );
			/*if( newConditionNode != null && parentNode != null )
			{
				int idx = -1;
				
				// Search the previous node
				foreach( TreeNode node in parentNode.Nodes )
				{
					if( node is TreeNodes.Condition )
					{
						idx = parentNode.Nodes.IndexOf( node );
						break;
					}
				}

				if( idx != -1 )
				{
					// Set the font to the node
					newConditionNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );

					// Remove the previous condition node
					parentNode.Nodes.RemoveAt( idx );

					// Add the node to the node.
					parentNode.Nodes.Insert( idx, newConditionNode);
				}
			}*/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CreateTargetItem( object sender, EventArgs args )
		{
			if( mainTree.SelectedNode is TreeNodes.AnySubject )
			{
				TreeNodes.AnySubject anyNode = (TreeNodes.AnySubject)mainTree.SelectedNode;
				TreeNodes.Target targetNode = (TreeNodes.Target)anyNode.Parent;

				int idx = targetNode.Nodes.IndexOf( anyNode );
				targetNode.Nodes.RemoveAt( idx );

				TargetMatchReadWriteCollection matchCollection = new TargetMatchReadWriteCollection();
				matchCollection.Add( 
					new SubjectMatchElementReadWrite( 
						Consts.Schema1.InternalFunctions.StringEqual, 
						new pol.AttributeValueElementReadWrite( Consts.Schema1.InternalDataTypes.XsdString, "Somebody", Xacml.XacmlVersion.Version11 ),  //TODO: check version
						new SubjectAttributeDesignatorElement( Consts.Schema1.InternalDataTypes.XsdString, false, Consts.Schema1.SubjectElement.ActionSubjectId, "", "", Xacml.XacmlVersion.Version11 ), Xacml.XacmlVersion.Version11 ) );  //TODO: check version
				SubjectElementReadWrite targetItem = new SubjectElementReadWrite( matchCollection, Xacml.XacmlVersion.Version11 );  //TODO: check version

				TreeNodes.TargetItem targetItemNode = new TreeNodes.TargetItem( targetItem );

				targetNode.Nodes.Insert( idx, targetItemNode ); 
				targetNode.TargetDefinition.Subjects.IsAny = false;
				targetNode.TargetDefinition.Subjects.ItemsList.Add( targetItem );
				targetItemNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );

				mainTree.SelectedNode = targetItemNode;
			}
			else if( mainTree.SelectedNode is TreeNodes.AnyAction )
			{
				TreeNodes.AnyAction anyActionNode = (TreeNodes.AnyAction)mainTree.SelectedNode;
				TreeNodes.Target targetNode = (TreeNodes.Target)anyActionNode.Parent;

				int idx = targetNode.Nodes.IndexOf( anyActionNode );
				targetNode.Nodes.RemoveAt( idx );

				TargetMatchReadWriteCollection matchCollection = new TargetMatchReadWriteCollection();
				matchCollection.Add( 
					new ActionMatchElementReadWrite( 
						Consts.Schema1.InternalFunctions.StringEqual, 
						new pol.AttributeValueElementReadWrite( Consts.Schema1.InternalDataTypes.XsdString, "DoSomething", Xacml.XacmlVersion.Version11 ),  //TODO: check version
						new ActionAttributeDesignatorElement( Consts.Schema1.InternalDataTypes.XsdString, false, Consts.Schema1.ActionElement.ActionId, "", Xacml.XacmlVersion.Version11 ), Xacml.XacmlVersion.Version11 ) ); //TODO: check version
				ActionElementReadWrite action = new ActionElementReadWrite( matchCollection, Xacml.XacmlVersion.Version11 ); //TODO: check version

				TreeNodes.TargetItem actionNode = new TreeNodes.TargetItem( action );

				targetNode.Nodes.Insert( idx, actionNode ); 
				targetNode.TargetDefinition.Actions.IsAny = false;
				targetNode.TargetDefinition.Actions.ItemsList.Add( action );
				actionNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );

				mainTree.SelectedNode = actionNode;
			}
			else if( mainTree.SelectedNode is TreeNodes.AnyResource )
			{
				TreeNodes.AnyResource anyNode = (TreeNodes.AnyResource)mainTree.SelectedNode;
				TreeNodes.Target targetNode = (TreeNodes.Target)anyNode.Parent;

				int idx = targetNode.Nodes.IndexOf( anyNode );
				targetNode.Nodes.RemoveAt( idx );

				TargetMatchReadWriteCollection matchCollection = new TargetMatchReadWriteCollection();
				matchCollection.Add( 
					new ResourceMatchElementReadWrite( 
						Consts.Schema1.InternalFunctions.StringEqual, 
						new pol.AttributeValueElementReadWrite( Consts.Schema1.InternalDataTypes.XsdString, "Something", Xacml.XacmlVersion.Version11 ),  //TODO: check version
						new ResourceAttributeDesignatorElement( Consts.Schema1.InternalDataTypes.XsdString, false, Consts.Schema1.ResourceElement.ResourceId, "", Xacml.XacmlVersion.Version11 ), Xacml.XacmlVersion.Version11 ) ); //TODO: check version
				ResourceElementReadWrite targetItem = new ResourceElementReadWrite( matchCollection, Xacml.XacmlVersion.Version11 ); //TODO: check version

				TreeNodes.TargetItem targetItemNode = new TreeNodes.TargetItem( targetItem );

				targetNode.Nodes.Insert( idx, targetItemNode ); 
				targetNode.TargetDefinition.Resources.IsAny = false;
				targetNode.TargetDefinition.Resources.ItemsList.Add( targetItem );
				targetItemNode.NodeFont = new Font( mainTree.Font, FontStyle.Bold );

				mainTree.SelectedNode = targetItemNode;
			}			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTree_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// Check if the control have been modified
			if( mainPanel.Controls.Count != 0 )
			{
				if( !(mainPanel.Controls[0] is ContextCustomControls.XmlViewer) )
				{
					CustomControls.BaseControl baseControl = mainPanel.Controls[0] as CustomControls.BaseControl;

					mainTree.SelectedNode.NodeFont = new Font( mainTree.Font, FontStyle.Regular );
					TreeNodes.NoBoldNode oNode = null;
					if( baseControl is CustomControls.PolicySet )
					{
						oNode = new TreeNodes.PolicySet(((CustomControls.PolicySet)baseControl).PolicySetElement);
					}
					else if( baseControl is CustomControls.PolicySet )
					{
						oNode = new TreeNodes.Policy(((CustomControls.Policy)baseControl).PolicyElement);
					}
					else if( baseControl is CustomControls.Rule )
					{
						oNode = new TreeNodes.Rule(((CustomControls.Rule)baseControl).RuleElement);
					}
					else if( baseControl is CustomControls.TargetItem )
					{
						TargetItemBaseReadWrite element = ((CustomControls.TargetItem)baseControl).TargetItemBaseElement;
						oNode = new TreeNodes.TargetItem(element);
					}
					else if( baseControl is CustomControls.Obligations )
					{
						oNode = new TreeNodes.Obligations(((CustomControls.Obligations)baseControl).ObligationsElement);
					}
					else if( baseControl is ContextCustomControls.Attribute )
					{
						oNode = new ContextTreeNodes.Attribute( ((ContextCustomControls.Attribute)baseControl).AttributeElement );
					}
					else if( baseControl is ContextCustomControls.Resource )
					{
						oNode = new ContextTreeNodes.Resource( ((ContextCustomControls.Resource)baseControl).ResourceElement );
					}
				
					if( oNode != null )
					{
						mainTree.SelectedNode = oNode;
						mainTree.SelectedNode.Text = oNode.Text;
					}
				}
			}
		}

		private void mniDelete_Click(object sender, System.EventArgs e)
		{
			if(mainTree.SelectedNode != null)
			{
				mainPanel.Controls.Clear();
				if(mainTree.SelectedNode.Parent != null)
				{
					TreeNodes.NoBoldNode node = (TreeNodes.NoBoldNode)mainTree.SelectedNode.Parent;
					if( node is TreeNodes.PolicySet )
					{
						DeleteFromPolicySet( (TreeNodes.NoBoldNode)mainTree.SelectedNode );
					}
					else if( node is TreeNodes.Policy )
					{
						DeleteFromPolicy( (TreeNodes.NoBoldNode)mainTree.SelectedNode );
					}
					else if( node is TreeNodes.Target )
					{
						DeleteFromTarget( (TreeNodes.TargetItem)mainTree.SelectedNode );
					}
					else if( node is ContextTreeNodes.Request )
					{
						DeleteFromRequest( (TreeNodes.NoBoldNode)mainTree.SelectedNode );
					}
					if( mainTree.SelectedNode is ContextTreeNodes.Attribute )
					{
						DeleteContextAttribute( (TreeNodes.NoBoldNode)mainTree.SelectedNode.Parent );
						mainTree.SelectedNode.Remove();
					}
					else
					{
						node.Nodes.Remove( mainTree.SelectedNode );
					}
				}
				else
				{
					menuItem5.Enabled = true;
					menuItem2.Enabled = true;
					menuItem3.Enabled = false;
					menuItem9.Enabled = false;
					mainTree.Nodes.Clear();
				}
			}
		}

		private void DeleteFromRequest( TreeNodes.NoBoldNode childNode )
		{
			ContextTreeNodes.Request parentNode = (ContextTreeNodes.Request)mainTree.SelectedNode.Parent;
			if( childNode is ContextTreeNodes.Action )
			{
				parentNode.RequestDefinition.Action = null;
			}
			else if( childNode is ContextTreeNodes.Resource )
			{
				con.ResourceElementReadWrite resource = ((ContextTreeNodes.Resource)childNode).ResourceDefinition;
								
				int index = parentNode.RequestDefinition.Resources.GetIndex( resource );
				parentNode.RequestDefinition.Resources.RemoveAt( index );
			}
			else if( childNode is ContextTreeNodes.Subject )
			{
				con.SubjectElementReadWrite subject = ((ContextTreeNodes.Subject)childNode).SubjectDefinition;
								
				int index = parentNode.RequestDefinition.Subjects.GetIndex( subject );
				parentNode.RequestDefinition.Subjects.RemoveAt( index );
			}
		}

		private void DeleteContextAttribute( TreeNodes.NoBoldNode parentNode )
		{
			ContextTreeNodes.Attribute attributeNode = (ContextTreeNodes.Attribute)mainTree.SelectedNode;
			con.AttributeElementReadWrite attribute = attributeNode.AttributeDefinition;

			if( parentNode is ContextTreeNodes.Action )
			{
				con.ActionElementReadWrite action = ((ContextTreeNodes.Action)parentNode).ActionDefinition;

				int index = action.Attributes.GetIndex( attribute );
				action.Attributes.RemoveAt( index );
			}
			else if( parentNode is ContextTreeNodes.Resource )
			{
				con.ResourceElementReadWrite resource = ((ContextTreeNodes.Resource)parentNode).ResourceDefinition;

				int index = resource.Attributes.GetIndex( attribute );
				resource.Attributes.RemoveAt( index );
			}
			else if( parentNode is ContextTreeNodes.Subject )
			{
				con.SubjectElementReadWrite subject = ((ContextTreeNodes.Subject)parentNode).SubjectDefinition;

				int index = subject.Attributes.GetIndex( attribute );
				subject.Attributes.RemoveAt( index );
			}
		}

		private void DeleteFromTarget( TreeNodes.TargetItem childNode )
		{
			TreeNodes.Target parentNode = (TreeNodes.Target)mainTree.SelectedNode.Parent;

			TargetItemBaseReadWrite element = childNode.TargetItemDefinition;

			if( element is ActionElementReadWrite )
			{
				TreeNodes.AnyAction anyAction = new AnyAction();
				parentNode.Nodes.Add( anyAction );
				parentNode.TargetDefinition.Actions.ItemsList = null;
				parentNode.TargetDefinition.Actions.IsAny = true;
			}
			else if( element is ResourceElementReadWrite )
			{
				TreeNodes.AnyResource anyResource = new AnyResource();
				parentNode.Nodes.Add( anyResource );
				parentNode.TargetDefinition.Resources.ItemsList = null;
				parentNode.TargetDefinition.Resources.IsAny = true;
			}
			else if( element is SubjectElementReadWrite )
			{
				TreeNodes.AnySubject anySubject = new AnySubject();
				parentNode.Nodes.Add( anySubject );
				parentNode.TargetDefinition.Subjects.ItemsList = null;
				parentNode.TargetDefinition.Subjects.IsAny = true;
			}
		}
		private void DeleteFromPolicySet( TreeNodes.NoBoldNode childNode )
		{
			TreeNodes.PolicySet parentNode = (TreeNodes.PolicySet)mainTree.SelectedNode.Parent;
			if( childNode is TreeNodes.Policy )
			{
				TreeNodes.Policy policyNode = (TreeNodes.Policy)childNode;
				parentNode.PolicySetDefinition.Policies.Remove( policyNode.PolicyDefinition );
			}
			else if( childNode is TreeNodes.PolicySet )
			{
				TreeNodes.PolicySet policySetNode = (TreeNodes.PolicySet)childNode;
				parentNode.PolicySetDefinition.Policies.Remove( policySetNode.PolicySetDefinition );
			}
			else if( childNode is TreeNodes.Obligations )
			{
				parentNode.PolicySetDefinition.Obligations = null;
			}
			else if( childNode is TreeNodes.Target )
			{
				parentNode.PolicySetDefinition.Target = null;
			}
		}

		private void DeleteFromPolicy( TreeNodes.NoBoldNode childNode )
		{
			TreeNodes.Policy parentNode = (TreeNodes.Policy)mainTree.SelectedNode.Parent;
			if( childNode is TreeNodes.Rule )
			{
				pol.RuleElementReadWrite rule = ((TreeNodes.Rule)childNode).RuleDefinition;
				int index = parentNode.PolicyDefinition.Rules.GetIndex( rule );
				parentNode.PolicyDefinition.Rules.RemoveAt( index );
			}
			else if( childNode is TreeNodes.Obligations )
			{
				parentNode.PolicyDefinition.Obligations = null;
			}
			else if( childNode is TreeNodes.Target )
			{
				parentNode.PolicyDefinition.Target = null;
			}
		}

		private void DeleteFromRule( TreeNodes.NoBoldNode childNode )
		{
			TreeNodes.Policy parentNode = (TreeNodes.Policy)mainTree.SelectedNode.Parent;
			if( childNode is TreeNodes.Rule )
			{
				parentNode.PolicyDefinition.Rules.RemoveAt( childNode.Index - 1 );
			}
			else if( childNode is TreeNodes.Obligations )
			{
				parentNode.PolicyDefinition.Obligations = null;
			}
			else if( childNode is TreeNodes.Target )
			{
				parentNode.PolicyDefinition.Target = null;
			}
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			if( saveFileDialog.ShowDialog() == DialogResult.OK )
			{
				XmlTextWriter writer = new XmlTextWriter( saveFileDialog.FileName,System.Text.Encoding.UTF8 );
				writer.Namespaces = true;
				writer.Formatting = Formatting.Indented;

				if( docType == DocumentType.Request )
				{
					con.ContextDocumentReadWrite oCon = ((ContextTreeNodes.Context)mainTree.TopNode).ContextDefinition;
					oCon.WriteRequestDocument(writer);
				}
				else if( docType == DocumentType.Policy )
				{
					pol.PolicyDocumentReadWrite oPol = ((TreeNodes.PolicyDocument)mainTree.TopNode).PolicyDocumentDefinition;
					oPol.WriteDocument(writer);
				}

				writer.Close();
			}
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			mainTree.Nodes.Clear();
			mainPanel.Controls.Clear();
			_path = string.Empty;
			menuItem2.Enabled = true;
			menuItem5.Enabled = true;
			menuItem3.Enabled = false;
			menuItem9.Enabled = false;
			menuItem8.Enabled = false;
			menuItem7.Enabled = false;
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			openFileDialog.Filter = "Request Files|*.xml|All Files|*.*";
			if( openFileDialog.ShowDialog() == DialogResult.OK )
			{
				System.IO.Stream stream = openFileDialog.OpenFile();
				_path = openFileDialog.FileName;
				docType = DocumentType.Request;
				con.ContextDocumentReadWrite doc = ContextLoader.LoadContextDocument( stream, XacmlVersion.Version11, DocumentAccess.ReadWrite );
				mainTree.Nodes.Add( new ContextTreeNodes.Context( doc ) );
				menuItem3.Enabled = true;
				menuItem9.Enabled = true;
				menuItem5.Enabled = false;
				menuItem2.Enabled = false;
				menuItem7.Enabled = true;
				menuItem8.Enabled = false;
				stream.Close();
			}
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			if( MessageBox.Show( this,"The request will be saved. Do you want to proceed?", "Warning", MessageBoxButtons.YesNo ) == DialogResult.Yes )
			{
				//Loads the policy
				openFileDialog.Filter = "Policy Files|*.xml|All Files|*.*";
				if( openFileDialog.ShowDialog() == DialogResult.OK )
				{
					menuItem9_Click( sender, e );
					pol.PolicyDocumentReadWrite oPol = PolicyLoader.LoadPolicyDocument( openFileDialog.OpenFile(), XacmlVersion.Version11 );
					//Gets the context from the TreeView
					System.IO.Stream stream = new System.IO.FileStream( _path, FileMode.Open );
					con.ContextDocumentReadWrite oCon = ContextLoader.LoadContextDocument( stream , XacmlVersion.Version11 );
				
					stream.Close();

					//Evaluates the request
					Runtime.EvaluationEngine engine = new Runtime.EvaluationEngine();
					con.ResponseElement res = engine.Evaluate( (pol.PolicyDocument)oPol, (con.ContextDocument)oCon );
				
					mainPanel.Controls.Clear();
					//Creates the xml
					string path = Path.GetTempFileName();
					XmlWriter writer = new XmlTextWriter( path, System.Text.Encoding.UTF8 );
					res.WriteDocument( writer );
					writer.Close();

					mainPanel.Controls.Add( new ContextCustomControls.XmlViewer( path, Consts.ContextSchema.ResponseElement.Response ) );
				}
			}
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			if( MessageBox.Show( this,"The policy will be saved. Do you want to proceed?", "Warning", MessageBoxButtons.YesNo ) == DialogResult.Yes )
			{
				//Loads the request
				openFileDialog.Filter = "Request Files|*.xml|All Files|*.*";
				if( openFileDialog.ShowDialog() == DialogResult.OK )
				{
					menuItem9_Click( sender, e );
					con.ContextDocumentReadWrite oCon = ContextLoader.LoadContextDocument( openFileDialog.OpenFile(), XacmlVersion.Version11 );
					//Gets the policy from the TreeView
					System.IO.Stream stream = new System.IO.FileStream( _path, FileMode.Open );
					pol.PolicyDocumentReadWrite oPol = PolicyLoader.LoadPolicyDocument( stream , XacmlVersion.Version11 );
				
					stream.Close();

					//Evaluates the request
					Runtime.EvaluationEngine engine = new Runtime.EvaluationEngine();
					con.ResponseElement res = engine.Evaluate( (pol.PolicyDocument)oPol, (con.ContextDocument)oCon );

					//Creates the xml
					string path = Path.GetTempFileName();
					XmlWriter writer = new XmlTextWriter( path, System.Text.Encoding.UTF8 );
					res.WriteDocument( writer );
					writer.Close();
				
					mainPanel.Controls.Clear();

					mainPanel.Controls.Add(new ContextCustomControls.XmlViewer(path, Consts.ContextSchema.ResponseElement.Response));
				}
			}
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			XmlTextWriter writer = new XmlTextWriter( _path,System.Text.Encoding.UTF8 );
			writer.Namespaces = true;
			writer.Formatting = Formatting.Indented;

			if( docType == DocumentType.Request )
			{
				con.ContextDocumentReadWrite oCon = ((ContextTreeNodes.Context)mainTree.TopNode).ContextDefinition;
				oCon.WriteRequestDocument(writer);
			}
			else if( docType == DocumentType.Policy )
			{
				pol.PolicyDocumentReadWrite oPol = ((TreeNodes.PolicyDocument)mainTree.TopNode).PolicyDocumentDefinition;
				oPol.WriteDocument(writer);
			}

			writer.Close();		
		}
	}
}
