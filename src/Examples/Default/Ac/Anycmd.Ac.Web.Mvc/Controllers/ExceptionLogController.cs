
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Exceptions;
    using Logging;
    using MiniUI;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.LogViewModels;

    /// <summary>
    /// 系统异常模型视图控制器
    /// </summary>
    [DeveloperFilter(Order = 21)]
    [Guid("48205317-9CFC-40B5-95F2-227EA2E9DDE3")]
    public class ExceptionLogController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("系统异常管理")]
        [Guid("DE3CBE31-B4F5-4912-832A-309BD84C9DEB")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("系统异常详细信息")]
        [Guid("D438F8F3-D55F-4166-A6A1-B23F21E25E81")]
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
        [Description("根据ID获取系统异常详细信息")]
        [Guid("BF9ED3C6-4021-4493-8892-565692EA247C")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("分页获取系统异常")]
        [Guid("53C81FA6-AFA7-47FC-A82C-849DE2A5B6DC")]
        public ActionResult GetPlistExceptionLogs(GetPlistExceptionLogs requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var exceptionlogs = GetRequiredService<ILoggingService>().GetPlistExceptionLogs(requestData.Filters, requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            var data = new MiniGrid<ExceptionLog> { total = requestData.Total.Value, data = exceptionlogs };

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("清空系统异常")]
        [HttpPost]
        [Guid("5C6262A3-BD78-4510-9A06-0011D70AFA55")]
        public ActionResult ClearExceptionLog()
        {
            var responseResult = new ResponseData { success = false };
            GetRequiredService<ILoggingService>().ClearExceptionLog();
            responseResult.success = true;

            return this.JsonResult(responseResult);
        }
    }
}
