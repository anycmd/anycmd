
namespace Anycmd.Edi.Client
{
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 命令模型
    /// </summary>
    public class CommandWorkbook
    {
        private HSSFWorkbook _workbook = null;

        /// <summary>
        /// 构建命令对象
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="ontologyCode"></param>
        /// <param name="elementHeader"></param>
        public CommandWorkbook(string sheetName, string ontologyCode, IList<CommandColHeader> elementHeader)
        {
            if (elementHeader == null || elementHeader.Count == 0)
            {
                throw new ArgumentNullException("elementHeader");
            }
            string infoValueKeys = string.Empty;
            int l = infoValueKeys.Length;
            foreach (var item in elementHeader)
            {
                if (infoValueKeys.Length != l)
                {
                    infoValueKeys += ",";
                }
                infoValueKeys += item.Code;
            }
            this.ElementHeader = elementHeader;
            this.SheetName = sheetName;
            this.CommandHeader = new List<CommandColHeader>{
                new CommandColHeader{ Code = CommandColHeader.StateCode, Name="状态码", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.ReasonPhrase, Name="原因短语", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.Description, Name="响应说明", DefaultValue="", IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.ServerTicks, Name="响应时间", DefaultValue="", IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.LocalEntityId, Name="本地实体标识", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.InfoIdKeys, Name="信息标识键", DefaultValue="xm,zzjgm", IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.InfoValueKeys, Name="信息值键", DefaultValue=infoValueKeys, IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.MessageId, Name="请求标识", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.MessageType, Name="请求类型", DefaultValue="Action", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.Verb, Name="动作码", DefaultValue="Create", IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.Ontology, Name="本体码", DefaultValue=ontologyCode, IsHidden=false},
                new CommandColHeader{ Code = CommandColHeader.EventSourceType, Name="事件源类型", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.EventSubjectCode, Name="事件主题码", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.EventStateCode, Name="事件状态码", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.EventReasonPhrase, Name="事件原因短语", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.IsDumb, Name="是哑弹", DefaultValue="False", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.TimeStamp, Name="本地时间戳", DefaultValue="", IsHidden=true},
                new CommandColHeader{ Code = CommandColHeader.Version, Name="协议版本号", DefaultValue="v1", IsHidden=true}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public string SheetName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CommandColHeader> CommandHeader { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CommandColHeader> ElementHeader { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HSSFWorkbook Workbook
        {
            get { return _workbook ?? (_workbook = CreateCommandTemplate()); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HSSFWorkbook CreateCommandTemplate()
        {
            // 操作Excel
            var hssfworkbook = new HSSFWorkbook();

            #region Sheet1 数据
            ISheet dataSheet = hssfworkbook.CreateSheet(this.SheetName); // 数据工作区

            ICellStyle dataHelderStyle = hssfworkbook.CreateCellStyle();
            dataHelderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            dataHelderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            dataHelderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            dataHelderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            dataHelderStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
            dataHelderStyle.FillPattern = FillPattern.SolidForeground;

            ICellStyle commandHelderStyle = hssfworkbook.CreateCellStyle();
            commandHelderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            commandHelderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            commandHelderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            commandHelderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            commandHelderStyle.FillForegroundColor = HSSFColor.LightBlue.Index;
            commandHelderStyle.FillPattern = FillPattern.SolidForeground;

            var rowIndex = 0;
            var headRow0 = dataSheet.CreateRow(rowIndex);
            rowIndex++;
            var headRow1 = dataSheet.CreateRow(rowIndex);
            rowIndex++;
            var headRow2 = dataSheet.CreateRow(rowIndex);
            dataSheet.CreateFreezePane(0, 3, 0, 3);
            int i = 0;
            // 写第一行本体元素码
            foreach (var element in this.ElementHeader)
            {
                var cell = headRow0.CreateCell(i, CellType.String);
                cell.CellStyle = dataHelderStyle;
                cell.SetCellValue(element.Code);
                i++;
            }
            i = 0;
            // 写第二行元素默认值
            foreach (var element in this.ElementHeader)
            {
                var cell = headRow1.CreateCell(i, CellType.String);
                cell.CellStyle = dataHelderStyle;
                cell.SetCellValue(string.Empty);
                i++;
            }
            i = 0;
            // 写第三行本体元素名
            foreach (var element in this.ElementHeader)
            {
                var cell = headRow2.CreateCell(i, CellType.String);
                cell.SetCellValue(element.Name);
                cell.CellStyle = dataHelderStyle;
                i++;
            }
            int j = i;
            // 写第一行命令元素码
            foreach (var item in this.CommandHeader)
            {
                var cell = headRow0.CreateCell(j, CellType.String);
                cell.CellStyle = commandHelderStyle;
                cell.SetCellValue(item.Code);
                if (item.IsHidden)
                {
                    dataSheet.SetColumnHidden(j, hidden: true);
                }
                j++;
            }
            j = i;
            // 写第二行命令元素默认值
            foreach (var item in this.CommandHeader)
            {
                var cell = headRow1.CreateCell(j, CellType.String);
                cell.CellStyle = commandHelderStyle;
                cell.SetCellValue(item.DefaultValue);
                j++;
            }
            j = i;
            // 写第三行命令元素名
            foreach (var item in this.CommandHeader)
            {
                var cell = headRow2.CreateCell(j, CellType.String);
                cell.SetCellValue(item.Name);
                cell.CellStyle = commandHelderStyle;
                j++;
            }
            #endregion

            return hssfworkbook;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet">要合并单元格所在的sheet</param>
        /// <param name="rowstart">开始行的索引</param>
        /// <param name="rowend">结束行的索引</param>
        /// <param name="colstart">开始列的索引</param>
        /// <param name="colend">结束列的索引</param>
        private void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            var cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
    }
}
