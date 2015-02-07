
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Edi.Entities;
    using Exceptions;
    using MiniUI;
    using Query;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.PluginViewModels;

    /// <summary>
    /// 插件模型视图控制器<see cref="Plugin"/>
    /// </summary>
    [Guid("A678091F-6C8E-4575-A380-2197837B8971")]
    public class PluginController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 插件主页
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("插件主页")]
        [Guid("FBB74A31-4874-43FE-8A54-CB07234C79C7")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 插件详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("插件详细信息")]
        [Guid("1C1A9F3C-EA3A-4AF7-8516-C83C9DDE275D")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new PluginInfo(AcDomain, base.EntityType.GetData(id));
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
        /// 根据ID获取命令插件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取命令插件")]
        [Guid("6BA69D91-03F8-40BE-9A05-C7854583B817")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Plugin>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取命令插件详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取命令插件详细信息")]
        [Guid("B1B9F677-3206-469A-A293-AA133D6CADAA")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new PluginInfo(AcDomain, base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取命令插件
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取命令插件")]
        [Guid("07A892B9-3F63-4699-959F-D3127CF64228")]
        public ActionResult GetPlistPlugins(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var dataDics = GetRequiredService<IPluginQuery>().GetPlist("Plugin", () =>
            {
                List<SqlParameter> ps;
                var filterString = new SqlFilterStringBuilder().FilterString(requestModel.Filters, "a", out ps);
                if (!string.IsNullOrEmpty(filterString))
                {
                    filterString = " where " + filterString;
                }
                return new SqlFilter(filterString, ps.ToArray());
            }, requestModel);
            Debug.Assert(requestModel.Total != null, "requestModel.Total != null");
            var data = new MiniGrid<Dictionary<string, object>> { total = requestModel.Total.Value, data = dataDics };

            return this.JsonResult(data);
        }
    }
}
