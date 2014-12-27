
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Exceptions;
    using MiniUI;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Web.Mvc;
    using Util;
    using ViewModels.StateCodeViewModels;

    /// <summary>
    /// 数据交换状态码模型视图控制器
    /// </summary>
    [Guid("0A3CE8EF-7F36-4D00-9C76-5CA295C1A172")]
    public class StateCodeController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 信息字典管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息字典管理")]
        [Guid("3259817E-BCAC-41C8-92AF-A4A6E3C43EFB")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 信息字典详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息字典详细信息")]
        [Guid("4CEA7780-0282-4D2B-B488-D6F428E87A2A")]
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

        /// <summary>
        /// 根据ID获取信息字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息字典")]
        [Guid("07569C78-BB2E-459C-AC7C-B2F19EF53E12")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        /// <summary>
        /// 根据ID获取信息字典详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息字典详细信息")]
        [Guid("202502BC-2271-4336-8CF9-954A86E26DD7")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        /// <summary>
        /// 分页获取信息字典
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取信息字典")]
        [Guid("D713BD7C-EC5C-4647-872E-A3E06FD2C9F6")]
        public ActionResult GetPlistStateCodes(GetPlistStateCodes requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var dataDics = GetRequiredService<IStateCodeQuery>().GetPlist("StateCode", () =>
            {
                List<SqlParameter> ps;
                var filterString = new SqlFilterStringBuilder().FilterString(requestModel.Filters, "a", out ps);
                if (!string.IsNullOrEmpty(filterString))
                {
                    filterString = " where " + filterString;
                }
                return new SqlFilter(filterString, ps.ToArray());
            }, requestModel);
            var data = new MiniGrid<Dictionary<string, object>> { total = requestModel.Total.Value, data = dataDics };

            return this.JsonResult(data);
        }

        /// <summary>
        /// 更新状态码
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新状态码")]
        [Guid("160C2446-17DF-43CC-8BBE-44B8B1B64FB5")]
        [HttpPost]
        public ActionResult Update(StateCodeUpdateInput requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            throw new ValidationException("暂不支持修改");
        }
    }
}
