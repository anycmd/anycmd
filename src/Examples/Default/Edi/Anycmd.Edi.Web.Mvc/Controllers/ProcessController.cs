
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Host.Edi.Entities;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.ProcessViewModels;

    /// <summary>
    /// 进程模型视图控制器<see cref="Process"/>
    /// </summary>
    [Guid("0816A013-7349-4421-9766-64464391ABEC")]
    public class ProcessController : AnycmdController
    {
        private static readonly EntityTypeState ProcessEntityType;

        static ProcessController()
        {
            if (!Host.EntityTypeSet.TryGetEntityType("Edi", "Process", out ProcessEntityType))
            {
                throw new CoreException("意外的实体类型");
            }
        }

        #region ViewResults
        /// <summary>
        /// 进程主页
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("进程主页")]
        [Guid("AA050B92-C7BB-40EF-842E-3671D08007C7")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 进程详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("进程详细信息")]
        [Guid("0E4272DD-EF91-422F-B342-26FC2F1DA2A9")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new ProcessInfo(ProcessEntityType.GetData(id));
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
        /// 根据ID获取进程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取进程")]
        [Guid("EE3330EB-9F52-4FF2-9BE5-63ED4283831A")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Process>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取进程详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取进程详细信息")]
        [Guid("0363FCD3-271D-4274-BDEB-F4505769718F")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new ProcessInfo(ProcessEntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取进程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取进程")]
        [Guid("3EB554F9-A7FF-4951-B511-EFE965BD92DC")]
        public ActionResult GetPlistProcesses(GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState entityType;
            if (!Host.EntityTypeSet.TryGetEntityType("Edi", "Process", out entityType))
            {
                throw new CoreException("意外的实体类型Edi.Process");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!Host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Process实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = Host.NodeHost.Processs.Select(ProcessTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<ProcessTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加进程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加进程")]
        [HttpPost]
        [Guid("04ABA1F2-A1E7-4E60-A835-799B4496FBA3")]
        public ActionResult Create(ProcessCreateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            Host.AddProcess(input);

            return this.JsonResult(new ResponseData { success = true, id = input.Id.Value });
        }

        /// <summary>
        /// 更新进程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新进程")]
        [HttpPost]
        [Guid("D78E9AFE-FDCB-42B4-A7B2-3C0969BEEE1B")]
        public ActionResult Update(ProcessUpdateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            Host.UpdateProcess(input);

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }
    }
}
