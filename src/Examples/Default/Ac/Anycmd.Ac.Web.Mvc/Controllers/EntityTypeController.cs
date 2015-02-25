
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Host.Ac;
    using Exceptions;
    using MiniUI;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.EntityTypeViewModels;

    /// <summary>
    /// 系统模型模型视图控制器
    /// </summary>
    [Guid("D7560945-C153-4DFB-95A4-DC343534B8A6")]
    public class EntityTypeController : AnycmdController
    {
        #region ViewPages

        [By("xuexs")]
        [Description("实体类型管理")]
        [Guid("991E71E7-9DB0-4F50-BF49-2C79350DFCE6")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("实体类型详细信息")]
        [Guid("E266D470-75F0-421A-84C2-86DD605B249B")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new EntityTypeInfo(base.EntityType.GetData(id));
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

        [By("xuexs")]
        [Description("根据ID获取实体类型")]
        [Guid("04A22377-9335-4890-9F0E-B228FD588947")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取实体类型详细信息")]
        [Guid("749C08B4-C4A1-4BC0-A3BE-6C9D09EB1188")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new EntityTypeInfo(base.EntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("分页获取实体类型")]
        [Guid("B910C434-04A8-415A-9271-788A770C8679")]
        public ActionResult GetPlistEntityTypes(GetPlistResult requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistEntityTypes(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<EntityTypeTr> { total = requestData.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加实体类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("4DA8200C-B9D4-42C7-9149-CA7A0209E01D")]
        public ActionResult Create(EntityTypeCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新实体类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("54C4E3C2-B972-44FF-B6B9-76E8543FC023")]
        public ActionResult Update(EntityTypeUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除实体类型")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("B6FAD137-535C-4668-A1BB-C7281455E719")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveEntityType, AcSession, id, ',');
        }
    }
}
