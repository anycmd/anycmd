
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac.Rbac;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.DsdViewModels;

    [Guid("2BB502B3-EEE9-43A8-A24B-32A5ED5CA4D8")]
    public class DsdSetController : AnycmdController
    {
        [By("xuexs")]
        [Description("动态职责分离角色集")]
        [Guid("C3C896E9-9F5E-4470-8795-688AF4F09F70")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("动态职责分离角色集详细信息")]
        [Guid("B85ADE5B-A32C-41A8-9702-F62CA7366978")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = base.EntityType.GetData(id);
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

        [By("xuexs")]
        [Description("动态职责分离角色集角色列表")]
        [Guid("620BB57C-05C8-48A8-B47A-36A992F0736B")]
        public ViewResultBase Roles()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("根据ID获取动态职责分离角色集")]
        [Guid("5D705B9C-7853-4D84-9C4D-DD987A4EB9FE")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取动态职责分离角色集详细信息")]
        [Guid("78451402-95A5-4360-8306-2A15F93D76E7")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("分页获取动动态职责分离角色集列表")]
        [Guid("108B6AB3-E411-4634-829E-CFB517CEE724")]
        public ActionResult GetPlistDsdSets(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistDsdSets(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid { total = requestModel.Total.Value, data = data.Select(a => a.ToTableRowData()) });
        }

        #region AddOrDeleteRoleMembers
        [By("xuexs")]
        [Description("加入或删除角色")]
        [HttpPost]
        [Guid("5BAA01D1-5CF8-4FD9-90F5-883BC9B488D5")]
        public ActionResult AddOrDeleteRoleMembers()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    var entity = GetRequiredService<IRepository<DsdRole>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.Handle(new RemoveDsdRoleCommand(UserSession, id));
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new DsdRoleCreateIo
                        {
                            Id = new Guid(row["Id"].ToString()),
                            RoleId = new Guid(row["RoleId"].ToString()),
                            DsdSetId = new Guid(row["DsdSetId"].ToString())
                        };
                        AcDomain.Handle(createInput.ToCommand(UserSession));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        [By("xuexs")]
        [Description("创建动态职责分离角色集")]
        [HttpPost]
        [Guid("AB894D3F-0E69-4145-BD8C-4344C4B45153")]
        public ActionResult Create(DsdSetCreateIo input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(UserSession));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新动态职责分离角色集")]
        [HttpPost]
        [Guid("471447CA-97E1-4704-BD9D-5605D3259C69")]
        public ActionResult Update(DsdSetUpdateIo input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(UserSession));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("删除动态职责分离角色集")]
        [HttpPost]
        [Guid("51D0DF6E-5D23-448D-A536-7C7C8F780FB0")]
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
                    throw new ValidationException("意外的动态职责分离角色集标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemoveDsdSetCommand(UserSession, item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
