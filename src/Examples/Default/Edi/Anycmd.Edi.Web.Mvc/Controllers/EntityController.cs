
namespace Anycmd.Edi.Web.Mvc.Controllers
{
	using Anycmd.Web.Mvc;
	using Client;
	using DataContracts;
	using Engine.Ac;
	using Engine.Edi;
	using Engine.Host.Edi;
	using Engine.Hecp;
	using Engine.Info;
	using Exceptions;
	using MiniUI;
	using NPOI.HSSF.UserModel;
	using NPOI.HSSF.Util;
	using NPOI.POIFS.FileSystem;
	using NPOI.SS.UserModel;
	using Query;
	using ServiceModel.Operations;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using Util;
	using ViewModel;
	using ViewModels;

	/// <summary>
	/// 实体模型视图控制器。
	/// <para>实体是具体本体下的一个具体事物，这个事物可能存在物理世界的真实映射也可以是完
	/// 全虚构的事物。实体有一个重要属性是必须可以“标识”，也就是说必定可以区分出两个实
	/// 体的不同。在师生基础数据库中每一个教师是一个教师实体，每一个学生是一个学生实体。
	/// 师生基础中心库为每一个教师和学生实体分配唯一的编号，这个编号就是实体的唯一标识，
	/// 各业务系统通过该标识与中心系统交换信息。</para>
	/// <para>实体（Entity）就是交换平台所面向的业务数据，是要交换的数据，如教师、学生。
	/// 实体（Entity）在各节点有自己的存储标识，这个标识我们称为节点本地的实体（Entity）标识，记为LocalEntityID。
	/// 实体是具有相同属性描述的对象(人、地点、事物)。每一个实体都是唯一的，一个实体和另一个实体是不同的，
	/// 所以在各种系统中都会为实体分配一个唯一的标识。但在系统的不同层次可能会对业务上同一个实体采用不同的信息标识，
	/// 如在基于面向对象模式构建的系统中程序为每一个实体对象分配唯一的指针；在关系数据库系统中会为每一个实体记录
	/// 分配唯一的主键值等这都是实体标识。从中我们可以看出，实体标识是有上下文的：关系数据库的上下文是“存取”所以其主键是存取标识，
	/// 内存中的指针是内存块的索引标识。那么数据交换实体标识的上下文是“数据交换”。在Anycmd系统中，“数据交换实体”指的是教师（JS）和学生（XS）
	/// 这两类本体类别的记录。而“数据交换实体标识”指的是由中心系统所分配的整个交换平台中唯一的Guid编码。因为本系统是数据交换系统，
	/// 它的领域上下文是“数据交换”，所以当我们省略“数据交换”这个限定词而只说“实体”两字时它指的是“数据交换实体”。</para>
	/// </summary>
	[Guid("E768C201-C59A-4A96-8D36-29F224B2B11D")]
	public class EntityController : AnycmdController
	{
		private const string RESULT_SHEET_NAME = "Result";

		#region ViewResults
		/// <summary>
		/// 数据管理
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("数据管理")]
		[Guid("652ABFBD-648C-4173-9509-49903B17AC65")]
		public ViewResultBase Index(string ontologyCode)
		{
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("本体码不能为空");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			return View();
		}

		/// <summary>
		/// 数据详细信息
		/// </summary>
		/// <returns></returns>
		[By("xuexs")]
		[Description("数据详细信息")]
		[Guid("FFC4512F-458C-4928-A320-9CC0718504D9")]
		public ViewResultBase Details()
		{
			return ViewResult();
		}

		/// <summary>
		/// 用以控制权限，Action名和当前Action所在应用系统名、区域名、控制器名用来生成操作码和权限码。
		/// </summary>
		/// <returns></returns>
		[By("xuexs")]
		[Description("添加或修改")]
		[Guid("7A12F9BE-0EEC-48BD-B13A-DD8718332ACF")]
		public ViewResultBase Edit()
		{
			return ViewResult();
		}

		/// <summary>
		/// 数据导入界面
		/// </summary>
		/// <returns></returns>
		[By("xuexs")]
		[Description("数据导入界面")]
		[HttpGet]
		[Guid("1F0F78A3-A369-4427-B351-022BF747FDFE")]
		public ViewResultBase Import()
		{
			return ViewResult();
		}
		#endregion

