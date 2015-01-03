
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using Engine.Host.Ac;
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

    [Guid("C297BB87-7A94-4E5B-81B1-BC440D145018")]
    public class SsdSetController : AnycmdController
    {
        [By("xuexs")]
        [Description("静态职责分离角色集")]
        [Guid("5103907C-E042-446B-ACB8-E63F183EEA46")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("静态职责分离角色集详细信息")]
        [Guid("4264490D-071D-49EE-B51F-604E976A985F")]
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
        [Description("静态职责分离角色集角色列表")]
        [Guid("9605DCAE-5AA8-4F38-9EFA-C3A018CDD3F0")]
        public ViewResultBase Roles()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("根据ID获取静态职责分离角色集")]
        [Guid("8E3271D2-3176-4CB1-BCB8-CFDC7F85A117")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取静态职责分离角色集详细信息")]
        [Guid("9F854FF5-42EE-44BD-9C78-006C7B4C0D15")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("分页获取静态职责分离角色集列表")]
        [Guid("EE7DB589-4211-47CA-B764-12C6ACE69CBE")]
        public ActionResult GetPlistSsdSets(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistSsdSets(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid { total = requestModel.Total.Value, data = data.Select(a => a.ToTableRowData()) });
        }

        #region AddOrDeleteRoleMembers
        [By("xuexs")]
        [Description("加入或删除角色")]
        [HttpPost]
        [Guid("6ACA1559-D527-4997-B06E-94E3E78E88DA")]
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
                    var entity = GetRequiredService<IRepository<SsdRole>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.Handle(new RemoveSsdRoleCommand(id));
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new SsdRoleCreateIo
                        {
                            Id = new Guid(row["Id"].ToString()),
                            RoleId = new Guid(row["RoleId"].ToString()),
                            SsdSetId = new Guid(row["SsdSetId"].ToString())
                        };
                        AcDomain.Handle(new AddSsdRoleCommand(createInput));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        [By("xuexs")]
        [Description("创建静态职责分离角色集")]
        [HttpPost]
        [Guid("3916A756-3933-4917-A17C-0857919B1122")]
        public ActionResult Create(SsdSetCreateIo input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(new AddSsdSetCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新静态职责分离角色集")]
        [HttpPost]
        [Guid("D7977127-C7D6-4977-9DBF-BBF1FFA49EA5")]
        public ActionResult Update(SsdSetUpdateIo input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(new UpdateSsdSetCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("删除静态职责分离角色集")]
        [HttpPost]
        [Guid("274B8ED2-1F6A-4D8B-963A-BB8DA7B71C74")]
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
                    throw new ValidationException("意外的静态职责分离角色集标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemoveSsdSetCommand(item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
