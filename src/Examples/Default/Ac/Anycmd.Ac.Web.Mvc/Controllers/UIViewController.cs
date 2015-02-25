
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.FunctionViewModels;
    using ViewModels.UIViewViewModels;

    /// <summary>
    /// 系统页面模型视图控制器<see cref="UiView"/>
    /// </summary>
    [Guid("44AED0F0-9508-4406-8B84-CCEACD79F591")]
    public class UiViewController : AnycmdController
    {
        private readonly EntityTypeState _functionEntityType;

        public UiViewController()
        {
            _functionEntityType = base.GetEntityType(new Coder("Ac", "Function"));
        }

        #region ViewResults
        [By("xuexs")]
        [Description("页面集")]
        [Guid("9CFD1178-5AE2-49B8-A759-3F808A67AC7C")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("页面详细信息")]
        [Guid("9D6B0ED6-F0A8-4F2A-96F3-BC7AAA88E245")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = FunctionInfo.Create(_functionEntityType.GetData(id));
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

        [By("xuexs")]
        [Description("页面按钮列表")]
        [Guid("2C12A712-E3C8-4F5A-8C00-56B012BB2F66")]
        public ViewResultBase UiViewButtons()
        {
            return ViewResult();
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取页面")]
        [Guid("6D985A58-C4ED-4ADD-A62D-3409EEADDA46")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取页面详细信息")]
        [Guid("4615F1FE-3A74-46AE-BE29-7A3D7F157D3B")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(FunctionInfo.Create(_functionEntityType.GetData(id.Value)));
        }

        [By("xuexs")]
        [Description("获取页面提示信息")]
        [Guid("E5F6FE68-48DF-4437-BE95-F22D47845896")]
        public ActionResult Tooltip(Guid? uiViewId)
        {
            if (uiViewId.HasValue)
            {
                UiViewState view;
                IFunction function;
                if (!AcDomain.UiViewSet.TryGetUiView(uiViewId.Value, out view))
                {
                    view = UiViewState.Empty;
                    function = new Function();
                }
                else
                {
                    FunctionState functionState;
                    if (!AcDomain.FunctionSet.TryGetFunction(view.Id, out functionState))
                    {
                        throw new ValidationException("意外的功能标识" + view.Id);
                    }
                    function = functionState;
                }
                return this.PartialView("Partials/Tooltip", new UiViewViewModel(view, function.Description));
            }
            else
            {
                return this.Content("无效的viewId");
            }
        }

        [By("xuexs")]
        [Description("编辑页面帮助")]
        [Guid("B741E0B4-D663-498E-B0D0-1D31B47D6CD0")]
        public ActionResult TooltipEdit(string isInner, Guid? viewId)
        {
            if (viewId.HasValue)
            {
                UiViewState view;
                IFunction function;
                if (!AcDomain.UiViewSet.TryGetUiView(viewId.Value, out view))
                {
                    view = UiViewState.Empty;
                    function = new Function();
                }
                else
                {
                    FunctionState functionState;
                    if (!AcDomain.FunctionSet.TryGetFunction(view.Id, out functionState))
                    {
                        throw new ValidationException("意外的功能标识" + view.Id);
                    }
                    function = functionState;
                }
                return this.PartialView(new UiViewViewModel(view, function.Description));
            }
            else
            {
                return this.Content("无效的viewId");
            }
        }

        [By("xuexs")]
        [Description("编辑页面帮助")]
        [ValidateInput(enableValidation: false)]
        [Guid("0ED77A03-FD6F-4C12-A132-116A2DA80AD0")]
        public ActionResult SaveTooltip(string tooltip, Guid? uiViewId)
        {
            if (!uiViewId.HasValue)
            {
                throw new ValidationException("非法的页面标识");
            }
            var entity = GetRequiredService<IRepository<UiView>>().GetByKey(uiViewId.Value);
            if (entity == null)
            {
                throw new ValidationException("标识为" + uiViewId + "的页面不存在");
            }
            AcDomain.Handle(new UiViewUpdateInput
            {
                Icon = entity.Icon,
                Id = entity.Id,
                Tooltip = tooltip
            }.ToCommand(AcSession));
            return this.JsonResult(new ResponseData { success = true });
        }

        [By("xuexs")]
        [Description("分页获取页面")]
        [Guid("BC1A6CCA-233C-4BCB-BF8F-52CE47011233")]
        public ActionResult GetPlistUiViews(GetPlistResult requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistUiViews(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<UiViewTr> { total = requestData.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加页面")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("2D266DE3-2B9E-4D4A-86E7-E696EB1224B0")]
        public ActionResult Create(UiViewCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("更新页面")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("932860B5-285A-4866-B8F2-A33CE0B84C80")]
        public ActionResult Update(UiViewUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("添加或移除页面按钮")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("D1EC1643-C4E7-4BEF-8411-6400F4D5B3CB")]
        public ActionResult AddOrRemoveButtons()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    string functionIdStr = row["FunctionId"] == null ? null : row["FunctionId"].ToString();
                    Guid? functionId = string.IsNullOrEmpty(functionIdStr) ? null : new Guid?(new Guid(functionIdStr));
                    if (functionId.HasValue)
                    {
                        FunctionState function;
                        if (!AcDomain.FunctionSet.TryGetFunction(functionId.Value, out function))
                        {
                            throw new ValidationException("意外的托管功能标识" + functionId.Value);
                        }
                    }
                    var inputModel = new UiViewButton
                    {
                        Id = new Guid(row["Id"].ToString()),
                        IsEnabled = int.Parse(row["IsEnabled"].ToString()),
                        ButtonId = new Guid(row["ButtonId"].ToString()),
                        UiViewId = new Guid(row["UiViewId"].ToString()),
                        FunctionId = functionId
                    };

                    if (bool.Parse(row["IsAssigned"].ToString()))
                    {
                        if (AcDomain.RetrieveRequiredService<IRepository<UiViewButton>>().AsQueryable().Any(a => a.Id == inputModel.Id))
                        {
                            var updateModel = new UiViewButtonUpdateInput()
                            {
                                Id = inputModel.Id,
                                IsEnabled = inputModel.IsEnabled,
                                FunctionId = inputModel.FunctionId
                            };
                            AcDomain.Handle(updateModel.ToCommand(AcSession));
                        }
                        else
                        {
                            var input = new UiViewButtonCreateInput()
                            {
                                Id = inputModel.Id,
                                ButtonId = inputModel.ButtonId,
                                IsEnabled = inputModel.IsEnabled,
                                FunctionId = inputModel.FunctionId,
                                UiViewId = inputModel.UiViewId
                            };
                            AcDomain.Handle(input.ToCommand(AcSession));
                        }
                    }
                    else
                    {
                        AcDomain.Handle(new RemoveUiViewButtonCommand(AcSession, inputModel.Id));
                    }
                    if (functionId.HasValue)
                    {
                        int functionIsEnabled = int.Parse(row["FunctionIsEnabled"].ToString());
                        FunctionState function;
                        if (!AcDomain.FunctionSet.TryGetFunction(functionId.Value, out function))
                        {
                            throw new AnycmdException("意外的功能标识" + functionId.Value);
                        }
                        var input = new FunctionUpdateInput
                        {
                            Id = function.Id,
                            Code = function.Code,
                            SortCode = function.SortCode,
                            IsManaged = function.IsManaged,
                            IsEnabled = function.IsEnabled,
                            DeveloperId = function.DeveloperId,
                            Description = function.Description
                        };
                        input.IsEnabled = functionIsEnabled;
                        AcDomain.Handle(input.ToCommand(AcSession));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }

        [By("xuexs")]
        [Description("删除页面")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("C4D88E1E-4FC4-40F2-BDEF-6830AF0F1FA8")]
        public ActionResult Delete(string id)
        {
            string[] ids = id.Split(',');
            var idArray = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    idArray[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的应用系统标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemoveUiViewCommand(AcSession, item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