		#region Get
		/// <summary>
		/// 根据本体码和数据ID获取数据
		/// </summary>
		/// <param name="nodeId"></param>
		/// <param name="ontologyCode"></param>
		/// <param name="id"></param>
		/// <param name="translate"></param>
		/// <param name="archiveId"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("根据本体码和数据ID获取数据")]
		[Guid("86ED94FF-05A3-4E7F-9FF9-F7FB874F32B4")]
		public ActionResult Get(string nodeId, string ontologyCode, string id, bool? translate, Guid? archiveId)
		{
			if (!archiveId.HasValue)
			{
				OntologyDescriptor ontology;
				if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
				{
					throw new ValidationException("非法的本体码");
				}
				var selectElements = new OrderedElementSet();
				foreach (var item in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
				{
					selectElements.Add(item);
				}
				var infoValues = ontology.EntityProvider.Get(ontology, id, selectElements);
				Dictionary<string, string> infoValueDic;
				if (translate.HasValue && translate.Value == true)
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Element.TranslateValue(v.Value));
				}
				else
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Value);
				}
				return this.JsonResult(infoValueDic);
			}
			else
			{
				ArchiveState archive;
				if (!archiveId.HasValue || !AcDomain.NodeHost.Ontologies.TryGetArchive(archiveId.Value, out archive))
				{
					throw new ValidationException("意外的归档Id");
				}
				OntologyDescriptor ontology = archive.Ontology;
				var queryInfoIDs = new List<InfoItem>();
				queryInfoIDs.Add(InfoItem.Create(ontology.IdElement, id));
				var selectElements = new OrderedElementSet();
				foreach (var item in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
				{
					selectElements.Add(item);
				}
				var infoValues = ontology.EntityProvider.Get(archive, id, selectElements);
				Dictionary<string, string> infoValueDic;
				if (translate.HasValue && translate.Value == true)
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Element.TranslateValue(v.Value));
				}
				else
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Value);
				}
				return this.JsonResult(infoValueDic);
			}
		}
		#endregion

		#region GetInfo
		/// <summary>
		/// 根据本体码和数据ID获取数据
		/// </summary>
		/// <param name="nodeId"></param>
		/// <param name="ontologyCode"></param>
		/// <param name="id"></param>
		/// <param name="translate"></param>
		/// <param name="archiveId"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("根据本体码和数据ID获取数据")]
		[Guid("4A0A8E2C-AC17-4334-9E9E-6D6A26FE22EA")]
		public ActionResult GetInfo(string nodeId, string ontologyCode, string id, bool? translate, Guid? archiveId)
		{
			if (!archiveId.HasValue)
			{
				OntologyDescriptor ontology;
				if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
				{
					throw new ValidationException("非法的本体码");
				}
				var selectElements = new OrderedElementSet();
				foreach (var item in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
				{
					selectElements.Add(item);
				}
				var infoValues = ontology.EntityProvider.Get(ontology, id, selectElements);
				Dictionary<string, string> infoValueDic;
				if (translate.HasValue && translate.Value == true)
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Element.TranslateValue(v.Value));
				}
				else
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Value);
				}
				return this.JsonResult(infoValueDic);
			}
			else
			{
				ArchiveState archive;
				if (!archiveId.HasValue || !AcDomain.NodeHost.Ontologies.TryGetArchive(archiveId.Value, out archive))
				{
					throw new ValidationException("意外的归档Id");
				}
				OntologyDescriptor ontology = archive.Ontology;
				var queryInfoIDs = new List<InfoItem>();
				queryInfoIDs.Add(InfoItem.Create(ontology.IdElement, id));
				var selectElements = new OrderedElementSet();
				foreach (var item in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
				{
					selectElements.Add(item);
				}
				var infoValues = ontology.EntityProvider.Get(archive, id, selectElements);
				Dictionary<string, string> infoValueDic;
				if (translate.HasValue && translate.Value == true)
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Element.TranslateValue(v.Value));
				}
				else
				{
					infoValueDic = infoValues.ToDictionary(k => k.Element.Element.Code, v => v.Value);
				}
				return this.JsonResult(infoValueDic);
			}
		}
		#endregion

		#region GetPlistEntity
		/// <summary>
		/// 分页获取数据
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("分页获取数据")]
		[Guid("E6C8A490-32D5-44E4-B2C7-8438FCF64F8D")]
		public ActionResult GetPlistEntity(GetPlistEntity requestModel)
		{
			if (!ModelState.IsValid)
			{
				return ModelState.ToJsonResult();
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(requestModel.OntologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码");
			}
			if (ontology.Ontology.IsOrganizationalEntity)
			{
				if (string.IsNullOrEmpty(requestModel.OrganizationCode))
				{
					throw new ValidationException("没有选中组织结构");
				}
				OrganizationState org;
				if (!AcDomain.OrganizationSet.TryGetOrganization(requestModel.OrganizationCode, out org))
				{
					throw new ValidationException("非法的组织结构码" + requestModel.OrganizationCode);
				}
			}
			var selectElements = new OrderedElementSet();
			foreach (var element in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
			{
				if (element.Element != null && element.Element.IsGridColumn)
				{
					selectElements.Add(element);
				}
			}
			if (selectElements.Count == 0)
			{
				throw new ValidationException("selectElements为空");
			}
			IDataTuples infoValues;
			infoValues = !requestModel.ArchiveId.HasValue ? GetInfoValues(requestModel, selectElements) : GetArchivedInfoValues(requestModel);
			var data = new List<Dictionary<string, string>>();
			foreach (var row in infoValues.Tuples)
			{
				var dic = new Dictionary<string, string>();
				for (int i = 0; i < infoValues.Columns.Count; i++)
				{
					string value;
					if (requestModel.Translate.HasValue && requestModel.Translate.Value)
					{
						value = infoValues.Columns[i].TranslateValue(row[i].ToString());
					}
					else
					{
						value = row[i].ToString();
					}
					dic.Add(infoValues.Columns[i].Element.Code, value);
				}
				data.Add(dic);
			}

			return this.JsonResult(new MiniGrid { total = requestModel.Total.Value, data = data });
		}
		#endregion

		#region DownloadTemplate
		/// <summary>
		/// 下载模板
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="elements"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("下载模板")]
		[Guid("934759D6-BA21-4F47-B2E1-E84540F038A4")]
		public ActionResult DownloadTemplate(string ontologyCode, string elements)
		{
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			var selectElements = new List<CommandColHeader>();
			if (string.IsNullOrEmpty(elements))
			{
				throw new ValidationException("未选中任何项");
			}
			else
			{
				string[] elementCodes = elements.Split(',');
				foreach (var elementCode in elementCodes)
				{
					ElementDescriptor element;
					if (!ontology.Elements.TryGetValue(elementCode, out element))
					{
						throw new ValidationException("意外的本体元素码" + elementCode);
					}
					else
					{
						if (element.Element.IsEnabled != 1)
						{
							continue;
						}
						selectElements.Add(new CommandColHeader()
						{
							Code = element.Element.Code,
							Name = element.Element.Name
						});
					}
				}
			}
			if (selectElements.Count == 0)
			{
				throw new ValidationException("selectElements为空");
			}
			var commandModel = new CommandWorkbook(ontology.Ontology.Name, ontology.Ontology.Code, selectElements);
			// 操作Excel
			HSSFWorkbook hssfworkbook = commandModel.Workbook;
			var filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
			hssfworkbook.Write(filestream); //把文件读到内存流里面
			string contentType = "application/vnd.ms-excel";
			string fileName = ontology.Ontology.Name + "模板.xls";
			if (Request.Browser.Type.IndexOf("IE", StringComparison.OrdinalIgnoreCase) > -1)
			{
				fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
			}
			Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
			Response.Clear();
			return new FileContentResult(filestream.GetBuffer(), contentType);
		}
		#endregion

		#region DownloadResult
		/// <summary>
		/// 下载执行结果
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("下载执行结果")]
		[Guid("C60A0C4D-730F-4FD7-9C1A-806B3952B5FF")]
		public ActionResult DownloadResult(string ontologyCode, string fileName)
		{
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
			string fullName = Path.Combine(dirPath, fileName);
			if (!System.IO.File.Exists(fullName))
			{
				throw new ValidationException("下载的文件不存在" + fullName);
			}
			// 操作Excel
			FileStream fs = System.IO.File.OpenRead(fullName);
			IWorkbook workbook = new HSSFWorkbook(fs);//从流内容创建Workbook对象
			fs.Close();

			ISheet sheet = workbook.GetSheet(RESULT_SHEET_NAME);
			var sheetIndex = workbook.GetSheetIndex(sheet);
			for (int i = 0; i < workbook.NumberOfSheets; i++)
			{
				if (i != sheetIndex)
				{
					workbook.RemoveSheetAt(i);
				}
			}
			sheetIndex = workbook.GetSheetIndex("Failed");
			if (sheetIndex >= 0)
			{
				workbook.RemoveSheetAt(sheetIndex);
			}
			ISheet failedSheet = workbook.CreateSheet("Failed");
			if (sheet.LastRowNum == 2)
			{
				throw new ValidationException("没有待导入数据");
			}
			int rowIndex = 0;
			IRow headRow0 = sheet.GetRow(rowIndex);
			var columnWidthDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < headRow0.Cells.Count; i++)
			{
				var cell = headRow0.Cells[i];
				columnWidthDic.Add(cell.SafeToStringTrim(), sheet.GetColumnWidth(i));
			}
			IRow failedRow0 = failedSheet.CreateRow(rowIndex);
			var cells = headRow0.Cells;
			for (int i = 0; i < cells.Count; i++)
			{
				var cell = failedRow0.CreateCell(i);
				cell.CellStyle = cells[i].CellStyle;
				cell.SetCellValue(cells[i].SafeToStringTrim());
			}
			rowIndex++;
			IRow headRow1 = sheet.GetRow(rowIndex);
			IRow failedRow1 = failedSheet.CreateRow(rowIndex);
			cells = headRow1.Cells;
			for (int i = 0; i < cells.Count; i++)
			{
				var cell = failedRow1.CreateCell(i);
				cell.CellStyle = cells[i].CellStyle;
				cell.SetCellValue(cells[i].SafeToStringTrim());
			}
			rowIndex++;
			IRow headRow2 = sheet.GetRow(rowIndex);
			IRow failedRow2 = failedSheet.CreateRow(rowIndex);
			cells = headRow2.Cells;
			for (int i = 0; i < cells.Count; i++)
			{
				var cell = failedRow2.CreateCell(i);
				cell.CellStyle = cells[i].CellStyle;
				cell.SetCellValue(cells[i].SafeToStringTrim());
			}
			failedSheet.CreateFreezePane(0, 3, 0, 3);
			rowIndex++;
			int resultFailedRowIndex = rowIndex;
			int stateCodeIndex = -1;
			int localEntityIdIndex = -1;
			int infoValueKeysIndex = -1;
			int infoIdKeysIndex = -1;
			for (int i = 0; i < headRow0.Cells.Count; i++)
			{
				var value = headRow0.GetCell(i).SafeToStringTrim();
				if (CommandColHeader.StateCode.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					stateCodeIndex = i;
					break;
				}
			}
			if (stateCodeIndex < 0)
			{
				throw new ValidationException("目标Excel中没有头为$StateCode的列");
			}
			for (int i = 0; i < headRow0.Cells.Count; i++)
			{
				var value = headRow0.GetCell(i).SafeToStringTrim();
				if (CommandColHeader.LocalEntityId.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					localEntityIdIndex = i;
					break;
				}
			}
			if (localEntityIdIndex < 0)
			{
				throw new ValidationException("目标Excel中没有头为$LocalEntityID的列");
			}
			for (int i = 0; i < headRow0.Cells.Count; i++)
			{
				var value = headRow0.GetCell(i).SafeToStringTrim();
				if (CommandColHeader.InfoValueKeys.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					infoValueKeysIndex = i;
					break;
				}
			}
			if (infoValueKeysIndex < 0)
			{
				throw new ValidationException("目标Excel中没有头为$InfoValueKeys的列");
			}
			for (int i = 0; i < headRow0.Cells.Count; i++)
			{
				var value = headRow0.GetCell(i).SafeToStringTrim();
				if (CommandColHeader.InfoIdKeys.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					infoIdKeysIndex = i;
					break;
				}
			}
			if (infoIdKeysIndex < 0)
			{
				throw new ValidationException("目标Excel中没有头为$InfoIDKeys的列");
			}
			string infoValueKeys = headRow1.GetCell(infoValueKeysIndex).SafeToStringTrim();
			if (string.IsNullOrEmpty(infoValueKeys))
			{
				throw new ValidationException("$InfoValueKeys单元格无值");
			}
			string infoIDKeys = headRow1.GetCell(infoIdKeysIndex).SafeToStringTrim();
			if (string.IsNullOrEmpty(infoIDKeys))
			{
				throw new ValidationException("$InfoIDKeys单元格无值");
			}
			var selectKeys = new List<string>();
			string[] keys = infoIDKeys.Split(',');
			if (keys == null || keys.Length == 0)
			{
				throw new ValidationException("$InfoIDKeys单元格内的值格式错误");
			}
			selectKeys.AddRange(keys);
			keys = infoValueKeys.Split(',');
			if (keys == null || keys.Length == 0)
			{
				throw new ValidationException("$InfoValueKeys单元格内的值格式错误");
			}
			selectKeys.AddRange(keys);
			var entityIDs = new List<string>();
			var selectElements = new OrderedElementSet();
			foreach (var key in selectKeys)
			{
				if (!ontology.Elements.ContainsKey(key))
				{
					throw new ValidationException("Excel文件的$InfoValueKeys单元格内有非法的本体元素码" + key);
				}
				if (ontology.Elements[key].Element.IsEnabled != 1)
				{
					continue;
				}
				selectElements.Add(ontology.Elements[key]);
			}
			if (ontology.Elements.ContainsKey("LoginName"))
			{
				if (!selectElements.Contains(ontology.Elements["LoginName"]))
				{
					selectElements.Add(ontology.Elements["LoginName"]);
				}
			}
			for (int i = rowIndex; i <= sheet.LastRowNum; i++)
			{
				var row = sheet.GetRow(i);
				if (row != null)
				{
					var stateCodeStr = row.GetCell(stateCodeIndex).SafeToStringTrim();
					if (!string.IsNullOrEmpty(stateCodeStr))
					{
						int stateCode;
						if (!int.TryParse(stateCodeStr, out stateCode))
						{
							throw new AnycmdException("文件" + fullName + "行中有意外的状态码");
						}
						if (stateCode >= 200 && stateCode < 300)
						{
							var cell = row.GetCell(localEntityIdIndex);
							entityIDs.Add(cell.SafeToStringTrim());
						}
						if (stateCode < 200 || stateCode >= 300)
						{
							IRow resultRow = failedSheet.CreateRow(resultFailedRowIndex);
							for (int j = 0; j < headRow0.Cells.Count; j++)
							{
								var cell = resultRow.CreateCell(j);
								var oldCell = row.GetCell(j);
								if (oldCell != null)
								{
									cell.CellStyle = oldCell.CellStyle;
									cell.SetCellValue(oldCell.SafeToStringTrim());
								}
							}
							resultFailedRowIndex++;
						}
					}
					sheet.RemoveRow(row);
				}
				rowIndex++;
			}
			sheet.RemoveRow(headRow0);
			sheet.RemoveRow(headRow1);
			sheet.RemoveRow(headRow2);
			workbook.SetSheetName(workbook.GetSheetIndex(sheet), "Success");
			#region Success 数据
			rowIndex = 0;
			var headRow = sheet.CreateRow(rowIndex);
			sheet.CreateFreezePane(0, 1, 0, 1);
			rowIndex++;
			ICellStyle helderStyle = workbook.CreateCellStyle();
			IFont font = workbook.CreateFont();
			font.FontHeightInPoints = 14;
			helderStyle.SetFont(font);
			helderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
			helderStyle.FillPattern = FillPattern.SolidForeground;
			int cellIndex = 0;
			foreach (var element in selectElements)
			{
				ICell cell = headRow.CreateCell(cellIndex, CellType.String);
				sheet.SetColumnHidden(cellIndex, hidden: false);
				if (element.IsCodeValue)
				{
					cell.SetCellValue(element.Element.Name + "码");
				}
				else
				{
					cell.SetCellValue(element.Element.Name);
				}
				if (!string.IsNullOrEmpty(element.Element.Description))
				{
					//添加批注
					IDrawing draw = sheet.CreateDrawingPatriarch();
					IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
					comment.String = new HSSFRichTextString(element.Element.Description);//添加批注内容
					comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
					cell.CellComment = comment;//将之前设置的批注给定某个单元格
				}
				cell.CellStyle = helderStyle;
				if (columnWidthDic.ContainsKey(element.Element.Code) && columnWidthDic[element.Element.Code] > 0)
				{
					sheet.SetColumnWidth(cellIndex, columnWidthDic[element.Element.Code]);
				}
				else if (element.Element.Width > 0)
				{
					sheet.SetColumnWidth(cellIndex, element.Element.Width * 256 / 5);
				}
				if (element.IsCodeValue)
				{
					cellIndex++;
					ICell nameCell = headRow.CreateCell(cellIndex, CellType.String);
					sheet.SetColumnHidden(cellIndex, hidden: false);
					nameCell.SetCellValue(element.Element.Name + "名");
					nameCell.CellStyle = helderStyle;
					if (columnWidthDic.ContainsKey(element.Element.Code) && columnWidthDic[element.Element.Code] > 0)
					{
						sheet.SetColumnWidth(cellIndex, columnWidthDic[element.Element.Code]);
					}
					else if (element.Element.Width > 0)
					{
						sheet.SetColumnWidth(cellIndex, element.Element.Width * 256 / 5);
					}
				}
				cellIndex++;
			}
			if (entityIDs.Count > 0)
			{
				DataTuple infoValues = ontology.EntityProvider.GetList(ontology, selectElements, entityIDs);
				foreach (var record in infoValues.Tuples)
				{
					var row = sheet.CreateRow(rowIndex);
					int j = 0;
					for (int col = 0; col < infoValues.Columns.Count; col++)
					{
						var element = infoValues.Columns[col];
						var item = record[col];
						ICell cell = row.CreateCell(j, CellType.String);
						cell.SetCellValue(item.ToString());
						if (element.IsCodeValue)
						{
							j++;
							ICell nameCell = row.CreateCell(j, CellType.String);
							nameCell.SetCellValue(element.TranslateValue(item.ToString()));
						}
						j++;
					}
					rowIndex++;
				}
			}
			#endregion
			var filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
			workbook.Write(filestream); //把文件读到内存流里面
			string contentType = "application/vnd.ms-excel";
			fileName = fileName.Substring(0, fileName.Length - 36 - ".xls".Length);
			fileName += "_result";
			if (Request.Browser.Type.IndexOf("IE") > -1)
			{
				fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
			}
			Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", fileName));
			Response.Clear();

			return new FileContentResult(filestream.GetBuffer(), contentType);
		}
		#endregion

		#region GetFiles
		/// <summary>
		/// 文件列表
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("文件列表")]
		[Guid("529ED44B-31AB-4760-BBF8-BF115FDDD1A0")]
		public ActionResult GetFiles(string ontologyCode)
		{
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("未传入本体码");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			var files = new FileInfo[0];
			string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
			if (Directory.Exists(dirPath))
			{
				var dirInfo = new DirectoryInfo(dirPath);
				files = dirInfo.GetFiles();
			}
			string userName = UserSession.Account.Name;

			return this.JsonResult(new MiniGrid
			{
				data = files.OrderByDescending(a => a.CreationTime)
					.Select(a => new
					{
						Id = a.Name,
						Name = a.Name.Substring(0, a.Name.Length - 36 - ".xls".Length),
						a.CreationTime,
						UserName = userName
					}),
				total = files.Length
			});
		}
		#endregion

		#region GetColumns
		/// <summary>
		/// 获取列头
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("获取列头")]
		[Guid("CF0349F2-9398-42DD-B026-0D43249D8D02")]
		public ActionResult GetColumns(string ontologyCode, string fileName)
		{
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("未传入本体码");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
			string fullName = Path.Combine(dirPath, fileName);
			if (System.IO.File.Exists(fullName))
			{
				FileStream fs = System.IO.File.OpenRead(fullName);
				IWorkbook workbook = new HSSFWorkbook(fs);//从流内容创建Workbook对象
				fs.Close();
				ISheet sheet = workbook.GetSheet(RESULT_SHEET_NAME);//获取结果工作表
				if (sheet == null)
				{
					throw new AnycmdException(fullName + "中没有名称为" + RESULT_SHEET_NAME + "的sheet");
				}
				IRow headRow1 = sheet.GetRow(0);
				var list = new List<MiniGridColumn>();
				list.Add(new MiniGridColumn { field = "", header = "", type = "indexcolumn", width = 60 });
				#region 提取列索引
				for (int i = 0; i < headRow1.Cells.Count; i++)
				{
					string column = headRow1.GetCell(i).SafeToStringTrim();
					if (!string.IsNullOrEmpty(column))
					{
						list.Add(new MiniGridColumn { field = column, width = 100, header = column });
					}
				}
				return this.JsonResult(new { columns = list.Select(a => new { a.field, a.header, a.type, a.width }) });
				#endregion
			}
			else
			{
				throw new ValidationException("文件不存在");
			}
		}
		#endregion

		#region GetFileRows
		/// <summary>
		/// 获取Excel文件内容
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("获取Excel文件内容")]
		[Guid("A9E5B2B2-77A6-4CC8-943D-DF63A3900BCB")]
		public ActionResult GetFileRows(string ontologyCode, string fileName)
		{
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("未传入本体码");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
			string fullName = Path.Combine(dirPath, fileName);
			if (!System.IO.File.Exists(fullName))
			{
				throw new ValidationException("文件不存在");
			}
			FileStream fs = System.IO.File.OpenRead(fullName);
			IWorkbook workbook = new HSSFWorkbook(fs);//从流内容创建Workbook对象
			fs.Close();

			ISheet sheet = workbook.GetSheet(RESULT_SHEET_NAME);
			IRow headRow1 = sheet.GetRow(0);
			var data = new List<Dictionary<string, string>>();
			for (int i = 1; i <= sheet.LastRowNum; i++)
			{
				var row = sheet.GetRow(i);
				if (row != null)
				{
					var dic = new Dictionary<string, string>();
					for (int j = 0; j < headRow1.Cells.Count; j++)
					{
						var column = headRow1.GetCell(j).SafeToStringTrim();
						if (!string.IsNullOrEmpty(column))
						{
							var value = row.GetCell(j).SafeToStringTrim();
							dic.Add(column, value);
						}
					}
					if (dic.Count != 0)
					{
						data.Add(dic);
					}
				}
			}
			return this.JsonResult(new MiniGrid { total = data.Count, data = data });
		}
		#endregion

		#region Import
		// TODO:重构，原因：太长
		/// <summary>
		/// 导入数据
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("导入数据")]
		[HttpPost]
		[Guid("DA91F8E9-50F9-46A0-B7BE-CE055AC879DF")]
		public ActionResult Import(string ontologyCode)
		{
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("未传入本体码");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			string message = "";
			if (Request.Files.Count == 0)
			{
				throw new ValidationException("错误:请上传文件!");
			}
			HttpPostedFileBase file = Request.Files[0];
			string fileName = file.FileName;
			if (string.IsNullOrEmpty(fileName) || file.ContentLength == 0)
			{
				message = "错误:请上传文件!";
			}
			else
			{
				bool isSave = true;
				string fileType = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
				fileName = fileName.Substring(0, fileName.Length - fileType.Length);
				if (file.ContentLength > 1024 * 1024 * 10)
				{
					message = "错误:文件大小不能超过10M!";
					isSave = false;
				}
				else if (fileType != ".xls")
				{
					message = "错误:文件上传格式不正确,请上传.xls格式文件!";
					isSave = false;
				}
				if (isSave)
				{
					string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
					DirectoryInfo dirInfo;
					dirInfo = !Directory.Exists(dirPath) ? Directory.CreateDirectory(dirPath) : new DirectoryInfo(dirPath);
					string fullName = Path.Combine(dirPath, fileName + Guid.NewGuid().ToString() + fileType);
					file.SaveAs(fullName);
					try
					{
						FileStream fs = System.IO.File.OpenRead(fullName);
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

						ISheet sheet = null;
						// 工作表sheet的命名规则是：本体码 或 本体名 或 ‘工作表’
						var sheetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
							ontology.Ontology.Code, ontology.Ontology.Name, "工作表","Failed","失败的","Sheet1"
						};
						foreach (var sheetName in sheetNames)
						{
							if (sheet != null)
							{
								break;
							}
							int dataSheetIndex = workbook.GetSheetIndex(sheetName);
							if (dataSheetIndex >= 0)
							{
								sheet = workbook.GetSheetAt(dataSheetIndex);
							}
						}
						if (sheet == null)
						{
							System.IO.File.Delete(fullName);
							throw new ValidationException("没有名称为'" + ontology.Ontology.Code + "'或'" + ontology.Ontology.Name + "'或'工作表'的sheet");
						}
						int sheetIndex = workbook.GetSheetIndex(sheet);
						workbook.SetSheetName(sheetIndex, RESULT_SHEET_NAME);
						for (int i = 0; i < workbook.NumberOfSheets; i++)
						{
							if (i != sheetIndex)
							{
								workbook.RemoveSheetAt(i);
							}
						}
						if (sheet.LastRowNum == 2)
						{
							System.IO.File.Delete(fullName);
							throw new ValidationException("没有待导入数据");
						}
						int rowIndex = 0;
						IRow headRow1 = sheet.GetRow(rowIndex);
						rowIndex++;
						IRow headRow2 = sheet.GetRow(rowIndex);
						rowIndex++;
						IRow headRow3 = sheet.GetRow(rowIndex);
						rowIndex++;
						#region 提取列索引 这些字段在Excel模板上对应前缀为“$”的列。
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
										System.IO.File.Delete(fullName);
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
						var commands = new Dictionary<int, Message>();
						#region 检测Excel中的每一行是否合法
						for (int i = rowIndex; i <= sheet.LastRowNum; i++)
						{
							// 检测合法性的进度，未展示进度条
							var percent = (decimal)(((decimal)100 * i) / sheet.LastRowNum);
							var row = sheet.GetRow(i);
							if (row != null)
							{
								string infoIdKeys = row.GetCell(infoIdKeysIndex).SafeToStringTrim();
								if (string.IsNullOrEmpty(infoIdKeys))
								{
									infoIdKeys = implicitInfoIdKeys;
								}
								string infoValueKeys = row.GetCell(infoValueKeysIndex).SafeToStringTrim();
								if (string.IsNullOrEmpty(infoValueKeys))
								{
									infoValueKeys = implicitInfoValueKeys;
								}
								var infoIdCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
								var infoValueCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
								if (infoIdKeys != null)
								{
									foreach (var item in infoIdKeys.Split(','))
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
								ontologyCode = row.GetCell(ontologyCodeIndex).SafeToStringTrim();
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
								long timeStamp = 0;
								if (!string.IsNullOrEmpty(row.GetCell(timeStampIndex).SafeToStringTrim()))
								{
									if (!long.TryParse(row.GetCell(timeStampIndex).SafeToStringTrim(), out timeStamp))
									{
										throw new ApplicationException("timeStamp值设置错误");
									}
								}
								responsedSum++;
								var ticks = DateTime.UtcNow.Ticks;
								var command = new Message()
								{
									IsDumb = isDumb,
									Verb = actionCode,
									MessageId = Guid.NewGuid().ToString(),
									Ontology = ontologyCode,
									Version = version,
									MessageType = requestType,
									TimeStamp = DateTime.UtcNow.Ticks,
									Body = new BodyData(infoId.ToArray(), infoValue.ToArray())
									{
										Event = new EventData
										{
											ReasonPhrase = row.GetCell(eventReasonPhraseIndex).SafeToStringTrim(),
											SourceType = row.GetCell(eventSourceTypeIndex).SafeToStringTrim(),
											Status = eventStateCode,
											Subject = row.GetCell(eventSubjectCodeIndex).SafeToStringTrim()
										}
									}
								};
								var credential = new CredentialData
								{
									ClientType = ClientType.Node.ToName(),
									CredentialType = CredentialType.Token.ToName(),
									ClientId = AcDomain.NodeHost.Nodes.ThisNode.Node.Id.ToString(),
									Ticks = ticks,
									UserName = UserSession.Account.Id.ToString()
								};
								command.Credential = credential;
								commands.Add(i, command);
							}
						}
						if (responsedSum == 0)
						{
							throw new ValidationException("没有可导入行");
						}
						else
						{
							foreach (var command in commands)
							{
								// 检测合法性的进度，未展示进度条
								decimal percent = (decimal)(((decimal)100 * command.Key) / commands.Count);
								var result = AnyMessage.Create(HecpRequest.Create(AcDomain, command.Value), AcDomain.NodeHost.Nodes.ThisNode).Response();
								if (result.Body.Event.Status < 200)
								{
									throw new ValidationException(string.Format("{0} {1} {2}", result.Body.Event.Status, result.Body.Event.ReasonPhrase, result.Body.Event.Description));
								}
								var row = sheet.GetRow(command.Key);
								var stateCodeCell = row.CreateCell(stateCodeIndex);
								var reasonPhraseCell = row.CreateCell(reasonPhraseIndex);
								var descriptionCell = row.CreateCell(descriptionIndex);
								var serverTicksCell = row.CreateCell(serverTicksIndex);
								var localEntityIDCell = row.CreateCell(localEntityIdIndex);
								if (result.Body.Event.Status < 200 || result.Body.Event.Status >= 300)
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
								stateCodeCell.SetCellValue(result.Body.Event.Status);
								reasonPhraseCell.SetCellValue(result.Body.Event.ReasonPhrase);
								descriptionCell.SetCellValue(result.Body.Event.Description);
								serverTicksCell.SetCellValue(DateTime.Now.ToString());
								if (result.Body.InfoValue != null)
								{
									var idItem = result.Body.InfoValue.Where(a => a.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
									if (idItem != null)
									{
										localEntityIDCell.SetCellValue(idItem.Value);
									}
								}
							}
							var newFile = new FileStream(fullName, FileMode.Create);
							workbook.Write(newFile);
							newFile.Close();
						}
						#endregion
					}
					catch (OfficeXmlFileException)
					{
						System.IO.File.Delete(fullName);
						throw new ValidationException("暂不支持Office2007及以上版本的Excel文件");
					}
				}
			}
			TempData["Message"] = message;
			return this.RedirectToAction("Import", new { ontologyCode });
		}
		#endregion

		#region DeleteFile
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="fileNames"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("删除文件")]
		[Guid("A8C4D2E4-8536-423E-8C82-9B0FB4485061")]
		public ActionResult DeleteFile(string ontologyCode, string fileNames)
		{
			if (string.IsNullOrEmpty(fileNames))
			{
				throw new ValidationException("未指定要删除的文件");
			}
			if (string.IsNullOrEmpty(ontologyCode))
			{
				throw new ValidationException("未传入本体码");
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码" + ontologyCode);
			}
			string dirPath = Server.MapPath("~/Content/Import/Excel/" + ontology.Ontology.Code + "/" + UserSession.Account.Id);
			string[] files = fileNames.Split('/');
			foreach (var fileName in files)
			{
				string fullName = Path.Combine(dirPath, fileName);
				if (System.IO.File.Exists(fullName))
				{
					System.IO.File.Delete(fullName);
				}
			}
			return this.JsonResult(new ResponseData { msg = "删除成功", success = true });
		}
		#endregion

		#region Export
		/// <summary>
		/// 导出数据
		/// </summary>
		/// <param name="requestModel"></param>
		/// <param name="elements"></param>
		/// <param name="limit"></param>
		/// <param name="exportType"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("导出数据")]
		[Guid("FF77E67B-9099-4FF4-8833-804DF779BFDA")]
		public ActionResult Export(GetPlistEntity requestModel, string elements, string limit, string exportType)
		{
			if (!ModelState.IsValid)
			{
				return ModelState.ToJsonResult();
			}
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(requestModel.OntologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码");
			}
			if (string.IsNullOrEmpty(requestModel.OrganizationCode))
			{
				throw new ValidationException("没有选中组织结构");
			}
			OrganizationState org;
			if (!AcDomain.OrganizationSet.TryGetOrganization(requestModel.OrganizationCode, out org))
			{
				throw new ValidationException("非法的组织结构码" + requestModel.OrganizationCode);
			}
			var selectElements = new OrderedElementSet();
			if (string.IsNullOrEmpty(elements))
			{
				foreach (var element in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
				{
					selectElements.Add(element);
				}
			}
			else
			{
				string[] elementCodes = elements.Split(',');
				foreach (var elementCode in elementCodes)
				{
					ElementDescriptor element;
					if (!ontology.Elements.TryGetValue(elementCode, out element))
					{
						throw new ValidationException("意外的本体元素码" + elementCode);
					}
					else
					{
						if (element.Element.IsEnabled != 1)
						{
							continue;
						}
						selectElements.Add(element);
					}
				}
			}
			if (selectElements.Count == 0)
			{
				throw new ValidationException("selectElements为空");
			}
			if (string.IsNullOrEmpty(exportType))
			{
				exportType = "currentPage";
			}
			exportType = exportType.ToLower();
			switch (exportType)
			{
				case "allpage":
					requestModel.PageSize = int.MaxValue;
					break;
				case "temp":
					requestModel.PageSize = 0;
					break;
				default:
					break;
			}
			if (!string.IsNullOrEmpty(limit))
			{
				int size;
				if (int.TryParse(limit, out size))
				{
					requestModel.PageSize = size;
				}
			}
			IDataTuples infoValues;
			infoValues = !requestModel.ArchiveId.HasValue ? GetInfoValues(requestModel, selectElements) : GetArchivedInfoValues(requestModel);
			string contentType = "application/vnd.ms-excel";
			string fileName = org.Name + ontology.Ontology.Name + ".xls";
			if (Request.Browser.Type.IndexOf("IE") > -1)
			{
				fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
			}
			Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
			Response.Clear();
			//Excel功能
			var hssfworkbook = new HSSFWorkbook(); //新建一个xls文件
			#region Sheet1 数据
			ISheet sheet1 = hssfworkbook.CreateSheet(ontology.Ontology.Name); //创建一个sheet
			int rowIndex = 0;
			var headRow = sheet1.CreateRow(rowIndex);
			sheet1.CreateFreezePane(0, 1, 0, 1);
			rowIndex++;
			ICellStyle helderStyle = hssfworkbook.CreateCellStyle();
			IFont font = hssfworkbook.CreateFont();
			font.FontHeightInPoints = 14;
			helderStyle.SetFont(font);
			helderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
			helderStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
			helderStyle.FillPattern = FillPattern.SolidForeground;
			int i = 0;
			foreach (var element in selectElements)
			{
				ICell cell = headRow.CreateCell(i, CellType.String);
				if (element.IsCodeValue)
				{
					cell.SetCellValue(element.Element.Name + "码");
				}
				else
				{
					cell.SetCellValue(element.Element.Name);
				}
				if (!string.IsNullOrEmpty(element.Element.Description))
				{
					//添加批注
					IDrawing draw = sheet1.CreateDrawingPatriarch();
					IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
					comment.String = new HSSFRichTextString(element.Element.Description);//添加批注内容
					comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
					cell.CellComment = comment;//将之前设置的批注给定某个单元格
				}
				cell.CellStyle = helderStyle;
				if (element.Element.Width > 0)
				{
					sheet1.SetColumnWidth(i, element.Element.Width * 256 / 5);
				}
				if (element.IsCodeValue)
				{
					i++;
					ICell nameValue = headRow.CreateCell(i, CellType.String);
					nameValue.SetCellValue(element.Element.Name + "名");
					nameValue.CellStyle = helderStyle;
					if (element.Element.Width > 0)
					{
						sheet1.SetColumnWidth(i, element.Element.Width * 256 / 5);
					}
				}
				i++;
			}
			foreach (var record in infoValues.Tuples)
			{
				var row = sheet1.CreateRow(rowIndex);
				int j = 0;
				for (int col = 0; col < infoValues.Columns.Count; col++)
				{
					var element = infoValues.Columns[col];
					var item = record[col];
					ICell cell = row.CreateCell(j, CellType.String);
					cell.SetCellValue(item.ToString());
					if (element.IsCodeValue)
					{
						j++;
						ICell nameCell = row.CreateCell(j, CellType.String);
						nameCell.SetCellValue(element.TranslateValue(item.ToString()));
					}
					j++;
				}
				rowIndex++;
			}
			#endregion
			#region orgSheet 组织结构字典
			ISheet orgSheet = hssfworkbook.CreateSheet("组织结构字典"); //创建一个sheet
			ICellStyle invalidOrgCodeStyle = hssfworkbook.CreateCellStyle();
			invalidOrgCodeStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidOrgCodeStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidOrgCodeStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidOrgCodeStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidOrgCodeStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
			invalidOrgCodeStyle.FillPattern = FillPattern.SolidForeground;
			ICellStyle invalidParentOrgStyle = hssfworkbook.CreateCellStyle();
			invalidParentOrgStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidParentOrgStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidParentOrgStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidParentOrgStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
			invalidParentOrgStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
			invalidParentOrgStyle.FillPattern = FillPattern.SolidForeground;
			rowIndex = 0;
			headRow = orgSheet.CreateRow(rowIndex);
			orgSheet.CreateFreezePane(0, 1, 0, 1);
			rowIndex++;
			i = 0;
			var orgCols = new string[] { "父名称", "父编码", "名称", "编码", "创建时间", "最后修改时间", "备注" };
			foreach (var colName in orgCols)
			{
				orgSheet.SetColumnWidth(i, 20 * 256);
				ICell cell = headRow.CreateCell(i, CellType.String);
				cell.SetCellValue(colName);
				cell.CellStyle = helderStyle;
				i++;
			}
			if (UserSession.IsDeveloper())
			{
				foreach (var item in AcDomain.OrganizationSet)
				{
					var parentOrg = item.Parent;
					var row = orgSheet.CreateRow(rowIndex);
					row.CreateCell(0, CellType.String).SetCellValue(parentOrg.Name);
					row.CreateCell(1, CellType.String).SetCellValue(parentOrg.Code);
					row.CreateCell(2, CellType.String).SetCellValue(item.Name);
					ICell codeCell = row.CreateCell(3, CellType.String);
					if (item != OrganizationState.VirtualRoot)
					{
						if (parentOrg == OrganizationState.Empty)
						{
							codeCell.CellStyle = invalidParentOrgStyle;
							//添加批注
							IDrawing draw = orgSheet.CreateDrawingPatriarch();
							IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
							comment.String = new HSSFRichTextString("警告：该组织结构的上级组织结构不存在。");//添加批注内容
							comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
							codeCell.CellComment = comment;//将之前设置的批注给定某个单元格
						}
						else if (parentOrg != OrganizationState.VirtualRoot && !item.Code.StartsWith(parentOrg.Code))
						{
							codeCell.CellStyle = invalidOrgCodeStyle;
							//添加批注
							IDrawing draw = orgSheet.CreateDrawingPatriarch();
							IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
							comment.String = new HSSFRichTextString("警告：该组织结构的编码没有以上级组织结构编码为前缀，这是错误的，后续是要改正的。");//添加批注内容
							comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
							codeCell.CellComment = comment;//将之前设置的批注给定某个单元格
						}
					}
					codeCell.SetCellValue(item.Code);
					string createOn = string.Empty;
					if (item.CreateOn.HasValue)
					{
						createOn = item.CreateOn.ToString();
					}
					string modifiedOn = string.Empty;
					if (item.ModifiedOn.HasValue)
					{
						modifiedOn = item.ModifiedOn.ToString();
					}
					row.CreateCell(4, CellType.String).SetCellValue(createOn);
					row.CreateCell(5, CellType.String).SetCellValue(modifiedOn);
					row.CreateCell(6, CellType.String).SetCellValue(item.Description);
					rowIndex++;
				}
			}
			else
			{
				foreach (var myOrg in UserSession.AccountPrivilege.Organizations)
				{
					foreach (var item in AcDomain.OrganizationSet)
					{
						if (item.Code.StartsWith(myOrg.Code, StringComparison.OrdinalIgnoreCase))
						{
							var row = orgSheet.CreateRow(rowIndex);
							var parentOrg = item.Parent;
							row.CreateCell(0, CellType.String).SetCellValue(parentOrg.Name);
							row.CreateCell(1, CellType.String).SetCellValue(parentOrg.Code);
							row.CreateCell(2, CellType.String).SetCellValue(item.Name);
							ICell codeCell = row.CreateCell(3, CellType.String);
							if (item != OrganizationState.VirtualRoot)
							{
								if (parentOrg == OrganizationState.Empty)
								{
									codeCell.CellStyle = invalidParentOrgStyle;
									//添加批注
									IDrawing draw = orgSheet.CreateDrawingPatriarch();
									IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
									comment.String = new HSSFRichTextString("警告：该组织结构的上级组织结构不存在。");//添加批注内容
									comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
									codeCell.CellComment = comment;//将之前设置的批注给定某个单元格
								}
								else if (parentOrg != OrganizationState.VirtualRoot && !item.Code.StartsWith(parentOrg.Code, StringComparison.OrdinalIgnoreCase))
								{
									codeCell.CellStyle = invalidOrgCodeStyle;
									//添加批注
									IDrawing draw = orgSheet.CreateDrawingPatriarch();
									IComment comment = draw.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 8));//里面参数应该是指示批注的位置大小吧
									comment.String = new HSSFRichTextString("警告：该组织结构的编码没有以上级组织结构编码为前缀，这是错误的，后续是要改正的。");//添加批注内容
									comment.Author = AcDomain.NodeHost.Nodes.ThisNode.Name;//添加批注作者
									codeCell.CellComment = comment;//将之前设置的批注给定某个单元格
								}
							}
							codeCell.SetCellValue(item.Code);
							string createOn = string.Empty;
							if (item.CreateOn.HasValue)
							{
								createOn = item.CreateOn.ToString();
							}
							string modifiedOn = string.Empty;
							if (item.ModifiedOn.HasValue)
							{
								modifiedOn = item.ModifiedOn.ToString();
							}
							row.CreateCell(4, CellType.String).SetCellValue(createOn);
							row.CreateCell(5, CellType.String).SetCellValue(modifiedOn);
							row.CreateCell(6, CellType.String).SetCellValue(item.Description);
							rowIndex++;
						}
					}
				}
			}
			#endregion
			#region infoDicSheet 信息字典
			ISheet infoDicSheet = hssfworkbook.CreateSheet("信息字典"); //创建一个sheet
			rowIndex = 0;
			headRow = infoDicSheet.CreateRow(rowIndex);
			infoDicSheet.CreateFreezePane(0, 1, 0, 1);
			rowIndex++;
			i = 0;
			var dicCols = new string[] { "字典名", "字典码", "字典项名", "字典项码", "等级", "创建时间", "最后修改时间", "备注" };
			foreach (var colName in dicCols)
			{
				infoDicSheet.SetColumnWidth(i, 20 * 256);
				ICell cell = headRow.CreateCell(i, CellType.String);
				cell.SetCellValue(colName);
				cell.CellStyle = helderStyle;
				i++;
			}
			foreach (var infoDic in AcDomain.NodeHost.InfoDics)
			{
				foreach (var infoDicItem in AcDomain.NodeHost.InfoDics.GetInfoDicItems(infoDic))
				{
					var row = infoDicSheet.CreateRow(rowIndex);
					row.CreateCell(0, CellType.String).SetCellValue(infoDic.Name);
					row.CreateCell(1, CellType.String).SetCellValue(infoDic.Code);
					row.CreateCell(2, CellType.String).SetCellValue(infoDicItem.Name);
					row.CreateCell(3, CellType.String).SetCellValue(infoDicItem.Code);
					row.CreateCell(4, CellType.String).SetCellValue(infoDicItem.Level);
					string createOn = string.Empty;
					if (infoDicItem.CreateOn.HasValue)
					{
						createOn = infoDicItem.CreateOn.ToString();
					}
					string modifiedOn = string.Empty;
					if (infoDicItem.ModifiedOn.HasValue)
					{
						modifiedOn = infoDicItem.ModifiedOn.ToString();
					}
					row.CreateCell(5, CellType.String).SetCellValue(createOn);
					row.CreateCell(6, CellType.String).SetCellValue(modifiedOn);
					row.CreateCell(7, CellType.String).SetCellValue(infoDicItem.Description);
					rowIndex++;
				}
			}
			#endregion
			var filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
			hssfworkbook.Write(filestream); //把文件读到内存流里面
			return new FileContentResult(filestream.GetBuffer(), contentType);
		}
		#endregion

		#region AddOrUpdate
		/// <summary>
		/// 添加或更新数据
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("添加或更新数据")]
		[HttpPost]
		[Guid("91251E0E-EBCC-43AD-BC41-269D2C32BBFE")]
		public ActionResult AddOrUpdate(string ontologyCode)
		{
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码");
			}
			NameValueCollection form = Request.Form;
			bool isCreate = string.IsNullOrEmpty(form["id"]);
			IList<InfoItem> infoValues = new List<InfoItem>();
			IList<InfoItem> infoIDs = new List<InfoItem>();
			foreach (string k in form.Keys)
			{
				if (!k.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					ElementDescriptor element;
					if (ontology.Elements.TryGetValue(k, out element))
					{
						if (element.Element.IsEnabled != 1)
						{
							continue;
						}
						if (isCreate)
						{
							if (!string.IsNullOrEmpty(form[k]) && form[k] != "undefined")
							{
								infoValues.Add(InfoItem.Create(element, form[k]));
							}
						}
						else
						{
							if (form["__" + k] != form[k])
							{
								infoValues.Add(InfoItem.Create(element, form[k]));
							}
						}
					}
				}
			}
			string id = form["Id"];
			infoIDs.Add(InfoItem.Create(ontology.IdElement, form["Id"]));
			var response = new ResponseData { success = true };
			if (infoValues.Count > 0)
			{
				IMessageDto commandResult;
				if (isCreate)
				{
					infoIDs.Add(InfoItem.Create(ontology.Elements["ZZJGM"], form["ZZJGM"]));
					infoIDs.Add(InfoItem.Create(ontology.Elements["XM"], form["XM"]));
					commandResult = this.Save(ontology, infoIDs, infoValues, out id);
					if (commandResult.Body.Event.Status == (int)Status.ToAudit)
					{
						response.success = false;
						response.msg = commandResult.Body.Event.Description;
						response.Warning();
					}
				}
				else
				{
					commandResult = this.Update(ontology, infoIDs, infoValues);
					if (commandResult.Body.Event.Status == (int)Status.ToAudit)
					{
						response.success = false;
						response.msg = commandResult.Body.Event.Description;
						response.Warning();
					}
				}
				if (commandResult.Body.Event.Status < 200 || commandResult.Body.Event.Status >= 300)
				{
					response.success = false;
					response.msg = commandResult.Body.Event.Description;
					response.Warning();
					if (commandResult.Body.Event.Status == (int)Status.AlreadyExist)
					{
						response.msg = "本组织结构下已经存在重名的用户";
					}
				}
			}
			else
			{
				response.success = false;
				response.msg = "数据无变化";
				response.Warning();
			}
			response.id = id;

			return this.JsonResult(response);
		}
		#endregion

		#region DeleteEntity
		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="ontologyCode"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[By("xuexs")]
		[Description("删除数据")]
		[HttpPost]
		[Guid("A3BAA1A8-C5FD-4033-A11A-BD527FAF59E0")]
		public ActionResult DeleteEntity(string ontologyCode, string id/*infoID*/)
		{
			var response = new ResponseData { success = true, id = id };
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码");
			}
			string[] infoIDs = id.Split(',');

			foreach (var item in infoIDs)
			{
				if (id != "-1")
				{
					var commandResult = this.DeleteEntity(ontology, new Guid(item));
					if (commandResult.Body.Event.Status == (int)Status.ToAudit)
					{
						response.success = false;
						response.msg = commandResult.Body.Event.Description;
						response.Info();
					}
					if (commandResult.Body.Event.Status < 200 || commandResult.Body.Event.Status >= 300)
					{
						response.success = false;
						response.msg = commandResult.Body.Event.Description;
						response.Warning();
					}
				}
			}

			return this.JsonResult(response);
		}
		#endregion

		#region GetInfoValues
		private IDataTuples GetInfoValues(
			GetPlistEntity requestModel, OrderedElementSet selectElements)
		{
			OntologyDescriptor ontology;
			if (!AcDomain.NodeHost.Ontologies.TryGetOntology(requestModel.OntologyCode, out ontology))
			{
				throw new ValidationException("非法的本体码");
			}
			requestModel.Includedescendants = requestModel.Includedescendants ?? false;
			IDataTuples infoValues = null;
			if (string.IsNullOrEmpty(requestModel.OrganizationCode) && !UserSession.IsDeveloper())
			{
				throw new ValidationException("对不起，您没有查看全部数据的权限");
			}
			else
			{
				if (ontology.Ontology.IsOrganizationalEntity && !string.IsNullOrEmpty(requestModel.OrganizationCode))
				{
					if (requestModel.Includedescendants.HasValue && requestModel.Includedescendants.Value)
					{
						requestModel.Filters.Add(FilterData.Like("ZZJGM", requestModel.OrganizationCode + "%"));
					}
					else
					{
						requestModel.Filters.Add(FilterData.EQ("ZZJGM", requestModel.OrganizationCode));
					}
				}
				infoValues = ontology.EntityProvider.GetPlist(
					ontology,
					selectElements,
					requestModel.Filters,
					requestModel);
			}

			return infoValues;
		}
		#endregion

		#region GetArchivedInfoValues
		private IDataTuples GetArchivedInfoValues(GetPlistEntity requestModel)
		{
			ArchiveState archive;
			if (!requestModel.ArchiveId.HasValue || !AcDomain.NodeHost.Ontologies.TryGetArchive(requestModel.ArchiveId.Value, out archive))
			{
				throw new ValidationException("意外的归档Id");
			}
			OntologyDescriptor ontology = archive.Ontology;
			requestModel.Includedescendants = requestModel.Includedescendants ?? false;
			IDataTuples infoValues = null;
			var selectElements = new OrderedElementSet();
			foreach (var element in ontology.Elements.Values.Where(a => a.Element.IsEnabled == 1))
			{
				if (element.Element != null && element.Element.IsGridColumn)
				{
					selectElements.Add(element);
				}
			}
			if (string.IsNullOrEmpty(requestModel.OrganizationCode) && !UserSession.IsDeveloper())
			{
				throw new ValidationException("对不起，您没有查看全部数据的权限");
			}
			else
			{
				if (ontology.Ontology.IsOrganizationalEntity && !string.IsNullOrEmpty(requestModel.OrganizationCode))
				{
					if (requestModel.Includedescendants.HasValue && requestModel.Includedescendants.Value)
					{
						requestModel.Filters.Add(FilterData.Like("ZZJGM", requestModel.OrganizationCode + "%"));
					}
					else
					{
						requestModel.Filters.Add(FilterData.EQ("ZZJGM", requestModel.OrganizationCode));
					}
				}
				infoValues = ontology.EntityProvider.GetPlist(
					archive,
					selectElements,
					requestModel.Filters,
					requestModel);
			}

			return infoValues;
		}
		#endregion

		#region Private Methods
		#region Save
		/// <summary>
		/// 保存新数据
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="infoIDs"></param>
		/// <param name="infoValues"></param>
		/// <param name="id"></param>
		/// <returns>命令状态模型</returns>
		/// <exception cref="AnycmdException">如果传入的信息ID字典或信息值字典为空则引发该异常，如果信息值
		/// 字典不为空但仅包含一个键为Id的值则也引发本异常（Id应出现在信息标识中是不应该出现在信息值中的）
		/// </exception>
		private IMessageDto Save(OntologyDescriptor ontology
			, IList<InfoItem> infoIDs, IList<InfoItem> infoValues, out string id)
		{
			return SaveOrUpdate(false, ontology, infoIDs, infoValues, out id);
		}
		#endregion

		#region Update
		/// <summary>
		/// 更新旧数据。调用者需自己找数据的变化，传入的infoValues是变化的数据
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="infoIDs"></param>
		/// <param name="infoValues"></param>
		/// <returns>命令状态模型</returns>
		/// <exception cref="AnycmdException">如果传入的信息ID字典或信息值字典为空则引发该异常，如果信息值
		/// 字典不为空但仅包含一个键为Id的值则也引发本异常（Id应出现在信息标识中是不应该出现在信息值中的）
		/// </exception>
		private IMessageDto Update(OntologyDescriptor ontology
			, IList<InfoItem> infoIDs, IList<InfoItem> infoValues)
		{
			string id;
			return SaveOrUpdate(true, ontology, infoIDs, infoValues, out id);
		}
		#endregion

		#region DeleteEntity
		/// <summary>
		/// 单条删除
		/// </summary>
		/// <param name="ontology"></param>
		/// <param name="id">单条数据的Guid信息标识</param>
		/// <returns></returns>
		private IMessageDto DeleteEntity(OntologyDescriptor ontology, Guid id)
		{
			var actionCode = "delete";
			var infoFormat = AcDomain.Config.InfoFormat;
			var infoIdDic = new List<InfoItem> {
				InfoItem.Create(ontology.IdElement, id.ToString())
			};
			var node = AcDomain.NodeHost.Nodes.ThisNode;
			var ticks = DateTime.UtcNow.Ticks;
			var cmd = new Message()
			{
				Version = ApiVersion.V1.ToName(),
				IsDumb = false,
				MessageType = MessageType.Action.ToName(),
				Credential = new CredentialData
				{
					ClientType = ClientType.Node.ToName(),
					UserType = UserType.None.ToName(),
					CredentialType = CredentialType.Token.ToName(),
					ClientId = node.Node.Id.ToString(),
					UserName = UserSession.Account.Id.ToString(),// UserName
					Password = TokenObject.Token(node.Node.Id.ToString(), ticks, node.Node.SecretKey),
					Ticks = ticks
				},
				Verb = actionCode,
				Ontology = ontology.Ontology.Code,
				TimeStamp = DateTime.UtcNow.Ticks,
				MessageId = Guid.NewGuid().ToString(),
				Body = new BodyData(new KeyValue[] { new KeyValue("Id", id.ToString()) }, new KeyValue[0])
				{
					Event = new EventData
					{
						Status = (int)Status.ReceiveOk,
						ReasonPhrase = Status.ReceiveOk.ToName()
					}
				}
			};

			return AnyMessage.Create(HecpRequest.Create(AcDomain, cmd), AcDomain.NodeHost.Nodes.ThisNode).Response();
		}
		#endregion

		#region SaveOrUpdate
		/// <summary>
		/// 保存新数据或更新旧数据
		/// </summary>
		/// <param name="isUpdate"></param>
		/// <param name="ontology"></param>
		/// <param name="infoIDs"></param>
		/// <param name="infoValues"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		private IMessageDto SaveOrUpdate(bool isUpdate, OntologyDescriptor ontology
			, ICollection<InfoItem> infoIDs, ICollection<InfoItem> infoValues, out string id)
		{
			id = string.Empty;
			if (infoIDs == null || infoIDs.Count == 0)
			{
				throw new AnycmdException("infoIDs不能为空");
			}
			if (infoValues == null || infoValues.Count == 0)
			{
				throw new AnycmdException("infoValues不能为空");
			}
			if (!isUpdate)
			{
				var idItem = infoIDs.FirstOrDefault(a => a.Element == ontology.IdElement);
				if (idItem != null)
				{
					infoIDs.Remove(idItem);
				}
			}

			IInfoStringConverter converter;
			if (!AcDomain.NodeHost.InfoStringConverters.TryGetInfoStringConverter(AcDomain.Config.InfoFormat, out converter))
			{
				throw new ValidationException("意外的信息字符串转化器，json信息格式转化器不存在或已被禁用");
			}
			var node = AcDomain.NodeHost.Nodes.ThisNode;
			var ticks = DateTime.UtcNow.Ticks;
			var cmd = new Message()
			{
				Version = ApiVersion.V1.ToName(),
				IsDumb = false,
				MessageType = MessageType.Action.ToName(),
				Credential = new CredentialData
				{
					ClientType = ClientType.Node.ToName(),
					UserType = UserType.None.ToName(),
					CredentialType = CredentialType.Token.ToName(),
					ClientId = node.Node.Id.ToString(),
					UserName = UserSession.Account.Id.ToString(),// UserName
					Password = TokenObject.Token(node.Node.Id.ToString(), ticks, node.Node.SecretKey),
					Ticks = ticks
				},
				MessageId = Guid.NewGuid().ToString(),
				Verb = isUpdate ? "update" : "create",
				Ontology = ontology.Ontology.Code,
				TimeStamp = DateTime.UtcNow.Ticks,
				Body = new BodyData(infoIDs.Select(a => new KeyValue(a.Key, a.Value)).ToArray(), infoValues.Select(a => new KeyValue(a.Key, a.Value)).ToArray())
				{
					Event = new EventData
					{
						Status = (int)Status.ReceiveOk,
						ReasonPhrase = Status.ReceiveOk.ToName()
					}
				}
			};
			var result = AnyMessage.Create(HecpRequest.Create(AcDomain, cmd), AcDomain.NodeHost.Nodes.ThisNode).Response();
			id = result.Body.InfoValue.Where(a => a.Key.Equals("Id", StringComparison.OrdinalIgnoreCase)).Single().Value;

			return result;
		}
		#endregion
		#endregion
	}
}
