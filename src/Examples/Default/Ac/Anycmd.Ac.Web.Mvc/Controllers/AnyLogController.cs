
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Exceptions;
    using Logging;
    using MiniUI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.LogViewModels;

    /// <summary>
    /// 任何日志模型视图控制器<see cref="AnyLog"/>
    /// </summary>
    [Guid("5BC0B717-B34F-4B9F-88A8-6DDC179EC34F")]
    public class AnyLogController : AnycmdController
    {
        [By("xuexs")]
        [Description("运行日志管理")]
        [Guid("3F1609FC-4C7D-4D55-80B7-6FDC31EA325D")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("运行日志详细信息")]
        [Guid("F036233D-20AD-4B63-8B3A-C2F59DF4CFBD")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    if (Guid.TryParse(Request["id"], out id))
                    {
                        var data = GetRequiredService<ILoggingService>().Get(id);
                        return new PartialViewResult { ViewName = "Partials/Details", ViewData = new ViewDataDictionary(data) };
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

        [By("xuexs")]
        [Description("根据ID获取运行日志详细信息")]
        [Guid("D47259B8-7838-4FC0-AFF2-8DEDD94DFDC6")]
        public ActionResult GetInfo(Guid? id)
        {
            IAnyLog data = null;
            if (id.HasValue)
            {
                data = GetRequiredService<ILoggingService>().Get(id.Value);
            }

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("清空运行日志")]
        [HttpPost]
        [Guid("67560F60-E228-418D-B477-C5EED408289B")]
        public ActionResult ClearAnyLog()
        {
            var responseResult = new ResponseData { success = false };
            GetRequiredService<ILoggingService>().ClearAnyLog();
            responseResult.success = true;

            return this.JsonResult(responseResult);
        }

        [By("xuexs")]
        [Description("分页获取运行日志")]
        [Guid("674B4B32-7477-43B3-9E23-A9C1A3E41E5C")]
        public ActionResult GetPlistAnyLogs(GetPlistAnyLogs requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            IList<IAnyLog> anyLogs = GetRequiredService<ILoggingService>().GetPlistAnyLogs(requestModel.Filters, requestModel);
            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            var data = new MiniGrid<IAnyLog> { total = requestModel.Total.Value, data = anyLogs };

            return this.JsonResult(data);
        }
    }
}
