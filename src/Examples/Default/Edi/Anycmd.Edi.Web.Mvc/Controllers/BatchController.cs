
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Host.Edi.Entities;
    using Engine.Rdb;
    using Exceptions;
    using MiniUI;
    using Query;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.BatchViewModels;

    /// <summary>
    /// 批模型视图控制器
    /// </summary>
    [Guid("D017A61B-AB19-44A5-B144-103DA5DBB757")]
    public class BatchController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 批主页
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("批主页")]
        [Guid("607355AE-8853-4242-961C-A8786E18399A")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 批详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("批详细信息")]
        [Guid("3D251CA4-F429-4C2C-8A23-2058F3B1933B")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new BatchInfo(base.EntityType.GetData(id));
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
        /// 根据ID获取批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取批")]
        [Guid("FE3B03A9-A382-4EF3-A8E5-0E780CE64115")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Batch>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取批详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取批详细信息")]
        [Guid("785B8559-2368-4590-BD79-6985DD7D7F11")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new BatchInfo(base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取批")]
        [Guid("35AD8138-8A99-4DCE-815C-4D748B4098ED")]
        public ActionResult GetPlistBatches(GetPlistBatchs input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Guid? ontologyId = null;
            if (string.IsNullOrEmpty(input.OntologyCode))
            {
                ontologyId = null;
            }
            else
            {
                OntologyDescriptor ontology;
                if (!AcDomain.NodeHost.Ontologies.TryGetOntology(input.OntologyCode, out ontology))
                {
                    throw new ValidationException("意外的本体码" + input.OntologyCode);
                }
                ontologyId = ontology.Ontology.Id;
            }
            var pagingData = new PagingInput(input.PageIndex
                , input.PageSize, input.SortField, input.SortOrder);
            if (ontologyId != null)
            {
                input.Filters.Insert(0, FilterData.EQ("OntologyId", ontologyId.Value));
            }
            var data = GetRequiredService<IBatchQuery>().GetPlist(base.EntityType, () =>
            {
                RdbDescriptor rdb;
                if (!AcDomain.Rdbs.TryDb(base.EntityType.DatabaseId, out rdb))
                {
                    throw new AnycmdException("意外配置的Batch实体类型对象数据库标识" + base.EntityType.DatabaseId);
                }
                List<DbParameter> ps;
                var filterString = new SqlFilterStringBuilder().FilterString(rdb, input.Filters, "a", out ps);
                if (!string.IsNullOrEmpty(filterString))
                {
                    filterString = " where " + filterString;
                }
                return new SqlFilter(filterString, ps.ToArray());
            }, pagingData);

            Debug.Assert(pagingData.Total != null, "pagingData.Total != null");
            return this.JsonResult(new MiniGrid<BatchTr> { total = pagingData.Total.Value, data = data.Select(a => new BatchTr(a)) });
        }

        /// <summary>
        /// 添加批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加批")]
        [HttpPost]
        [Guid("1CF94DC6-A574-4073-9009-9234C76FA224")]
        public ActionResult Create(BatchCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 修改批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("修改批")]
        [HttpPost]
        [Guid("931B022C-72D5-408A-9EDF-3021A987F7ED")]
        public ActionResult Update(BatchUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 删除批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除批")]
        [HttpPost]
        [Guid("EF7E5E62-E873-4AD8-8AD7-83D77307F37B")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveBatch, AcSession, id, ',');
        }
    }
}
