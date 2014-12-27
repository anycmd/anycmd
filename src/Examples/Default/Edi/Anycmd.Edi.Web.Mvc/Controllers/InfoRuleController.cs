
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Transactions;
    using Util;
    using ViewModel;
    using ViewModels.InfoConstraintViewModels;

    /// <summary>
    /// 信息验证器模型视图控制器<see cref="InfoRule"/>
    /// </summary>
    [Guid("3644F0EF-4AC5-497C-AD37-76E8DA1932E2")]
    public class InfoRuleController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 信息项验证器主页
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息项验证器主页")]
        [Guid("68C4693B-A65B-4C68-A7A5-7F08EF53B24F")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 信息项验证器详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息项验证器详细信息")]
        [Guid("57B7D226-660B-42CD-B45B-1A5E31F0A931")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new InfoRuleInfo(base.EntityType.GetData(id));
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

        /// <summary>
        /// 信息项验证器
        /// </summary>
        /// <param name="isInner"></param>
        /// <param name="elementId"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息规则(Rule)")]
        [Guid("B3F6AD68-C14D-4AF2-BA8B-15160B968CDD")]
        public ViewResultBase ElementInfoRules(string isInner, Guid? elementId)
        {
            if (!string.IsNullOrEmpty(isInner))
            {
                return PartialView("Partials/ElementInfoRules");
            }
            return View();
        }

        #endregion

        /// <summary>
        /// 根据ID获取信息项验证器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息项验证器")]
        [Guid("26B17379-4C31-4107-B788-E960EDCA4539")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<InfoRule>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取信息项验证器详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取信息项验证器详细信息")]
        [Guid("272EC356-490D-4209-A801-302C068E83B6")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new InfoRuleInfo(base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取信息项验证器
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取信息项验证器")]
        [Guid("163683A8-10D7-44D5-8D7F-B9EDEEDF1014")]
        public ActionResult GetPlistInfoRules(GetPlistInfoRules requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState infoRuleEntityType;
            if (!Host.EntityTypeSet.TryGetEntityType(new Coder("Edi", "InfoRule"), out infoRuleEntityType))
            {
                throw new AnycmdException("意外的实体类型");
            }
            foreach (var filter in requestData.Filters)
            {
                PropertyState property;
                if (!infoRuleEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的AppSystem实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = Host.NodeHost.InfoRules.Select(a => InfoRuleTr.Create(Host, a)).AsQueryable();
            foreach (var filter in requestData.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            requestData.Total = queryable.Count();
            var data = queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<InfoRuleTr> { total = requestData.Total.Value, data = data });
        }

        /// <summary>
        /// 分页获取元素信息项验证器
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取元素信息项验证器")]
        [Guid("B2D06914-BE8B-406C-A6E7-2909DD5187BE")]
        public ActionResult GetPlistElementInfoRules(GetPlistElementInfoRules requestModel)
        {
            ElementDescriptor element;
            if (!Host.NodeHost.Ontologies.TryGetElement(requestModel.ElementId, out element))
            {
                throw new ValidationException("意外的本体元素标识" + requestModel.ElementId);
            }
            var list = new List<ElementInfoRuleTr>();
            foreach (var item in element.Element.ElementInfoRules)
            {
                InfoRuleState infoRule;
                if (Host.NodeHost.InfoRules.TryGetInfoRule(item.InfoRuleId, out infoRule))
                {
                    list.Add(new ElementInfoRuleTr
                    {
                        InfoRuleId = infoRule.Id,
                        AuthorCode = infoRule.InfoRule.Author,
                        CreateOn = item.CreateOn,
                        ElementId = element.Element.Id,
                        FullName = infoRule.GetType().Name,
                        Id = item.Id,
                        Name = infoRule.InfoRule.Name,
                        Title = infoRule.InfoRule.Title,
                        SortCode = item.SortCode,
                        IsEnabled = item.IsEnabled
                    });
                }
            }
            var data = new MiniGrid<ElementInfoRuleTr> { total = list.Count, data = list };

            return this.JsonResult(data);
        }

        /// <summary>
        /// 更新信息项验证器
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新信息项验证器")]
        [HttpPost]
        [Guid("23EB29EB-DD1D-4130-914D-EE3CC5BA6D64")]
        public ActionResult Update(InfoRuleInput requestModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            var responseResult = new ResponseData { success = false };
            using (var coordinator = TransactionCoordinatorFactory.Create(GetRequiredService<IRepository<InfoRule>>().Context))
            {
                var entity = GetRequiredService<IRepository<InfoRule>>().GetByKey(requestModel.Id);
                entity.IsEnabled = requestModel.IsEnabled;
                GetRequiredService<IRepository<InfoRule>>().Update(entity);
                responseResult.id = entity.Id.ToString();
                responseResult.success = true;

                coordinator.Commit();
            }

            return this.JsonResult(responseResult);
        }
    }
}
