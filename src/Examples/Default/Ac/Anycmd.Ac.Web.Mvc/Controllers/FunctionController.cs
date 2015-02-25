
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.FunctionViewModels;
    using ViewModels.PrivilegeViewModels;

    /// <summary>
    /// 系统功能模型视图控制器
    /// </summary>
    [Guid("43C0730E-D533-4F2B-80D6-CEC399E7F764")]
    public class FunctionController : AnycmdController
    {
        #region ViewPages

        [By("xuexs")]
        [Description("功能集")]
        [Guid("7DE14ED7-4E9F-4470-8598-6C040C253B36")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("功能详细信息")]
        [Guid("1140ED5D-8D86-4E97-AD78-8C86FE0B3EC4")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = FunctionInfo.Create(base.EntityType.GetData(id));
                    return new PartialViewResult { ViewName = "Partials/Details", ViewData = new ViewDataDictionary(data) };
                }
                else
                {
                    throw new ValidationException("非法的Guid标识" + Request["id"]);
                }
            }
            else if (!string.IsNullOrEmpty(Request["isInner"]))
            {
                return new PartialViewResult { ViewName = "Partials/Details" };
            }
            else
            {
                return this.View();
            }
        }

        #endregion

        [By("xuexs")]
        [Description("刷新功能列表")]
        [DeveloperFilter(Order = 21)]
        [Guid("55D38422-4449-488B-8B53-9A6B9206184E")]
        public ActionResult Refresh()
        {
            var result = new ResponseData { success = true };
            if (!AcSession.IsDeveloper())
            {
                result.success = false;
                result.msg = "对不起，您不是开发人员，不能执行本功能";
            }
            else
            {
                GetRequiredService<IFunctionListImport>().Import(AcDomain, AcSession, AcDomain.Config.SelfAppSystemCode);
            }

            return this.JsonResult(result);
        }

        [By("xuexs")]
        [Description("根据ID获取功能")]
        [Guid("55430FEA-73DB-4939-9B9E-9D13468BD540")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取功能详细信息")]
        [Guid("510AE02B-5769-44F2-8AE8-8EC188671B80")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(FunctionInfo.Create(base.EntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("分页获取功能")]
        [Guid("84A035EF-5A03-4F33-AADF-00FD55A544BA")]
        public ActionResult GetPlistFunctions(GetPlistResult requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistFunctions(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<FunctionTr> { total = requestData.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加功能")]
        [DeveloperFilter(Order = 21)]
        [Guid("0F62D689-28A6-4F8D-A562-B616E5D09B6D")]
        public ActionResult Create(FunctionCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新功能信息")]
        [DeveloperFilter(Order = 21)]
        [Guid("53D288AD-DCFD-4730-A496-5E56025DA51F")]
        public ActionResult Update(FunctionUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除功能")]
        [DeveloperFilter(Order = 21)]
        [Guid("A8EDCE18-46E1-4A9D-909A-7F1FBCE735E0")]
        public ActionResult Delete(string id)
        {
            string[] ids = id.Split(',');
            var idArray = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    idArray[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的功能标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemoveFunctionCommand(AcSession, item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }

        [By("xuexs")]
        [Description("托管")]
        [Guid("731991CE-6C13-48BF-9F79-EA3072354B07")]
        public ActionResult Manage(string id)
        {
            string[] ids = id.Split(',');
            var idArray = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    idArray[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的功能标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                var entity = GetRequiredService<IRepository<Function>>().GetByKey(item);
                var input = new FunctionUpdateInput
                {
                    Code = entity.Code,
                    Description = entity.Description,
                    DeveloperId = entity.DeveloperId,
                    Id = entity.Id,
                    IsEnabled = entity.IsEnabled,
                    IsManaged = entity.IsManaged,
                    SortCode = entity.SortCode
                };
                input.IsManaged = true;
                AcDomain.Handle(input.ToCommand(AcSession));
            }
            return this.JsonResult(new ResponseData { id = id, success = true });
        }

        [By("xuexs")]
        [Description("取消托管")]
        [Guid("42E014A5-AAFB-4F4D-B0E1-21762A6F4201")]
        public ActionResult UnManage(string id)
        {
            string[] ids = id.Split(',');
            var idArray = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    idArray[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的功能标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                var entity = GetRequiredService<IRepository<Function>>().GetByKey(item);
                var input = new FunctionUpdateInput
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    SortCode = entity.SortCode,
                    IsManaged = entity.IsManaged,
                    IsEnabled = entity.IsEnabled,
                    DeveloperId = entity.DeveloperId,
                    Description = entity.Description
                };
                input.IsManaged = false;
                AcDomain.Handle(input.ToCommand(AcSession));
            }
            return this.JsonResult(new ResponseData { id = id, success = true });
        }

        [By("xuexs")]
        [Description("获取给定应用系统给定类型资源的托管功能")]
        [Guid("9675FCEE-1E6A-4DA7-9E93-48B5D0775D59")]
        public ActionResult GetManagedFunctions(Guid? appSystemId, string viewController)
        {
            CatalogState resource;
            if (!AcDomain.CatalogSet.TryGetCatalog(viewController, out resource))
            {
                throw new ValidationException("意外的资源码" + viewController);
            }
            IEnumerable<FunctionTr> data = null;
            if (appSystemId.HasValue && !string.IsNullOrEmpty(viewController))
            {
                data = AcDomain.FunctionSet.Where(a => a.AppSystem.Id == appSystemId.Value).Select(FunctionTr.Create).Where(a => a.IsManaged && a.ResourceTypeId == resource.Id);
            }
            else
            {
                data = new List<FunctionTr>();
            }

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("根据角色ID分页获取权限")]
        [Guid("98FA5133-5E6D-4CA1-96BB-CA3409615D38")]
        public ActionResult GetPlistPrivilegeByRoleId(GetPlistFunctionByRoleId requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistPrivilegeByRoleId(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<RoleAssignFunctionTr> { total = requestData.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("托管或取消托管功能")]
        [HttpPost]
        [Guid("F43DFDAE-F4C7-4177-9DBA-C8B4C54072FE")]
        public ActionResult ManageOrUnManageFunction()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改功能
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    bool isManaged = bool.Parse(row["IsManaged"].ToString());
                    var entity = GetRequiredService<IRepository<Function>>().GetByKey(id);
                    if (entity != null)
                    {
                        var input = new FunctionUpdateInput
                        {
                            Description = entity.Description,
                            DeveloperId = entity.DeveloperId,
                            IsEnabled = entity.IsEnabled,
                            IsManaged = entity.IsManaged,
                            SortCode = entity.SortCode,
                            Code = entity.Code,
                            Id = entity.Id
                        };
                        input.IsManaged = isManaged;
                        AcDomain.Handle(input.ToCommand(AcSession));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
    }
}
