
namespace Anycmd.Xacml.ControlCenter.ContextCustomControls
{
	/// <summary>
	/// Summary description for Response.
	/// </summary>
	public class XmlViewer : System.Windows.Forms.UserControl
	{
		private AxSHDocVw.AxWebBrowser axWebBrowser;
		private System.Windows.Forms.GroupBox grpViewer;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// A control that allows you to show an Xml file
		/// </summary>
		/// <param name="path">The path of the xml</param>
		/// <param name="title">The title</param>
		public XmlViewer( string path, string title )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			grpViewer.Text = title;
	
			object fullPath = path;
			object a = "", b = "", c = "", d = "";
			axWebBrowser.Navigate2( ref fullPath, ref a, ref b, ref c, ref d );
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(XmlViewer));
			this.axWebBrowser = new AxSHDocVw.AxWebBrowser();
			this.grpViewer = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
			this.grpViewer.SuspendLayout();
			this.SuspendLayout();
			// 
			// axWebBrowser
			// 
			this.axWebBrowser.ContainingControl = this;
			this.axWebBrowser.Enabled = true;
			this.axWebBrowser.Location = new System.Drawing.Point(16, 24);
			this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
			this.axWebBrowser.Size = new System.Drawing.Size(616, 376);
			this.axWebBrowser.TabIndex = 2;
			// 
			// grpViewer
			// 
			this.grpViewer.Controls.Add(this.axWebBrowser);
			this.grpViewer.Location = new System.Drawing.Point(8, 16);
			this.grpViewer.Name = "grpViewer";
			this.grpViewer.Size = new System.Drawing.Size(648, 424);
			this.grpViewer.TabIndex = 3;
			this.grpViewer.TabStop = false;
			this.grpViewer.Text = "grpViewer";
			// 
			// XmlViewer
			// 
			this.Controls.Add(this.grpViewer);
			this.Name = "XmlViewer";
			this.Size = new System.Drawing.Size(664, 456);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
			this.grpViewer.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
