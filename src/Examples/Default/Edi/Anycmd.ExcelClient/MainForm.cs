
namespace Anycmd.ExcelClient
{
    using DataContracts;
    using Edi.Client;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MessageDto = Anycmd.Edi.ServiceModel.Operations.Message;

    public partial class MainForm : Form
    {
        readonly Panel _panelBack = new Panel();
        readonly ProgressBar _progressBarExcelExport = new ProgressBar();
        readonly Label _lblCaption = new Label() { Width = 200 };
        private readonly string _serviceBaseUrl;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly string _clientType;
        private readonly string _credentialType;
        private readonly string _signatureMethod;
        private readonly TemplateForm _templateForm = new TemplateForm();
        private readonly ExcelView _excelViewForm = new ExcelView();

        public MainForm()
        {
            InitializeComponent();

            _serviceBaseUrl = ConfigurationManager.AppSettings["serviceBaseUrl"];
            _clientId = ConfigurationManager.AppSettings["ClientId"];
            _secretKey = ConfigurationManager.AppSettings["SecretKey"];
            _credentialType = ConfigurationManager.AppSettings["CredentialType"];
            _signatureMethod = ConfigurationManager.AppSettings["SignatureMethod"];
            _clientType = ConfigurationManager.AppSettings["ClientType"];

            _panelBack.Hide();
            this._panelBack.Controls.Add(_progressBarExcelExport);
            this._panelBack.Controls.Add(_lblCaption);
            this.Controls.Add(_panelBack);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!IsConfiged())
            {
                return;
            }
        }

