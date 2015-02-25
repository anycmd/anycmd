
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
    using ViewModels.ButtonViewModels;

    /// <summary>
    /// 按钮模型视图控制器
    /// </summary>
    [Guid("8DCD2F37-003D-444C-940E-DD5C067362DA")]
    public class ButtonController : AnycmdController
    {
        #region ViewPages

        [By("xuexs")]
        [Description("按钮管理")]
        [Guid("4DF175ED-A938-4EC9-8FD0-6E427F82F603")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("按钮详细信息")]
        [Guid("248E2808-B044-44C1-B979-9BBF1EA45186")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = ButtonInfo.Create(base.EntityType.GetData(id));
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

        #region Get
        [By("xuexs")]
        [Description("根据ID获取按钮")]
        [Guid("E15A4098-7002-47B3-ADD7-26AB8F25B358")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }
        #endregion

        #region GetInfo
        [By("xuexs")]
        [Description("根据ID获取按钮详细信息")]
        [Guid("E5F1585B-CC34-4F82-9C4A-F424E3632E13")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(ButtonInfo.Create(base.EntityType.GetData(id.Value)));
        }
        #endregion

        #region GetPlistButtons
        [By("xuexs")]
        [Description("分页获取按钮")]
        [Guid("A0A2B595-E090-47CA-98AA-FB658F39D6F0")]
        public ActionResult GetPlistButtons(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistButtons(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<ButtonTr> { total = requestModel.Total.Value, data = data });
        }
        #endregion

        #region GetPlistUIViewButtons
        [By("xuexs")]
        [Description("分页获取页面按钮")]
        [Guid("2E137B81-C0B6-42B7-969D-CD29A72B67F8")]
        public ActionResult GetPlistUiViewButtons(GetPlistUiViewButtons requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistUiViewButtons(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<UiViewAssignButtonTr> { total = requestData.Total.Value, data = data });
        }
        #endregion

        #region Add
        [By("xuexs")]
        [Description("添加按钮")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("E529EDE8-9EED-446A-9B79-3B5DA04BABEE")]
        public ActionResult Create(ButtonCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }
        #endregion

        #region Update
        [By("xuexs")]
        [Description("更新按钮")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("02F499AD-CD79-4206-9E78-55CC711179D2")]
        public ActionResult Update(ButtonUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }
        #endregion

        #region Delete
        [By("xuexs")]
        [Description("删除按钮")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("DC8CD97F-220E-46BF-BBFC-6D684994330A")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveButton, AcSession, id, ',');
        }
        #endregion
    }
}
