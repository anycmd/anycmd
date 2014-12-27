
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Infra;
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
    using ViewModels.Infra.DicViewModels;

    /// <summary>
    /// 系统字典模型视图控制器
    /// </summary>
    [Guid("C21DA400-A548-4C99-8A1D-63B82A9475DD")]
    public class DicController : AnycmdController
    {
        #region views
        [By("xuexs")]
        [Description("字典列表")]
        [Guid("347765F7-7CF6-4C7D-80EA-AF3FFA7BE648")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("系统字典管理")]
        [Guid("9359BDE3-162E-4E79-92AD-72826381C711")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = DicInfo.Create(base.EntityType.GetData(id));
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
        [Description("根据ID获取字典")]
        [Guid("46E1E2CD-9A20-47A7-A8EA-8698E9C5249A")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Dic>>().GetByKey(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取字典详细信息")]
        [Guid("67655DED-9FF8-417F-A2EE-B630D98480A2")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(DicInfo.Create(base.EntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("分页获取字典")]
        [Guid("C94359C7-ADB3-45FD-96A4-D4DD2AF1226A")]
        public ActionResult GetPlistDics(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistDics(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<DicTr> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("删除字典")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("BDDE912C-A2BD-40C1-BB7F-97A4A4720982")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(Host.RemoveDic, id, ',');
        }
    }
}
