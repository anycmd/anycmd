
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using MiniUI;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.AppSystemViewModels;

    /// <summary>
    /// 应用系统模型视图控制器<see cref="AppSystem"/>
    /// </summary>
    [Guid("07047DDC-B076-4440-95D1-08A66D2AB676")]
    public class AppSystemController : AnycmdController
    {
        #region 视图

        [By("xuexs")]
        [Guid("282B3CF0-9E33-482F-AF06-C4DBAE09E785")]
        [Description("权限应用系统管理")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("应用系统详细信息")]
        [Guid("19885B55-1785-47C9-BA9B-9263BD300231")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = AppSystemInfo.Create(base.EntityType.GetData(id));
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
        [Description("根据应用系统主键获取应用系统")]
        [Guid("17DA2D89-A0BA-4CA3-85FC-F87ED6248A64")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据应用系统主键获取应用系统相信信息")]
        [Guid("E3D35A89-6F1D-4249-9BFE-D0635D4849AA")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(AppSystemInfo.Create(base.EntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("分页查询应用系统")]
        [Guid("15CE0807-AB9B-4E9F-B5FA-A805D6822911")]
        public ActionResult GetPlistAppSystems(GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistAppSystems(input);

            Debug.Assert(input.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<AppSystemTr> { total = input.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加应用系统")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("BE3BE661-E52C-4A55-8B0C-E9B2F72BCD24")]
        public ActionResult Create(AppSystemCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新应用系统")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("04E9EEDD-819F-40E2-B5FD-1DA9033DE294")]
        public ActionResult Update(AppSystemUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除应用系统")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("B14960FD-319D-4B92-AE3D-A10455A10C3A")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveAppSystem, AcSession, id, ',');
        }
    }
}
