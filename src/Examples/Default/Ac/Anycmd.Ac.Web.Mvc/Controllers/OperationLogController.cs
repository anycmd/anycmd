
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Logging;
    using MiniUI;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModels.LogViewModels;

    /// <summary>
    /// 系统操作日志模型视图控制器<see cref="Logging.OperationLogBase"/>
    /// </summary>
    [Guid("4F7570D2-4EFC-4707-8277-D75D8F899EA2")]
    public class OperationLogController : AnycmdController
    {
        #region ViewPage

        [By("xuexs")]
        [Description("操作日志主页")]
        [Guid("399CA5DF-CE79-409D-827E-38B29E5698FB")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("操作日志")]
        [Guid("D5D33A37-2B66-40AE-8367-7D0E512C5B90")]
        public ViewResultBase OperationLogs()
        {
            return ViewResult();
        }
        #endregion

        [By("xuexs")]
        [Description("分页获取操作日志")]
        [Guid("D52DB082-E3EA-47AE-9C5F-71897FC813CB")]
        public ActionResult GetPlistOperationLogs(GetPlistOperationLogs requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var operationlogs = GetRequiredService<ILoggingService>().GetPlistOperationLogs(
                requestData.TargetId,
                requestData.LeftCreateOn,
                requestData.RightCreateOn,
                requestData.Filters,
                requestData);
            Debug.Assert(requestData.Total != null, "requestData.total != null");
            var data = new MiniGrid<OperationLog> { data = operationlogs, total = requestData.Total.Value };

            return this.JsonResult(data);
        }
    }
}