        private bool IsConfiged()
        {
            var sb = new StringBuilder();
            int l = sb.Length;
            if (string.IsNullOrEmpty(_serviceBaseUrl))
            {
                sb.AppendLine("未配置服务基地址;\n");
            }
            if (string.IsNullOrEmpty(_clientType))
            {
                sb.AppendLine("未配置客户端类型;\n");
            }
            if (string.IsNullOrEmpty(_clientId))
            {
                sb.AppendLine("未配置客户端标识;\n");
            }
            if (string.IsNullOrEmpty(_credentialType))
            {
                sb.AppendLine("未配置证书类型;\n");
            }
            if (string.IsNullOrEmpty(_secretKey))
            {
                sb.AppendLine("未配置密钥;\n");
            }

            if (sb.Length != l)
            {
                MessageBox.Show(sb.ToString());
            }

            return sb.Length == l;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();

            fd.Filter = "Excel|*.xls|所有文件|*.*";
            if (fd.ShowDialog() == DialogResult.OK)
                tbUrl.Text = fd.FileName;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            _excelViewForm.ShowDialog();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Icon ico = this.Icon;
            if (File.Exists("ExcelClient1.ico"))
            {
                this.Icon = new Icon("ExcelClient1.ico");
            }
            if (string.IsNullOrEmpty(tbUrl.Text))
            {
                MessageBox.Show("请先选择一个Excel文件，然后再导入。");
            }
            try
            {
                ShowProgressBar();
                FileStream fs = File.OpenRead(this.tbUrl.Text.Trim());
                IWorkbook workbook = new HSSFWorkbook(fs);//从流内容创建Workbook对象
                fs.Close();
                ICellStyle failStyle = workbook.CreateCellStyle();
                ICellStyle successStyle = workbook.CreateCellStyle();
                failStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                failStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                failStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                failStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                failStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
                failStyle.FillPattern = FillPattern.SolidForeground;

                successStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                successStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                successStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                successStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                successStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
                successStyle.FillPattern = FillPattern.SolidForeground;

                ISheet sheet = workbook.GetSheetAt(0);//获取第一个工作表
                if (sheet.LastRowNum == 2)
                {
                    this.Icon = ico;
                    HideProgressBar();
                    MessageBox.Show("没有待导入数据");
                    return;
                }
                int rowIndex = 0;
                IRow headRow1 = sheet.GetRow(rowIndex);
                rowIndex++;
                IRow headRow2 = sheet.GetRow(rowIndex);
                rowIndex++;
                IRow headRow3 = sheet.GetRow(rowIndex);
                rowIndex++;
                int actionCodeIndex = -1,
                    localEntityIdIndex = -1,
                    descriptionIndex = -1,
                    eventReasonPhraseIndex = -1,
                    eventSourceTypeIndex = -1,
                    eventStateCodeIndex = -1,
                    eventSubjectCodeIndex = -1,
                    infoIdKeysIndex = -1,
                    infoValueKeysIndex = -1,
                    isDumbIndex = -1,
                    timeStampIndex = -1,
                    ontologyCodeIndex = -1,
                    reasonPhraseIndex = -1,
                    requestTypeIndex = -1,
                    serverTicksIndex = -1,
                    stateCodeIndex = -1,
                    versionIndex = -1;
                string implicitMessageType = string.Empty,
                    implicitVerb = string.Empty,
                    implicitOntology = string.Empty,
                    implicitVersion = string.Empty,
                    implicitInfoIdKeys = string.Empty,
                    implicitInfoValueKeys = string.Empty;
                bool implicitIsDumb = false;
                #region 提取列索引
                for (int i = 0; i < headRow1.Cells.Count; i++)
                {
                    string value = headRow1.GetCell(i).SafeToStringTrim();
                    string implicitValue = headRow2.GetCell(i).SafeToStringTrim();
                    if (value != null)
                    {
                        value = value.ToLower();
                        if (value == CommandColHeader.Verb.ToLower())
                        {
                            actionCodeIndex = i;
                            implicitVerb = implicitValue;
                        }
                        else if (value == CommandColHeader.LocalEntityId.ToLower())
                        {
                            localEntityIdIndex = i;
                        }
                        else if (value == CommandColHeader.Description.ToLower())
                        {
                            descriptionIndex = i;
                        }
                        else if (value == CommandColHeader.EventReasonPhrase.ToLower())
                        {
                            eventReasonPhraseIndex = i;
                        }
                        else if (value == CommandColHeader.EventSourceType.ToLower())
                        {
                            eventSourceTypeIndex = i;
                        }
                        else if (value == CommandColHeader.EventStateCode.ToLower())
                        {
                            eventStateCodeIndex = i;
                        }
                        else if (value == CommandColHeader.EventSubjectCode.ToLower())
                        {
                            eventSubjectCodeIndex = i;
                        }
                        else if (value == CommandColHeader.InfoIdKeys.ToLower())
                        {
                            infoIdKeysIndex = i;
                            implicitInfoIdKeys = implicitValue;
                        }
                        else if (value == CommandColHeader.InfoValueKeys.ToLower())
                        {
                            infoValueKeysIndex = i;
                            implicitInfoValueKeys = implicitValue;
                        }
                        else if (value == CommandColHeader.IsDumb.ToLower())
                        {
                            isDumbIndex = i;
                            bool isDumb;
                            if (!bool.TryParse(implicitValue, out isDumb))
                            {
                                throw new ApplicationException("IsDumb值设置不正确");
                            }
                            implicitIsDumb = isDumb;
                        }
                        else if (value == CommandColHeader.TimeStamp.ToLower())
                        {
                            timeStampIndex = i;
                        }
                        else if (value == CommandColHeader.Ontology.ToLower())
                        {
                            ontologyCodeIndex = i;
                            implicitOntology = implicitValue;
                        }
                        else if (value == CommandColHeader.ReasonPhrase.ToLower())
                        {
                            reasonPhraseIndex = i;
                        }
                        else if (value == CommandColHeader.MessageId.ToLower())
                        {
                        }
                        else if (value == CommandColHeader.MessageType.ToLower())
                        {
                            requestTypeIndex = i;
                            implicitMessageType = implicitValue;
                        }
                        else if (value == CommandColHeader.ServerTicks.ToLower())
                        {
                            serverTicksIndex = i;
                        }
                        else if (value == CommandColHeader.StateCode.ToLower())
                        {
                            stateCodeIndex = i;
                        }
                        else if (value == CommandColHeader.Version.ToLower())
                        {
                            versionIndex = i;
                            implicitVersion = implicitValue;
                        }
                    }
                }
                #endregion
                int responsedSum = 0;
                int successSum = 0;
                int failSum = 0;
                for (int i = rowIndex; i <= sheet.LastRowNum; i++)
                {
                    decimal percent = (decimal)(((decimal)100 * i) / sheet.LastRowNum);
                    this._lblCaption.Text = "正在处理[" + percent.ToString("0.00") + "%][成功" + successSum.ToString() + "条，失败" + failSum.ToString() + "条]...";
                    _progressBarExcelExport.Value = Convert.ToInt32(percent);
                    Application.DoEvents();
                    var row = sheet.GetRow(i);
                    if (row == null)
                    {
                        break;
                    }
                    string infoIDKeys = row.GetCell(infoIdKeysIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(infoIDKeys))
                    {
                        infoIDKeys = implicitInfoIdKeys;
                    }
                    string infoValueKeys = row.GetCell(infoValueKeysIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(infoValueKeys))
                    {
                        infoValueKeys = implicitInfoValueKeys;
                    }
                    var infoIdCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var infoValueCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    if (infoIDKeys != null)
                    {
                        foreach (var item in infoIDKeys.Split(','))
                        {
                            infoIdCodes.Add(item);
                        }
                    }
                    if (infoValueKeys != null)
                    {
                        foreach (var item in infoValueKeys.Split(','))
                        {
                            infoValueCodes.Add(item);
                        }
                    }
                    var infoId = new List<KeyValue>();
                    var infoValue = new List<KeyValue>();
                    for (int j = 0; j < headRow1.Cells.Count; j++)
                    {
                        var elementCode = headRow1.GetCell(j).SafeToStringTrim();
                        if (!string.IsNullOrEmpty(elementCode) && elementCode[0] != '$')
                        {
                            var value = row.GetCell(j).SafeToStringTrim();
                            if (infoIdCodes.Contains(elementCode))
                            {
                                infoId.Add(new KeyValue(elementCode, value));
                            }
                            if (infoValueCodes.Contains(elementCode))
                            {
                                infoValue.Add(new KeyValue(elementCode, value));
                            }
                        }
                    }
                    if (infoId.Count == 0 || infoId.All(a => string.IsNullOrEmpty(a.Value)))
                    {
                        continue;
                    }
                    bool isDumb;
                    string isDumbValue = row.GetCell(isDumbIndex).SafeToStringTrim();
                    if (!string.IsNullOrEmpty(isDumbValue))
                    {
                        if (!bool.TryParse(isDumbValue, out isDumb))
                        {
                            throw new ApplicationException("IsDumb值设置不正确");
                        }
                    }
                    else
                    {
                        isDumb = implicitIsDumb;
                    }
                    string actionCode = row.GetCell(actionCodeIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(actionCode))
                    {
                        actionCode = implicitVerb;
                    }
                    string ontologyCode = row.GetCell(ontologyCodeIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(ontologyCode))
                    {
                        ontologyCode = implicitOntology;
                    }
                    var version = row.GetCell(versionIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(version))
                    {
                        version = implicitVersion;
                    }
                    var requestType = row.GetCell(requestTypeIndex).SafeToStringTrim();
                    if (string.IsNullOrEmpty(requestType))
                    {
                        requestType = implicitMessageType;
                    }
                    int eventStateCode = 0;
                    if (!string.IsNullOrEmpty(row.GetCell(eventStateCodeIndex).SafeToStringTrim()))
                    {
                        if (!int.TryParse(row.GetCell(eventStateCodeIndex).SafeToStringTrim(), out eventStateCode))
                        {
                            throw new ApplicationException("eventStateCode值设置错误");
                        }
                    }
                    if (!string.IsNullOrEmpty(row.GetCell(timeStampIndex).SafeToStringTrim()))
                    {
                        long timeStamp = 0;
                        if (!long.TryParse(row.GetCell(timeStampIndex).SafeToStringTrim(), out timeStamp))
                        {
                            throw new ApplicationException("timeStamp值设置错误");
                        }
                    }
                    var ticks = DateTime.UtcNow.Ticks;
                    var command = new MessageDto()
                    {
                        IsDumb = isDumb,
                        Verb = actionCode,
                        MessageId = Guid.NewGuid().ToString(),
                        Ontology = ontologyCode,
                        Version = version,
                        MessageType = requestType,
                        TimeStamp = DateTime.UtcNow.Ticks
                    };
                    command.Body = new BodyData(infoId.ToArray(), infoValue.ToArray())
                    {
                        Event = new EventData
                        {
                            ReasonPhrase = row.GetCell(eventReasonPhraseIndex).SafeToStringTrim(),
                            SourceType = row.GetCell(eventSourceTypeIndex).SafeToStringTrim(),
                            Status = eventStateCode,
                            Subject = row.GetCell(eventSubjectCodeIndex).SafeToStringTrim()
                        }
                    };
                    var credential = new CredentialData
                    {
                        ClientType = _clientType,
                        CredentialType = _credentialType,
                        SignatureMethod = _signatureMethod,
                        ClientId = _clientId,
                        Ticks = ticks
                    };
                    command.Credential = credential;
                    credential.Password = Signature.Sign(command.ToOrignalString(command.Credential), _secretKey);
                    var client = new JsonServiceClient(_serviceBaseUrl);
                    var response = client.Get(command);
                    responsedSum++;
                    if (response.Body.Event.Status < 200)
                    {
                        HideProgressBar();
                        MessageBox.Show(string.Format("{0} {1} {2}", (int)response.Body.Event.Status, response.Body.Event.ReasonPhrase, response.Body.Event.Description));
                        break;
                    }
                    var stateCodeCell = row.CreateCell(stateCodeIndex);
                    var reasonPhraseCell = row.CreateCell(reasonPhraseIndex);
                    var descriptionCell = row.CreateCell(descriptionIndex);
                    var serverTicksCell = row.CreateCell(serverTicksIndex);
                    var localEntityIdCell = row.CreateCell(localEntityIdIndex);
                    if (response.Body.Event.Status < 200 || response.Body.Event.Status >= 300)
                    {
                        failSum++;
                        stateCodeCell.CellStyle = failStyle;
                        reasonPhraseCell.CellStyle = failStyle;
                        descriptionCell.CellStyle = failStyle;
                    }
                    else
                    {
                        stateCodeCell.CellStyle = successStyle;
                        reasonPhraseCell.CellStyle = successStyle;
                        descriptionCell.CellStyle = successStyle;
                        successSum++;
                    }
                    stateCodeCell.SetCellValue(response.Body.Event.Status);
                    reasonPhraseCell.SetCellValue(response.Body.Event.ReasonPhrase);
                    descriptionCell.SetCellValue(response.Body.Event.Description);
                    serverTicksCell.SetCellValue(new DateTime(response.TimeStamp, DateTimeKind.Utc).ToLocalTime().ToString());
                    if (response.Body.InfoValue != null)
                    {
                        var localEntityIdItem = response.Body.InfoValue.FirstOrDefault(a => a.Key.Equals("Id", StringComparison.OrdinalIgnoreCase));
                        if (localEntityIdItem != null)
                        {
                            localEntityIdCell.SetCellValue(localEntityIdItem.Value);
                        }
                    }
                }
                this.Icon = ico;
                HideProgressBar();
                if (responsedSum == 0)
                {
                    MessageBox.Show("没有可导入行");
                }
                else
                {
                    var newFile = new FileStream(tbUrl.Text.Trim(), FileMode.Create);
                    workbook.Write(newFile);
                    newFile.Close();
                    System.Diagnostics.Process.Start("EXCEL.EXE", tbUrl.Text.Trim());
                }
            }
            catch (IOException)
            {
                this.Icon = ico;
                HideProgressBar();
                MessageBox.Show("文件读取失败，\"" + tbUrl.Text.Trim() + "\"可能不是Excel文件");
            }
            catch (Exception ex)
            {
                this.Icon = ico;
                HideProgressBar();
                MessageBox.Show(ex.Message);
            }
        }

