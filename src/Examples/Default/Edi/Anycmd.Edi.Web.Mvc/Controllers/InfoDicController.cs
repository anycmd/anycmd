
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
    using ViewModels.InfoDicViewModels;

    /// <summary>
    /// 信息字典模型视图控制器<see cref="InfoDic"/>
    /// </summary>
    [Guid("11BF3409-6F00-4897-A6F3-DEDC34511966")]
    public class InfoDicController : AnycmdController
    {
        private static readonly EntityTypeState InfoDicEntityType;

        static InfoDicController()
        {
            if (!Host.EntityTypeSet.TryGetEntityType("Edi", "InfoDic", out InfoDicEntityType))
            {
                throw new CoreException("意外的实体类型");
            }
        }

        #region ViewResults
        /// <summary>
        /// 信息字典管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息字典管理")]
        [Guid("2E2033C7-7E71-4605-A937-BE248429B976")]
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
        [Guid("32BA4310-0FB1-4677-A7C5-6AF14AC036BA")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new InfoDicInfo(Host, InfoDicEntityType.GetData(id));
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
        [Guid("E5415968-480D-444A-8FD0-C21FA934168F")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<InfoDic>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取信息字典详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息字典详细信息")]
        [Guid("78859F4D-F813-46CC-8040-F70781A5DC7B")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new InfoDicInfo(Host, InfoDicEntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取信息字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取信息字典")]
        [Guid("8E3EB5C7-427C-47DF-A893-7C4D86BA6314")]
        public ActionResult GetPlistInfoDics(GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState entityType;
            if (!Host.EntityTypeSet.TryGetEntityType("Edi", "InfoDic", out entityType))
            {
                throw new CoreException("意外的实体类型Edi.InfoDic");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!Host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的InfoDic实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = Host.NodeHost.InfoDics.Select(InfoDicTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<InfoDicTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加信息字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加信息字典")]
        [HttpPost]
        [Guid("574EAD72-89D1-4274-A5E8-265A04047A60")]
        public ActionResult Create(InfoDicCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.AddInfoDic(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新信息字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新信息字典")]
        [HttpPost]
        [Guid("E220DB69-161C-47BE-A58A-5E0E008D6064")]
        public ActionResult Update(InfoDicUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.UpdateInfoDic(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 删除信息字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除信息字典")]
        [HttpPost]
        [Guid("3A20A881-9EA6-4911-9F39-55EC60B7FFF7")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(Host.RemoveInfoDic, id, ',');
        }
    }
}
