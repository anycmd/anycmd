
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
    /// 信息字典项模型视图控制器<see cref="InfoDicItem"/>
    /// </summary>
    [Guid("2A37104A-2D8C-48CE-A766-5E271ED051FF")]
    public class InfoDicItemController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 信息字典项管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息字典项管理")]
        [Guid("7A459C71-5BAB-477C-9DDB-900FBE9F2B9F")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 信息字典项详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息字典项详细信息")]
        [Guid("890C1C84-3CC2-41DF-B126-0B03C4C556B3")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new InfoDicItemInfo(AcDomain, base.EntityType.GetData(id));
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
        /// 根据ID获取信息字典项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息字典项")]
        [Guid("49826114-B97B-4F4A-96D8-5D7532FCAADC")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<InfoDicItem>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取信息字典项详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息字典项详细信息")]
        [Guid("940CCA8B-5705-45AF-9411-D1A2FFB5B587")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new InfoDicItemInfo(AcDomain, base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 根据字典主键获取字典项列表
        /// </summary>
        /// <param name="dicId"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据字典主键获取字典项列表")]
        [Guid("B159CB91-6CFB-45C2-9871-03B16A0D429D")]
        public ActionResult GetDicItemsByDicId(Guid dicId)
        {
            InfoDicState infoDic;
            if (!AcDomain.NodeHost.InfoDics.TryGetInfoDic(dicId, out infoDic))
            {
                return this.JsonResult(null);
            }
            var data = AcDomain.NodeHost.InfoDics.GetInfoDicItems(infoDic).Select(d => new { code = d.Code, name = d.Name });

            return this.JsonResult(data);
        }

        /// <summary>
        /// 分页查询字典项
        /// </summary>
        /// <param name="dicId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页查询字典项")]
        [Guid("CE3CC36A-5D56-4B58-819C-2B8C1E1E79F5")]
        public ActionResult GetPlistInfoDicItems(Guid? dicId, GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            if (!dicId.HasValue)
            {
                throw new ValidationException("infoDicID参数是必须的");
            }
            InfoDicState infoDic;
            if (!AcDomain.NodeHost.InfoDics.TryGetInfoDic(dicId.Value, out infoDic))
            {
                throw new ValidationException("意外的信息字典标识" + dicId);
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "InfoDicItem"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.InfoDicItem");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的InfoDicItem实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = AcDomain.NodeHost.InfoDics.GetInfoDicItems(infoDic).Select(InfoDicItemTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<InfoDicItemTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加字典项")]
        [Guid("54156326-6144-4946-834B-1E9FC00318A6")]
        [HttpPost]
        public ActionResult Create(InfoDicItemCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新字典项")]
        [HttpPost]
        [Guid("FCD018F1-F656-4545-924A-70E604081BAA")]
        public ActionResult Update(InfoDicItemUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 删除字典项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除字典项")]
        [HttpPost]
        [Guid("131E3A46-EFA8-4817-A6AE-B470518E3F97")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveInfoDicItem, AcSession, id, ',');
        }
    }
}
