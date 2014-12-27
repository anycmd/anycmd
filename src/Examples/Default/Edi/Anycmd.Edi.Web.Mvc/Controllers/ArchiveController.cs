
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
    using ViewModels.ArchiveViewModels;

    /// <summary>
    /// 归档模型视图控制器
    /// </summary>
    [Guid("D2DFC1E1-4F7E-44AA-B361-3750EA988385")]
    public class ArchiveController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 归档主页
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("归档主页")]
        [Guid("4E0B5375-D54A-488E-BAC1-19F3DC5190B4")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 归档详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("归档详细信息")]
        [Guid("4A5F6607-D6C9-4CAD-BF02-1A67F985695F")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new ArchiveInfo(base.EntityType.GetData(id));
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
        /// 根据ID获取归档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取归档")]
        [Guid("70B67396-F530-4C6E-BE46-D156DEC3DD2D")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Archive>>().GetByKey(id.Value));
        }
        #endregion

        /// <summary>
        /// 根据ID获取归档详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取归档详细信息")]
        [Guid("CE8A907B-F699-426E-8FAD-71A86C527B19")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new ArchiveInfo(base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取归档
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取归档")]
        [Guid("825A4D92-D879-433A-A28C-8A7A0FFBD196")]
        public ActionResult GetPlistArchives(GetPlistArchives input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            OntologyDescriptor ontology;
            if (!Host.NodeHost.Ontologies.TryGetOntology(input.OntologyCode, out ontology))
            {
                throw new ValidationException("意外的本体码" + input.OntologyCode);
            }
            EntityTypeState entityType;
            if (!Host.EntityTypeSet.TryGetEntityType(new Coder("Edi", "Archive"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.Archive");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!Host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Archive实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = ontology.Archives.Select(ArchiveTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }

            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<ArchiveTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加归档
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加归档")]
        [HttpPost]
        [Guid("7622943F-D70D-434C-ACAD-35184D9B17A9")]
        public ActionResult Create(ArchiveCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.AddArchive(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 修改归档
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("修改归档")]
        [HttpPost]
        [Guid("3EED07C0-EB3C-45C1-9933-999408C9CD03")]
        public ActionResult Update(ArchiveUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.UpdateArchive(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 删除归档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除归档")]
        [HttpPost]
        [Guid("E0AEC253-B9E8-4E23-8AC7-F97BFE09DB90")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(Host.RemoveArchive, id, ',');
        }
    }
}
