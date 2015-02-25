
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Exceptions;
    using MiniUI;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModels;
    using ViewModels.AccountViewModels;

    /// <summary>
    /// 系统来访日志模型视图控制器
    /// </summary>
    [Guid("5ACEBA3A-DA76-4F80-8C1C-2BDC19BE8AD3")]
    public class VisitingLogController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("来访日志")]
        [Guid("5162E5E0-CCB4-4DDA-9132-F965911371F1")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("来访日志详细信息")]
        [Guid("8B3BD2DD-7CED-4A08-9551-FF45EA18D683")]
        public ViewResultBase Details()
        {
            return this.DetailsResult(GetRequiredService<IVisitingLogQuery>(), "VisitingLogInfo");
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取来访日志详细信息")]
        [Guid("EB8DB4E5-FB0F-4FF4-AB9C-F0D867DED9A5")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IVisitingLogQuery>().Get("VisitingLogInfo", id.Value));
        }

        [By("xuexs")]
        [Description("分页获取来访日志")]
        [Guid("DFBAA579-48D1-4B0D-8C91-CE511F4CF8F5")]
        public ActionResult GetPlistVisitingLogs(GetPlistVisitingLogs requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var visitingLogs = GetRequiredService<IVisitingLogQuery>().GetPlistVisitingLogTrs(
                requestData.Key, requestData.LeftVisitOn, requestData.RightVisitOn, requestData);
            Debug.Assert(requestData.Total != null, "requestData.total != null");
            ViewModelHelper.FillVisitingLog(visitingLogs);
            var data = new MiniGrid<DicReader> { total = requestData.Total.Value, data = visitingLogs };

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("分页获取我的来访日志")]
        [Guid("BDA1C0A1-C1C4-41DE-A775-134B19093D10")]
        public ActionResult GetPlistMyVisitingLogs(GetPlistMyVisitingLogs requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            if (!AcSession.Identity.IsAuthenticated)
            {
                return this.JsonResult(new MiniGrid<Dictionary<string, object>> { total = 0, data = new List<Dictionary<string, object>> { } });
            }
            var visitingLogs = GetRequiredService<IVisitingLogQuery>().GetPlistVisitingLogTrs(
                AcSession.Account.Id, AcSession.Identity.Name, requestData.LeftVisitOn, requestData.RightVisitOn
                , requestData);
            Debug.Assert(requestData.Total != null, "requestData.total != null");
            ViewModelHelper.FillVisitingLog(visitingLogs);
            var data = new MiniGrid<DicReader> { total = requestData.Total.Value, data = visitingLogs };

            return this.JsonResult(data);
        }
    }
}
