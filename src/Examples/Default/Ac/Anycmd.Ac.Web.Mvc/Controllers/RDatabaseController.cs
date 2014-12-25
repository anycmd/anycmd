
using System.Diagnostics;
using System.Linq;

namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Exceptions;
    using MiniUI;
    using Rdb;
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.RdbViewModels;

    /// <summary>
    /// 数据库模型视图控制器<see cref="Anycmd.Rdb.RDatabase"/>
    /// </summary>
    [Guid("ECFCD327-6E1F-483E-81DE-6D1036B5F9F9")]
    public class RDatabaseController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("数据库模块主页")]
        [Guid("03C93516-3B89-436E-B479-F73B73BD5D85")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("数据库文档列表页")]
        [Guid("FE3B73F7-073E-4B91-BD64-40D87E207C4F")]
        public ViewResultBase DbDocs()
        {
            return View();
        }

        [By("xuexs")]
        [Description("数据库文档详细页")]
        [Guid("8248AC6D-B29B-49D3-8A28-B0F733048AE2")]
        public ViewResultBase DbDoc(Guid databaseId)
        {
            RdbDescriptor rdb;
            if (!Host.Rdbs.TryDb(databaseId, out rdb))
            {
                throw new ValidationException("意外的关系数据库标识" + databaseId);
            }
            return View(rdb);
        }

        [By("xuexs")]
        [Description("数据库表")]
        [Guid("3AF46903-2D97-4F2A-8556-C9A09FA89706")]
        public ViewResultBase Tables()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("数据库视图")]
        [Guid("D2760030-A0AD-4784-8D0D-D369972964E3")]
        public ViewResultBase Views()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("视图定义")]
        [Guid("F54F2122-6B49-4E11-8C59-3B830AFA0EBF")]
        public ViewResultBase ViewDefinition()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("表空间")]
        [Guid("91822086-DFE5-4153-88FE-E2DB55B663BC")]
        public ViewResultBase TableSpaces()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("数据库表列")]
        [Guid("CD087D15-63B7-4229-A452-5FD65D68F467")]
        public ViewResultBase TableColumns()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("数据库视图列")]
        [Guid("B1BA9B55-1A1D-4824-BC8C-D3AC4A99919B")]
        public ViewResultBase ViewColumns()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("数据库详细信息")]
        [Guid("DC8A6702-2526-4DC1-9580-9CF81660CB65")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    Guid id;
                    if (Guid.TryParse(Request["id"], out id))
                    {
                        var data = GetRequiredService<IRdbMetaDataService>().GetDatabase(id);
                        return new PartialViewResult
                        {
                            ViewName = "Partials/Details",
                            ViewData = new ViewDataDictionary(DatabaseInfo.Create(data))
                        };
                    }
                }
                throw new ValidationException("非法的Guid标识" + Request["id"]);
            }
            else if (!string.IsNullOrEmpty(Request["isInner"]))
            {
                return new PartialViewResult { ViewName = "Partials/Details" };
            }
            else
            {
                return new ViewResult { ViewName = "Details" };
            }
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取数据库")]
        [Guid("4B37A319-5033-44A4-8ECA-C1AE0BE12B67")]
        public ActionResult Get(Guid? id)
        {
            RDatabase data = null;
            if (id.HasValue)
            {
                data = GetRequiredService<IRdbMetaDataService>().GetDatabase(id.Value);
            }

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("根据ID获取数据库详细信息")]
        [Guid("806ADDB2-905E-4A01-B4C7-58B651E9F6CF")]
        public ActionResult GetInfo(Guid? id)
        {
            DatabaseInfo data = null;
            if (id.HasValue)
            {
                data = DatabaseInfo.Create(GetRequiredService<IRdbMetaDataService>().GetDatabase(id.Value));
            }

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("根据ID获取数据库表文档")]
        [Guid("9FDE10E1-734B-4800-8016-49FF274EA4A4")]
        public ActionResult GetTable(Guid databaseId, string id)
        {
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbTable dbTable;
            if (!Host.DbTables.TryGetDbTable(db, id, out dbTable))
            {
                throw new ValidationException("意外的数据库表标识" + id);
            }

            return this.JsonResult(dbTable);
        }

        [By("xuexs")]
        [Description("根据ID获取数据库视图文档")]
        [Guid("8984CD2C-4406-429D-BED6-F8288AB04CCD")]
        public ActionResult GetView(Guid databaseId, string id)
        {
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbView dbView;
            if (!Host.DbViews.TryGetDbView(db, id, out dbView))
            {
                throw new ValidationException("意外的数据库视图标识" + id);
            }
            return this.JsonResult(dbView);
        }

        [By("xuexs")]
        [Description("获取视图定义")]
        [Guid("986733FD-1544-4237-B997-051EFFA15F9E")]
        public ActionResult GetViewDefinition(Guid databaseId, string viewId)
        {
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbView view;
            if (!Host.DbViews.TryGetDbView(db, viewId, out view))
            {
                throw new ValidationException("意外的数据库视图" + viewId);
            }
            return this.Content(GetRequiredService<IRdbMetaDataService>().GetViewDefinition(db, view));
        }

        [By("xuexs")]
        [Description("根据ID获取数据库表列文档")]
        [Guid("2C25AD73-3EAC-4A70-95EB-AFD93FC31701")]
        public ActionResult GetTableColumn(Guid databaseId, string id)
        {
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbTableColumn colum;
            if (!Host.DbTableColumns.TryGetDbTableColumn(db, id, out colum))
            {
                throw new ValidationException("意外的数据库表列标识" + id);
            }
            return this.JsonResult(colum);
        }

        [By("xuexs")]
        [Description("根据ID获取数据库视图列文档")]
        [Guid("19BDDE34-9D42-4126-9877-E0F42DD1399A")]
        public ActionResult GetViewColumn(Guid databaseId, string id)
        {
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            DbViewColumn colum;
            if (!Host.DbViewColumns.TryGetDbViewColumn(db, id, out colum))
            {
                throw new ValidationException("意外的数据库视图列标识" + id);
            }
            return this.JsonResult(colum);
        }

        [By("xuexs")]
        [Description("分页获取数据库")]
        [Guid("6884DF89-DAFF-4BFC-9A6F-7B1FEF6E81CC")]
        public ActionResult GetPlistDatabases(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistDatabases(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<IRDatabase> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("分页获取数据库表")]
        [Guid("8EAD5998-1496-4960-A31A-111ED6A0E251")]
        public ActionResult GetPlistTables(GetPlistTables requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistTables(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DbTable> { total = requestModel.Total.Value, data = data });
        }


        [By("xuexs")]
        [Description("查看表空间使用情况")]
        [Guid("056EF887-B698-43CD-B3BC-AA3193A56A99")]
        public ActionResult GetTableSpaces(Guid? databaseId, string sortField, string sortOrder)
        {
            if (!databaseId.HasValue)
            {
                throw new ValidationException("未传入databaseId");
            }
            RdbDescriptor db;
            if (!Host.Rdbs.TryDb(databaseId.Value, out db))
            {
                throw new ValidationException("意外的数据库Id");
            }
            var spaces = GetRequiredService<IRdbMetaDataService>().GetTableSpaces(db, sortField, sortOrder);

            return this.JsonResult(spaces);
        }

        [By("xuexs")]
        [Description("分页获取数据库表")]
        [Guid("9CC2A29C-3C89-4332-9AC5-5F3D5C667E35")]
        public ActionResult GetPlistViews(GetPlistViews requestModel)
        {
            if (!ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            var data = Host.GetPlistViews(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DbView> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("分页获取数据库表列")]
        [Guid("3D6B8A3B-469B-41D0-A680-C9780FF0FFDA")]
        public ActionResult GetPlistTableColumns(GetPlistTableColumns requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistTableColumns(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DbTableColumn> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("分页获取数据库视图列")]
        [Guid("79CFF591-45D6-4020-B0E1-A57BF59BA6F1")]
        public ActionResult GetPlistViewColumns(GetPlistViewColumns requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistViewColumns(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DbViewColumn> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("更新数据库信息")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("3A2958B0-D34C-4B51-9F41-2FA1749B55D4")]
        public ActionResult Update(DatabaseInput input)
        {
            var responseResult = new ResponseData { success = true, id = input.Id };
            if (ModelState.IsValid)
            {
                GetRequiredService<IRdbMetaDataService>().UpdateDatabase(input.Id, input.DataSource, input.Description);
            }
            else
            {
                responseResult.success = false;
                string msg = ModelState.Aggregate(string.Empty, (current1, item) => item.Value.Errors.Aggregate(current1, (current, e) => current + e.ErrorMessage));

                responseResult.msg = msg;
            }

            return this.JsonResult(responseResult);
        }

        [By("xuexs")]
        [Description("更新数据库表文档")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("4D6BC2D8-493C-4DC6-BC19-F1BA71A64D6F")]
        public ActionResult UpdateTable(DbTableInput input)
        {
            var responseResult = new ResponseData { success = true, id = input.Id };
            if (ModelState.IsValid)
            {
                RdbDescriptor db;
                if (!Host.Rdbs.TryDb(input.DatabaseId, out db))
                {
                    throw new ValidationException("意外的数据库Id");
                }
                GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.Table, input.Id, input.Description);
            }
            else
            {
                responseResult.success = false;
                string msg = ModelState.Aggregate(string.Empty, (current1, item) => item.Value.Errors.Aggregate(current1, (current, e) => current + e.ErrorMessage));

                responseResult.msg = msg;
            }

            return this.JsonResult(responseResult);
        }

        [By("xuexs")]
        [Description("更新数据库视图文档")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("A26B595A-4EF3-43A4-B12C-26E1CC3983AD")]
        public ActionResult UpdateView(DbViewInput input)
        {
            var responseResult = new ResponseData { success = true, id = input.Id };
            if (ModelState.IsValid)
            {
                RdbDescriptor db;
                if (!Host.Rdbs.TryDb(input.DatabaseId, out db))
                {
                    throw new ValidationException("意外的数据库Id");
                }
                GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.View, input.Id, input.Description);
            }
            else
            {
                responseResult.success = false;
                string msg = ModelState.Aggregate(string.Empty, (current1, item) => item.Value.Errors.Aggregate(current1, (current, e) => current + e.ErrorMessage));

                responseResult.msg = msg;
            }

            return this.JsonResult(responseResult);
        }

        [By("xuexs")]
        [Description("更新数据库表列文档")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("2933D6C1-9D10-476F-8D92-E2A0994F6D81")]
        public ActionResult UpdateTableColumn(DbTableColumnInput input)
        {
            var responseResult = new ResponseData { success = true, id = input.Id };
            if (ModelState.IsValid)
            {
                RdbDescriptor db;
                if (!Host.Rdbs.TryDb(input.DatabaseId, out db))
                {
                    throw new ValidationException("意外的数据库Id");
                }
                GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.TableColumn, input.Id, input.Description);
            }
            else
            {
                responseResult.success = false;
                string msg = ModelState.Aggregate(string.Empty, (current1, item) => item.Value.Errors.Aggregate(current1, (current, e) => current + e.ErrorMessage));

                responseResult.msg = msg;
            }

            return this.JsonResult(responseResult);
        }

        [By("xuexs")]
        [Description("更新数据库视图列文档")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("09C89C21-D36D-49C5-80C8-AB14A93C7D0B")]
        public ActionResult UpdateViewColumn(DbViewColumnInput input)
        {
            var responseResult = new ResponseData { success = true, id = input.Id };
            if (ModelState.IsValid)
            {
                RdbDescriptor db;
                if (!Host.Rdbs.TryDb(input.DatabaseId, out db))
                {
                    throw new ValidationException("意外的数据库Id");
                }
                GetRequiredService<IRdbMetaDataService>().CrudDescription(db, RDbMetaDataType.ViewColumn, input.Id, input.Description);
            }
            else
            {
                responseResult.success = false;
                string msg = ModelState.SelectMany(item => item.Value.Errors).Aggregate(string.Empty, (current, e) => current + e.ErrorMessage);

                responseResult.msg = msg;
            }

            return this.JsonResult(responseResult);
        }
    }
}
