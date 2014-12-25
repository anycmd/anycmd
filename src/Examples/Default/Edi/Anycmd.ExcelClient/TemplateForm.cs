
namespace Anycmd.ExcelClient
{
    using Edi.Client;
    using Edi.ServiceModel.Operations;
    using Edi.ServiceModel.Types;
    using NPOI.HSSF.UserModel;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public partial class TemplateForm : Form
    {
        private readonly string serviceBaseUrl;
        private List<OntologyData> ontologies;
        readonly Panel panelBack = new Panel();
        Label lblCaption = new Label() { Width = 200 };

        public TemplateForm()
        {
            InitializeComponent();

            panelBack.Hide();
            this.panelBack.Controls.Add(lblCaption);
            this.Controls.Add(panelBack);

            serviceBaseUrl = ConfigurationManager.AppSettings["serviceBaseUrl"];
            if (string.IsNullOrEmpty(serviceBaseUrl))
            {
                this.Close();
                MessageBox.Show("未配置服务基地址");
            }
        }

        private async void TemplateForm_Load(object sender, EventArgs e)
        {
            if (ontologies == null)
            {
                try
                {
                    ShowProgressBar();
                    lblCaption.Text = "努力加载中……";
                    var getAllOntology = new GetAllOntologies();
                    var client = new JsonServiceClient(serviceBaseUrl);
                    var response = await client.GetAsync(getAllOntology);
                    ontologies = response.Ontologies;
                    foreach (var ontology in ontologies)
                    {
                        if (!ontology.IsSystem)
                        {
                            var pnl = new FlowLayoutPanel()
                            {
                                Tag = ontology,
                                AutoScroll = true
                            };
                            var tabPage = new TabPage() { Text = ontology.Name, Tag = pnl };
                            tabPage.Controls.Add(pnl);
                            pnl.Dock = DockStyle.Fill;
                            tabControl.TabPages.Add(tabPage);
                            foreach (var element in ontology.Elements)
                            {
                                var chkb = new CheckBox()
                                {
                                    Text = element.Name + "(" + element.Key + ")",
                                    Tag = element,
                                    Width = 150
                                };
                                chkb.CheckedChanged += chkb_CheckedChanged;
                                pnl.Controls.Add(chkb);
                            }
                        }
                    }
                    HideProgressBar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpenEnabledUpdate();
        }

        private void chkb_CheckedChanged(object sender, EventArgs e)
        {
            btnOpenEnabledUpdate();
        }

        private void btnOpenEnabledUpdate()
        {
            bool anyIsChecked = false;
            var pnl = tabControl.SelectedTab.Tag as FlowLayoutPanel;
            if (pnl == null)
            {
                throw new ApplicationException();
            }
            foreach (var item in pnl.Controls)
            {
                var checkBox = item as CheckBox;
                if (checkBox != null)
                {
                    var chkb = checkBox;
                    if (chkb.Checked)
                    {
                        anyIsChecked = true;
                    }
                }
            }
            btnOpen.Enabled = anyIsChecked;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var pnl = tabControl.SelectedTab.Tag as FlowLayoutPanel;
                if (pnl == null)
                {
                    throw new ApplicationException();
                }
                var ontology = pnl.Tag as OntologyData;
                if (ontology == null)
                {
                    throw new ApplicationException();
                }
                var selectElements = new List<CommandColHeader>();
                foreach (var item in pnl.Controls)
                {
                    var checkBox = item as CheckBox;
                    if (checkBox != null)
                    {
                        var chkb = checkBox;
                        if (chkb.Checked)
                        {
                            var element = (ElementData)checkBox.Tag;
                            selectElements.Add(new CommandColHeader()
                            {
                                Code = element.Key,
                                Name = element.Name
                            });
                        }
                    }
                }
                if (selectElements.Count == 0)
                {
                    MessageBox.Show("没有勾选任何项");
                    return;
                }

                var commandModel = new CommandWorkbook(ontology.Name, ontology.Code, selectElements);
                // 操作Excel
                HSSFWorkbook hssfworkbook = commandModel.Workbook;

                string dataBaseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
                if (!Directory.Exists(dataBaseDir))
                {
                    Directory.CreateDirectory(dataBaseDir);
                }
                FileStream fs = File.Open(Path.Combine(dataBaseDir, ontology.Code + "Template.xls"), FileMode.OpenOrCreate);
                hssfworkbook.Write(fs);
                fs.Flush();
                fs.Close();
                System.Diagnostics.Process.Start("EXCEL.EXE", fs.Name);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region ShowProgressBar
        private void ShowProgressBar()
        {
            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;//获取屏幕高度，宽度
            panelBack.Top = 0;
            panelBack.Left = 0;
            panelBack.Width = this.Width;
            panelBack.Height = this.Height;
            panelBack.BringToFront();
            lblCaption.BringToFront();

            //写入数值
            panelBack.Show();
            Application.DoEvents();
        }
        #endregion

        private void HideProgressBar()
        {
            panelBack.Hide();
            this.lblCaption.Text = "";
        }
    }
}
