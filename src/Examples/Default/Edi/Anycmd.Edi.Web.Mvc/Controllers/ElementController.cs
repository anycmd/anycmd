
namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Engine.Edi.Messages;
    using Engine.Host.Ac;
    using Engine.Host.Edi.Entities;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.ElementViewModels;

    /// <summary>
    /// 本体元素模型视图控制器<see cref="Element"/>
    /// </summary>
    [Guid("9B7C31CD-7DB3-4D7C-AF0D-5894316ADA2A")]
    public class ElementController : AnycmdController
    {
        #region ViewResults
        /// <summary>
        /// 本体元素管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本体元素管理")]
        [Guid("073AAE95-E327-4ED3-A366-D4EAB4F9100B")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 本体元素详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本体元素详细信息")]
        [Guid("5466397B-5695-403F-BD76-4262A46EAFB6")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = ElementInfo.Create(base.EntityType.GetData(id));
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
        [Description("checks")]
        [Guid("CF8977FF-A7A2-4E3D-A7B1-B7AB4CE7A863")]
        public ViewResultBase Checks()
        {
            return ViewResult();
        }

        /// <summary>
        /// 获取字段提示信息
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取字段提示信息")]
        [Guid("541ECA38-8759-4A09-B7C0-8E77D371793F")]
        public ActionResult Tooltip(Guid? elementId)
        {
            if (elementId.HasValue)
            {
                return this.PartialView(
                    "Partials/Tooltip",
                    AcDomain.NodeHost.Ontologies.GetElement(elementId.Value).Element);
            }
            else
            {
                return this.Content("无效的elementId");
            }
        }

        /// <summary>
        /// 编辑本体元素帮助
        /// </summary>
        /// <param name="isInner"></param>
        /// <param name="tooltip"></param>
        /// <param name="elementId"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("编辑本体元素帮助")]
        [ValidateInput(enableValidation: false)]
        [Guid("59ED73F5-2E0C-4986-9E43-6D04D3148C40")]
        public ActionResult TooltipEdit(string isInner, string tooltip, Guid? elementId)
        {
            if (elementId.HasValue)
            {
                var element = AcDomain.NodeHost.Ontologies.GetElement(elementId.Value);
                if (Request.HttpMethod == "POST")
                {
                    var entity = AcDomain.NodeHost.Ontologies.GetElement(elementId.Value).Element;
                    if (entity != null)
                    {
                        AcDomain.UpdateElement(
                            new ElementUpdateInput
                            {
                                AllowFilter = entity.AllowFilter,
                                AllowSort = entity.AllowSort,
                                Code = entity.Code,
                                Description = entity.Description,
                                FieldCode = entity.FieldCode,
                                GroupId = entity.GroupId,
                                Icon = entity.Icon,
                                Id = entity.Id,
                                InfoDicId = entity.InfoDicId,
                                InputHeight = entity.InputHeight,
                                InputType = entity.InputType,
                                InputWidth = entity.InputWidth,
                                IsDetailsShow = entity.IsDetailsShow,
                                IsEnabled = entity.IsEnabled,
                                IsExport = entity.IsExport,
                                IsGridColumn = entity.IsGridColumn,
                                IsImport = entity.IsImport,
                                IsInfoIdItem = entity.IsInfoIdItem,
                                IsInput = entity.IsInput,
                                IsTotalLine = entity.IsTotalLine,
                                MaxLength = entity.MaxLength,
                                Name = entity.Name,
                                Nullable = entity.Nullable,
                                Ref = entity.Ref,
                                Regex = entity.Regex,
                                SortCode = entity.SortCode,
                                Width = entity.Width,
                                Tooltip = tooltip
                            });
                    }
                    return this.JsonResult(new ResponseData { success = true });
                }
                else
                {
                    return this.PartialView(element.Element);
                }
            }
            else
            {
                return this.Content("无效的elementId");
            }
        }
        #endregion

        /// <summary>
        /// 根据ID获取本体元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取本体元素")]
        [Guid("ADFC1E82-5476-4E3A-A392-9068E18E104E")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Element>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据ID获取本体元素详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据ID获取本体元素详细信息")]
        [Guid("8E9B33EC-4894-46C9-8FDC-5270D8EA96C5")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(ElementInfo.Create(base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页查询本体元素
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页查询本体元素")]
        [Guid("8E40008D-0B76-403D-90A8-6FE967B90E16")]
        public ActionResult GetPlistElements(GetPlistElements input)
        {
            if (!input.OntologyId.HasValue)
            {
                throw new ValidationException("必须传入本体标识");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(input.OntologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体标识" + input.OntologyId);
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "Element"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.Element");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Element实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = ontology.Elements.Values.Select(a => ElementTr.Create(a.Element)).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            if (input.GroupId.HasValue)
            {
                queryable = queryable.Where(a => a.GroupId == input.GroupId.Value);
            }

            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<ElementTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 根据本体元素ID分页获取动作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据本体元素ID分页获取动作")]
        [Guid("1562A56D-8283-411E-814F-6A83B46D502E")]
        public ActionResult GetElementActions(GetElementActions input)
        {
            ElementDescriptor element;
            if (!AcDomain.NodeHost.Ontologies.TryGetElement(input.ElementId, out element))
            {
                throw new ValidationException("意外的本体元素标识" + input.ElementId);
            }
            var list = new List<ElementAssignActionTr>();
            foreach (var action in element.Ontology.Actions.Values)
            {
                Guid id;
                string isAllowed;
                string isAudit;
                if (element.Element.ElementActions.ContainsKey(action.ActionVerb))
                {
                    var elementAction = element.Element.ElementActions[action.ActionVerb];
                    id = elementAction.Id;
                    isAllowed = elementAction.IsAllowed;
                    isAudit = elementAction.IsAudit;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAllowed = AllowType.ImplicitAllow.ToName();
                    isAudit = AuditType.ImplicitAudit.ToName();
                }
                var elementAssignAction = new ElementAssignActionTr
                {
                    ActionId = action.Id,
                    ActionIsAllow = action.AllowType.ToName(),
                    ElementCode = element.Element.Code,
                    ElementId = element.Element.Id,
                    ElementName = element.Element.Name,
                    Id = id,
                    IsAllowed = isAllowed,
                    IsAudit = isAudit,
                    Name = action.Name,
                    OntologyId = action.OntologyId,
                    Verb = action.Verb
                };
                list.Add(elementAssignAction);
            }
            var data = new MiniGrid<ElementAssignActionTr> { total = list.Count, data = list };

            return this.JsonResult(data);
        }

        /// <summary>
        /// 获取节点关心的本体元素
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取节点关心的本体元素")]
        [Guid("083B1FDB-6187-4AF6-A833-5634DBC5F07B")]
        public ActionResult GetNodeElementCares(GetNodeElementCares input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            NodeDescriptor node;
            if (!AcDomain.NodeHost.Nodes.TryGetNodeById(input.NodeId.ToString(), out node))
            {
                throw new ValidationException("意外的节点标识" + input.NodeId);
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(input.OntologyId, out ontology))
            {
                throw new ValidationException("意外的本体标识" + input.OntologyId);
            }
            var data = new List<NodeAssignElementTr>();
            var nodeElementCares = AcDomain.NodeHost.Nodes.GetNodeElementCares(node);
            foreach (var element in ontology.Elements.Values)
            {
                var id = Guid.NewGuid();
                var isAssigned = false;
                var isInfoIdItem = false;
                var nodeElementCare = nodeElementCares.FirstOrDefault(a => a.NodeId == input.NodeId && a.ElementId == element.Element.Id);
                if (nodeElementCare != null)
                {
                    id = nodeElementCare.Id;
                    isAssigned = true;
                    isInfoIdItem = nodeElementCare.IsInfoIdItem;
                }
                data.Add(new NodeAssignElementTr
                {
                    Code = element.Element.Code,
                    CreateOn = element.Element.CreateOn,
                    ElementId = element.Element.Id,
                    ElementIsInfoIdItem = element.Element.IsInfoIdItem,
                    Icon = element.Element.Icon,
                    Id = id,
                    IsAssigned = isAssigned,
                    IsEnabled = element.Element.IsEnabled,
                    IsInfoIdItem = isInfoIdItem,
                    Name = element.Element.Name,
                    NodeId = input.NodeId,
                    OntologyId = input.OntologyId,
                    SortCode = element.Element.SortCode
                });
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = data.AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<NodeAssignElementTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加本体元素
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加本体元素")]
        [HttpPost]
        [Guid("886762FC-C4D8-4C71-AFCC-9B3118B0403F")]
        public ActionResult Create(ElementCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.AddElement(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新本体元素
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新本体元素")]
        [HttpPost]
        [Guid("0DA4C841-AEFF-4553-B4E2-31AD58650935")]
        public ActionResult Update(ElementUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.UpdateElement(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        // TODO:逻辑移动到应用服务层
        #region AddOrUpdateElementActions
        /// <summary>
        /// 添加或更新本体元素级动作权限
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或更新本体元素级动作权限")]
        [HttpPost]
        [Guid("9D5B28EB-569D-4BA4-85E5-831379B13D25")]
        public ActionResult AddOrUpdateElementActions()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    var inputModel = new ElementAction()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        ElementId = new Guid(row["ElementId"].ToString()),
                        ActionId = new Guid(row["ActionId"].ToString()),
                        IsAudit = row["IsAudit"].ToString(),
                        IsAllowed = row["IsAllowed"].ToString()
                    };
                    ElementDescriptor element;
                    AcDomain.NodeHost.Ontologies.TryGetElement(inputModel.ElementId, out element);
                    ElementAction entity = null;
                    if (element != null)
                    {
                        entity = new ElementAction
                        {
                            ActionId = inputModel.ActionId,
                            IsAllowed = inputModel.IsAllowed,
                            IsAudit = inputModel.IsAudit,
                            ElementId = element.Element.Id,
                            Id = inputModel.Id
                        };
                        AcDomain.PublishEvent(new ElementActionUpdatedEvent(entity));
                    }
                    else
                    {
                        entity = new ElementAction
                        {
                            Id = inputModel.Id,
                            ElementId = inputModel.ElementId,
                            ActionId = inputModel.ActionId,
                            IsAudit = inputModel.IsAudit,
                            IsAllowed = inputModel.IsAllowed
                        };
                        AcDomain.PublishEvent(new ElementActionAddedEvent(entity));
                    }
                    AcDomain.CommitEventBus();
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        // TODO:逻辑移动到应用服务层
        #region AddOrRemoveNodes
        /// <summary>
        /// 添加或移除关心节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或移除关心节点")]
        [HttpPost]
        [Guid("CCE8761D-9301-4711-A078-C1BC8DCBDF73")]
        public ActionResult AddOrRemoveNodes()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    var inputModel = new NodeElementCare()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        NodeId = new Guid(row["NodeId"].ToString()),
                        ElementId = new Guid(row["ElementId"].ToString())
                    };
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    NodeElementCare entity = GetRequiredService<IRepository<NodeElementCare>>().GetByKey(inputModel.Id);
                    bool isNew = true;
                    if (entity != null)
                    {
                        isNew = false;
                        if (!isAssigned)
                        {
                            GetRequiredService<IRepository<NodeElementCare>>().Remove(entity);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        entity = new NodeElementCare {Id = inputModel.Id};
                    }
                    if (isAssigned)
                    {
                        entity.NodeId = inputModel.NodeId;
                        entity.ElementId = inputModel.ElementId;
                        if (isNew)
                        {
                            var count = GetRequiredService<IRepository<NodeElementCare>>().AsQueryable().Count(a => a.ElementId == entity.ElementId
                                                                                                                    && a.NodeId == entity.NodeId);
                            if (count > 0)
                            {
                                throw new ValidationException("给定的节点已关心给定的本体元素，无需重复关心");
                            }
                            GetRequiredService<IRepository<NodeElementCare>>().Add(entity);
                        }
                        else
                        {
                            GetRequiredService<IRepository<NodeElementCare>>().Update(entity);
                        }
                    }
                    GetRequiredService<IRepository<NodeElementCare>>().Context.Commit();
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除本体元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除本体元素")]
        [HttpPost]
        [Guid("9E7D2E71-A143-44C0-AE7A-5011E64D8B9D")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveElement, id, ',');
        }
        #endregion
    }
}