        #region ShowProgressBar
        private void ShowProgressBar()
        {
            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;//获取屏幕高度，宽度
            _progressBarExcelExport.Minimum = 0;
            _progressBarExcelExport.Maximum = 100;
            _panelBack.Top = 0;
            _panelBack.Left = 0;
            _panelBack.Width = this.Width;
            _panelBack.Height = this.Height;
            _progressBarExcelExport.SetBounds((this.Width - this.Width / 2) / 2, this.Height / 2 - 50, this.Width / 2, 30);
            _progressBarExcelExport.BackColor = Color.Green;
            _panelBack.BringToFront();
            _lblCaption.BringToFront();
            _progressBarExcelExport.BringToFront();
            _lblCaption.Left = _progressBarExcelExport.Left + 15;
            _lblCaption.Top = _progressBarExcelExport.Top - 17;

            _progressBarExcelExport.Show();

            //写入数值
            _panelBack.Show();
            panel.Hide();
            Application.DoEvents();
        }
        #endregion

        private void HideProgressBar()
        {
            _panelBack.Hide();
            panel.Show();
            this._lblCaption.Text = "";
        }

        private void tbUrl_TextChanged(object sender, EventArgs e)
        {
            bool enabled = !string.IsNullOrEmpty(tbUrl.Text);
            btnImport.Enabled = enabled;
            btnBrowse.Enabled = enabled;
        }

        private void tsmiMakeTemplate_Click(object sender, EventArgs e)
        {
            _templateForm.ShowDialog();
        }
    }
}
