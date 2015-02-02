
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Ac;
    using Exceptions;
    using MiniUI;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.Infra.DicViewModels;

    /// <summary>
    /// 系统字典项模型视图控制器
    /// </summary>
    [Guid("DC8EBCE9-FB1D-47FB-B006-0F09A8CF5C23")]
    public class DicItemController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("字典管理")]
        [Guid("E319B692-2C8A-4232-8D09-B981F9A89BD1")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("字典详细信息")]
        [Guid("51462B62-2014-4388-BB0E-4894318C840D")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = DicItemInfo.Create(base.EntityType.GetData(id));
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
        [Description("根据ID获取字典项")]
        [Guid("9102B794-71CF-463E-8D66-8A683B0C69C3")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取字典项详细信息")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(DicItemInfo.Create(base.EntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("分页获取字典项")]
        [Guid("BB0EBC5D-E3E1-4265-90B0-52831FA430D6")]
        public ActionResult GetPlistDicItems(GetPlistDicItems requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistDicItems(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DicItemTr> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加字典项")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("90EF1FCB-CBBE-4CD7-8323-CB318EAA3805")]
        public ActionResult Create(DicItemCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新字典项")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("E77D3D5F-18DC-4564-A445-3EFF13BC200B")]
        public ActionResult Update(DicItemUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除字典项")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("6B2BD7D5-98C7-4AE6-BA43-A51BEB00C218")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveDicItem, AcSession, id, ',');
        }
    }
}
