using System.Drawing;
using System.Windows.Forms;

namespace Anycmd.Xacml.ControlCenter.CustomControls
{
	/// <summary>
	/// Summary description for BaseControl.
	/// </summary>
	public class BaseControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		private bool _loadingData;

		/// <summary>
		/// 
		/// </summary>
		private bool _modified;

		/// <summary>
		/// 
		/// </summary>
		public BaseControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
			components = new System.ComponentModel.Container();
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( !_loadingData )
			{
				((ComboBox)sender).BackColor = Color.GreenYellow;
				_modified = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TextBox_TextChanged(object sender, System.EventArgs e)
		{
			if( !_loadingData )
			{
				((TextBox)sender).BackColor = Color.GreenYellow;
				_modified = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if( !_loadingData )
			{
				((CheckBox)sender).BackColor = Color.GreenYellow;
				_modified = true;
			}		
		}

		/// <summary>
		/// 
		/// </summary>
		protected bool LoadingData
		{
			get{ return _loadingData; }
			set{ _loadingData = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool Modified
		{
			get{ return _modified; }
		}
		/// <summary>
		/// 
		/// </summary>
		protected bool ModifiedValue
		{
			set{ _modified = value; }
		}
	}
}
