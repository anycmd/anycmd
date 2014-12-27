
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Ac.Infra;
    using Engine.Host.Ac.Infra.Messages;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.Infra.ResourceViewModels;

    /// <summary>
    /// 系统资源类型模型视图控制器
    /// </summary>
    [Guid("9338461A-CD1B-4A2E-A6D8-A3138C92C23C")]
    public class ResourceTypeController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("资源类型集")]
        [Guid("F313A334-2CCC-4D97-A9FB-896E151727DE")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("资源类型详细信息")]
        [Guid("6E26A732-182F-4D8D-B693-000A8AB95E60")]
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

        #endregion

        [By("xuexs")]
        [Description("根据ID获取资源类型")]
        [Guid("119629CC-6225-425C-8EB2-35A1F221E7E2")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<ResourceType>>().GetByKey(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取资源类型详细信息")]
        [Guid("D6D0BB13-4E2A-40B9-AC0D-1F1666D03FD8")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("分页获取资源类型")]
        [Guid("536D4FDA-0A1A-4B8F-8742-9C5D4DF8B49B")]
        public ActionResult GetPlistResources(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistResources(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<ResourceTypeTr> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("根据区域分页获取资源类型")]
        [Guid("7ED3B01B-FCD6-469F-AA68-CD4192286CFF")]
        public ActionResult GetPlistAppSystemResources(GetPlistAreaResourceTypes requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistAppSystemResources(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<ResourceTypeTr> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加资源类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("DC062A18-D07E-4FC8-9D44-F5248CD188CE")]
        public ActionResult Create(ResourceTypeCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.Handle(new AddResourceCommand(input));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新资源类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("5637BA9F-25B2-4876-BAEF-3E4DD8F8BF48")]
        public ActionResult Update(ResourceTypeUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.Handle(new UpdateResourceCommand(input));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除资源类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("A3A85F0C-AD62-41D3-84CC-18F6B60385E5")]
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
                    throw new ValidationException("意外的资源标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                Host.Handle(new RemoveResourceTypeCommand(item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
