
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
    using ViewModels.NodeViewModels;

    /// <summary>
    /// 节点模型视图控制器<see cref="Node"/>
    /// </summary>
    [Guid("5161FB00-7A57-4466-BDDF-6AC0E08E56C3")]
    public class NodeController : AnycmdController
    {
        #region ViewResults

        /// <summary>
        /// 节点管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("节点管理")]
        [Guid("60BF431C-42A2-4999-A536-47CED78E35CB")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 节点详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("节点详细信息")]
        [Guid("351F7EFB-303D-42BB-9D3B-293C978061DB")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new NodeInfo(base.EntityType.GetData(id));
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
        /// 用以控制权限，Action名和当前Action所在应用系统名、区域名、控制器名用来生成操作码和权限码。
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或修改")]
        [Guid("09EE6686-CD34-4D76-B8C1-83EE585B4319")]
        public ViewResultBase Edit()
        {
            return ViewResult();
        }

        /// <summary>
        /// 关心本体元素
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("关心本体元素")]
        [Guid("EFF597E1-51E0-426F-9957-3EE03CA48775")]
        public ViewResultBase NodeElementCares()
        {
            return ViewResult();
        }

        /// <summary>
        /// 节点组织结构
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("节点组织结构")]
        [Guid("F2EA000E-9D4A-4665-BB53-9B7790F897E4")]
        public ViewResultBase Organizations()
        {
            return ViewResult();
        }

        /// <summary>
        /// 权限
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("权限")]
        [Guid("04ED7234-1DCB-4612-8A75-3BF04DC7BA9F")]
        public ViewResultBase Permissions()
        {
            return ViewResult();
        }

        /// <summary>
        /// 关心本本体的节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("关心本本体的节点")]
        [Guid("4A139599-B0B7-4749-9AEE-2388087586F7")]
        public ViewResultBase OntologyNodeCares()
        {
            return ViewResult();
        }

        #endregion

        /// <summary>
        /// 根据节点ID获取节点详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据节点ID获取节点详细信息")]
        [Guid("65657CEB-A266-4806-A30B-5A988AF9D190")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Node>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 根据节点ID获取节点详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据节点ID获取节点详细信息")]
        [Guid("A4EDF595-5EDA-414A-8738-336EC1E999D7")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new NodeInfo(base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 根据本体ID和节点ID分页获取节点动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据本体ID和节点ID分页获取节点动作")]
        [Guid("8070F34A-59F9-4EF7-A0C5-4241614D52FF")]
        public ActionResult GetPlistNodeActions(GetPlistNodeActions requestModel)
        {
            NodeDescriptor node;
            if (!AcDomain.NodeHost.Nodes.TryGetNodeById(requestModel.NodeId.ToString(), out node))
            {
                throw new ValidationException("意外的节点标识" + requestModel.NodeId);
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(requestModel.OntologyId, out ontology))
            {
                throw new ValidationException("意外的本体标识" + requestModel.OntologyId);
            }
            var list = new List<NodeAssignActionTr>();
            foreach (var action in ontology.Actions.Values)
            {
                Guid id;
                string isAllowed;
                string isAudit;
                var nodeActions = node.Node.NodeActions;
                if (nodeActions.ContainsKey(ontology) && nodeActions[ontology].ContainsKey(action.ActionVerb))
                {
                    var nodeAction = nodeActions[ontology][action.ActionVerb];
                    id = nodeAction.Id;
                    isAllowed = nodeAction.IsAllowed;
                    isAudit = nodeAction.IsAudit;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAllowed = AllowType.ImplicitAllow.ToName();
                    isAudit = AuditType.ImplicitAudit.ToName();
                }
                var item = new NodeAssignActionTr
                {
                    ActionId = action.Id,
                    ActionIsAllow = action.AllowType.ToName(),
                    ActionIsAudit = action.AuditType.ToName(),
                    ActionIsPersist = action.IsPersist,
                    Id = id,
                    Name = action.Name,
                    NodeCode = node.Node.Code,
                    NodeId = node.Node.Id,
                    NodeName = node.Node.Name,
                    OntologyId = action.OntologyId,
                    OntologyName = ontology.Ontology.Name,
                    Verb = action.Verb,
                    IsAllowed = isAllowed,
                    IsAudit = isAudit
                };
                list.Add(item);
            }
            var data = new MiniGrid<NodeAssignActionTr> { total = list.Count, data = list };

            return this.JsonResult(data);
        }

        /// <summary>
        /// 根据本体元素ID和节点ID分页获取动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据本体元素ID和节点ID分页获取动作")]
        [Guid("CCB2739B-0916-405D-BD29-A1D5BAE1CAE7")]
        public ActionResult GetPlistNodeElementActions(Guid? nodeId, Guid? elementId, GetPlistResult requestModel)
        {
            if (!nodeId.HasValue)
            {
                throw new ValidationException("nodeID是必须的");
            }
            if (!elementId.HasValue)
            {
                throw new ValidationException("elementID是必须的");
            }
            NodeDescriptor node;
            if (!AcDomain.NodeHost.Nodes.TryGetNodeById(nodeId.Value.ToString(), out node))
            {
                throw new ValidationException("意外的节点标识" + nodeId);
            }
            ElementDescriptor element;
            if (!AcDomain.NodeHost.Ontologies.TryGetElement(elementId.Value, out element))
            {
                throw new ValidationException("意外的本体元素标识" + elementId);
            }
            var elementActions = element.Element.ElementActions;
            var data = new List<NodeElementAssignActionTr>();
            foreach (var action in element.Ontology.Actions.Values)
            {
                var elementAction = elementActions.ContainsKey(action.ActionVerb) ? elementActions[action.ActionVerb] : null;
                string elementActionAllowType = AllowType.ImplicitAllow.ToName();
                string elementActionAuditType = AuditType.ImplicitAudit.ToName();
                if (elementAction != null)
                {
                    elementActionAllowType = elementAction.AllowType.ToName();
                    elementActionAuditType = elementAction.AuditType.ToName();
                }
                var nodeElementActions = AcDomain.NodeHost.Nodes.GetNodeElementActions(node, element).Values;
                var id = Guid.NewGuid();
                DateTime? createOn = null;
                bool isAllowed = false;
                bool isAudit = false;
                var nodeElementAction = nodeElementActions.FirstOrDefault(a => a.NodeId == node.Node.Id && a.ElementId == element.Element.Id && a.ActionId == action.Id);
                if (nodeElementAction != null)
                {
                    id = nodeElementAction.Id;
                    createOn = nodeElementAction.CreateOn;
                    isAllowed = nodeElementAction.IsAllowed;
                    isAudit = nodeElementAction.IsAudit;
                }
                data.Add(new NodeElementAssignActionTr
                {
                    Id = id,
                    Name = action.Name,
                    NodeId = node.Node.Id,
                    OntologyId = action.OntologyId,
                    SortCode = action.SortCode,
                    ActionId = action.Id,
                    ActionIsAllow = action.AllowType.ToName(),
                    ElementActionIsAllow = elementActionAllowType,
                    ElementActionIsAudit = elementActionAuditType,
                    ElementCode = element.Element.Code,
                    ElementId = element.Element.Id,
                    ElementName = element.Element.Name,
                    IsAllowed = isAllowed,
                    IsAudit = isAudit,
                    Verb = action.Verb
                });
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = data.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<NodeElementAssignActionTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 根据本体ID分页获取关心该本体的节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("根据本体ID分页获取关心该本体的节点")]
        [Guid("79E7D7BA-E7C0-4636-AF18-29C699BA1647")]
        public ActionResult GetPlistNodeOntologyCares(Guid ontologyId, GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId, out ontology))
            {
                throw new ValidationException("意外的本体标识" + ontologyId);
            }
            var data = new List<OntologyAssignNodeTr>();
            foreach (var node in AcDomain.NodeHost.Nodes)
            {
                var nodeOntologyCares = AcDomain.NodeHost.Nodes.GetNodeOntologyCares(node);
                var id = Guid.NewGuid();
                var isAssigned = false;
                DateTime? createOn = null;
                var nodeOntologyCare = nodeOntologyCares.FirstOrDefault(a => a.NodeId == node.Node.Id && a.OntologyId == ontology.Ontology.Id);
                if (nodeOntologyCare != null)
                {
                    id = nodeOntologyCare.Id;
                    isAssigned = true;
                    createOn = nodeOntologyCare.CreateOn;
                }
                data.Add(new OntologyAssignNodeTr
                {
                    Code = node.Node.Code,
                    CreateOn = createOn,
                    Icon = node.Node.Icon,
                    Id = id,
                    IsAssigned = isAssigned,
                    IsEnabled = node.Node.IsEnabled,
                    Name = node.Node.Name,
                    NodeId = node.Node.Id,
                    OntologyId = ontology.Ontology.Id,
                    SortCode = node.Node.SortCode
                });
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = data.AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<OntologyAssignNodeTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 分页获取节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取节点")]
        [Guid("877CB916-A5CA-4753-86E9-1CCA77CD92D7")]
        public ActionResult GetPlistNodes(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "Node"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.Node");
            }
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Node实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = AcDomain.NodeHost.Nodes.Select(NodeTr.Create).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<NodeTr> { total = queryable.Count(), data = list });
        }

        #region GetOrganizationNodesByParentId
        /// <summary>
        /// 获取给定本体的组织结构
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取给定本体的组织结构")]
        [Guid("4C357D12-2DCD-44C7-8A82-86C5514E5709")]
        public ActionResult GetOrganizationNodesByParentId(Guid? nodeId, Guid? ontologyId, Guid? parentId)
        {
            if (!nodeId.HasValue)
            {
                throw new ValidationException("未传入节点标识");
            }
            if (!ontologyId.HasValue)
            {
                throw new ValidationException("未传入本体标识");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体表示");
            }
            if (parentId == Guid.Empty)
            {
                parentId = null;
            }
            string parentCode = null;
            if (parentId.HasValue)
            {
                OrganizationState org;
                if (!AcDomain.OrganizationSet.TryGetOrganization(parentId.Value, out org))
                {
                    throw new ValidationException("意外的组织结构标识" + parentId);
                }
                parentCode = org.Code;
            }
            var ontologyOrgs = ontology.Organizations;
            var orgs = AcDomain.OrganizationSet.Where(ontologyOrgs.ContainsKey);
            var noos = GetRequiredService<IRepository<NodeOntologyOrganization>>().AsQueryable().Where(a => a.OntologyId == ontologyId.Value && a.NodeId == nodeId.Value).ToList<NodeOntologyOrganization>();
            return this.JsonResult(orgs.Where(a => string.Equals(a.ParentCode, parentCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Code)
                .Select(a =>
                {
                    var norg = noos.FirstOrDefault(b => b.OrganizationId == a.Id);
                    bool @checked = true;
                    if (norg == null)
                    {
                        @checked = false;
                    }
                    else
                    {
                        @checked = true;
                    }

                    return new
                    {
                        a.Id,
                        a.Code,
                        a.Name,
                        ParentId = a.ParentCode,
                        isLeaf = AcDomain.OrganizationSet.All(b => !a.Code.Equals(b.ParentCode, StringComparison.OrdinalIgnoreCase)),
                        expanded = false,
                        @checked = @checked,
                        NodeId = nodeId,
                        OntologyId = ontologyId
                    };
                }).ToList());
        }
        #endregion

        #region AddOrRemoveOrganizations
        /// <summary>
        /// 添加或移除本体组织结构
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或移除本体组织结构")]
        [Guid("58281438-8B27-49FA-AE53-2C569D51EC63")]
        [HttpPost]
        public ActionResult AddOrRemoveOrganizations(Guid nodeId, Guid ontologyId, string addOrganizationIDs, string removeOrganizationIDs)
        {
            string[] addIDs = addOrganizationIDs.Split(',');
            string[] removeIDs = removeOrganizationIDs.Split(',');
            foreach (var item in addIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var organizationId = new Guid(item);
                    AcDomain.Handle(new AddNodeOntologyOrganizationCommand(UserSession, new NodeOntologyOrganizationCreateInput
                    {
                        Id = Guid.NewGuid(),
                        NodeId = nodeId,
                        OntologyId = ontologyId,
                        OrganizationId = organizationId
                    }));
                }
            }
            foreach (var item in removeIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var organizationId = new Guid(item);
                    AcDomain.Handle(new RemoveNodeOntologyOrganizationCommand(UserSession, nodeId, ontologyId, organizationId));
                }
            }
            GetRequiredService<IRepository<NodeOntologyOrganization>>().Context.Commit();

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加新节点")]
        [HttpPost]
        [Guid("1E47784A-FB3E-4AD8-877F-5DAE3FBF0008")]
        public ActionResult Create(NodeCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.AddNode(UserSession, input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新节点")]
        [HttpPost]
        [Guid("866B810D-FD6E-4319-82C0-7435B6B212CC")]
        public ActionResult Update(NodeUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.UpdateNode(UserSession, input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        #region UpdateNodes
        /// <summary>
        /// 更新节点配置
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新节点配置")]
        [HttpPost]
        [Guid("5C27DD39-C3D4-4375-8F1B-74448F6FA13C")]
        public ActionResult UpdateNodes()
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
                    var id = new Guid(row["Id"].ToString());
                    var isExecuteEnabled = bool.Parse(row["IsExecuteEnabled"].ToString());
                    var isProduceEnabled = bool.Parse(row["IsProduceEnabled"].ToString());
                    var isReceiveEnabled = bool.Parse(row["IsReceiveEnabled"].ToString());
                    var isTransferEnabled = bool.Parse(row["IsDistributeEnabled"].ToString());
                    Node entity = GetRequiredService<IRepository<Node>>().GetByKey(id);
                    if (entity != null)
                    {
                        entity.IsExecuteEnabled = isExecuteEnabled;
                        entity.IsProduceEnabled = isProduceEnabled;
                        entity.IsReceiveEnabled = isReceiveEnabled;
                        entity.IsDistributeEnabled = isTransferEnabled;
                        AcDomain.Handle(new UpdateNodeCommand(UserSession, new NodeUpdateInput
                        {
                            Abstract = entity.Abstract,
                            AnycmdApiAddress = entity.AnycmdApiAddress,
                            AnycmdWsAddress = entity.AnycmdWsAddress,
                            BeatPeriod = entity.BeatPeriod,
                            Code = entity.Code,
                            Description = entity.Description,
                            Email = entity.Email,
                            IsEnabled = entity.IsEnabled,
                            Icon = entity.Icon,
                            Id = entity.Id,
                            Mobile = entity.Mobile,
                            Name = entity.Name,
                            Organization = entity.Organization,
                            PublicKey = entity.PublicKey,
                            Qq = entity.Qq,
                            SecretKey = entity.SecretKey,
                            SortCode = entity.SortCode,
                            Steward = entity.Steward,
                            Telephone = entity.Telephone,
                            TransferId = entity.TransferId
                        }));
                    }
                    else
                    {
                        throw new AnycmdException("意外的节点");
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region AddOrRemoveOntologies
        /// <summary>
        /// 添加或移除关心本体
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或移除关心本体")]
        [HttpPost]
        [Guid("E65937B1-34B9-4295-AC59-291E9D65BC47")]
        public ActionResult AddOrRemoveOntologies()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";
                var id = new Guid(row["Id"].ToString());
                if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    NodeOntologyCare entity = GetRequiredService<IRepository<NodeOntologyCare>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.RemoveNodeOntologyCare(UserSession, id);
                        }
                    }
                    else if (isAssigned)
                    {
                        AcDomain.AddNodeOntologyCare(UserSession, new NodeOntologyCareCreateInput
                        {
                            Id = id,
                            NodeId = new Guid(row["NodeId"].ToString()),
                            OntologyId = new Guid(row["OntologyId"].ToString())
                        });
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region AddOrRemoveElementCares
        /// <summary>
        /// 添加或移除关心本体元素
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或移除关心本体元素")]
        [HttpPost]
        [Guid("3ED242C7-BD85-4B58-86D8-9F967A91BD2B")]
        public ActionResult AddOrRemoveElementCares()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";
                var id = new Guid(row["Id"].ToString());
                if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    NodeElementCare entity = GetRequiredService<IRepository<NodeElementCare>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.RemoveNodeElementCare(UserSession, id);
                        }
                        else
                        {
                            // TODO:IsInfoIDItem字段需要update
                        }
                    }
                    else if (isAssigned)
                    {
                        AcDomain.AddNodeElementCare(UserSession, new NodeElementCareCreateInput
                        {
                            Id = id,
                            NodeId = new Guid(row["NodeId"].ToString()),
                            ElementId = new Guid(row["ElementId"].ToString()),
                            IsInfoIdItem = bool.Parse(row["IsInfoIDItem"].ToString())
                        });
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        // TODO:逻辑移动到应用服务层
        #region AddOrUpdateNodeActions
        /// <summary>
        /// 添加或更新本体级权限
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或更新本体级权限")]
        [HttpPost]
        [Guid("99D5DA43-CB4B-426E-B43B-7AFA3B6F8D6E")]
        public ActionResult AddOrUpdateNodeActions()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";
                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    var inputModel = new NodeAction()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        NodeId = new Guid(row["NodeId"].ToString()),
                        ActionId = new Guid(row["ActionId"].ToString()),
                        IsAllowed = row["IsAllowed"].ToString(),
                        IsAudit = row["IsAudit"].ToString()
                    };
                    NodeDescriptor nodeDescriptor;
                    if (!AcDomain.NodeHost.Nodes.TryGetNodeById(inputModel.NodeId.ToString(), out nodeDescriptor))
                    {
                        throw new ValidationException("意外的节点标识" + inputModel.NodeId);
                    }
                    NodeAction entity = null;
                    if (nodeDescriptor != null)
                    {
                        entity = new NodeAction
                        {
                            Id = inputModel.Id,
                            ActionId = inputModel.ActionId,
                            IsAllowed = inputModel.IsAllowed,
                            IsAudit = inputModel.IsAudit,
                            NodeId = inputModel.NodeId
                        };
                        AcDomain.PublishEvent(new NodeActionUpdatedEvent(UserSession, entity));
                    }
                    else
                    {
                        entity = new NodeAction
                        {
                            Id = inputModel.Id,
                            NodeId = inputModel.NodeId,
                            ActionId = inputModel.ActionId,
                            IsAudit = inputModel.IsAudit,
                            IsAllowed = inputModel.IsAllowed
                        };
                        AcDomain.PublishEvent(new NodeActionAddedEvent(UserSession, entity));
                    }
                    AcDomain.CommitEventBus();
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        // TODO:逻辑移动到应用服务层
        #region AddOrUpdateNodeElementActions
        /// <summary>
        /// 添加或更新节点本体元素级权限
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或更新节点本体元素级权限")]
        [HttpPost]
        [Guid("51366E78-357E-44B2-B960-C3CCD6E17B57")]
        public ActionResult AddOrUpdateNodeElementActions()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";
                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    var inputModel = new NodeElementAction()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        NodeId = new Guid(row["NodeId"].ToString()),
                        ElementId = new Guid(row["ElementId"].ToString()),
                        ActionId = new Guid(row["ActionId"].ToString()),
                        IsAllowed = bool.Parse(row["IsAllowed"].ToString()),
                        IsAudit = bool.Parse(row["IsAudit"].ToString())
                    };
                    NodeElementAction entity = GetRequiredService<IRepository<NodeElementAction>>().GetByKey(inputModel.Id);
                    if (entity != null)
                    {
                        entity.IsAudit = inputModel.IsAudit;
                        entity.IsAllowed = inputModel.IsAllowed;
                        GetRequiredService<IRepository<NodeElementAction>>().Update(entity);
                    }
                    else
                    {
                        entity = new NodeElementAction();
                        entity.Id = inputModel.Id;
                        entity.NodeId = inputModel.NodeId;
                        entity.ElementId = inputModel.ElementId;
                        entity.ActionId = inputModel.ActionId;
                        entity.IsAudit = inputModel.IsAudit;
                        entity.IsAllowed = inputModel.IsAllowed;
                        var count = GetRequiredService<IRepository<NodeElementAction>>().AsQueryable().Count(a => a.NodeId == entity.NodeId
                                                                                                                  && a.ElementId == entity.ElementId
                                                                                                                  && a.ActionId == entity.ActionId);
                        if (count > 0)
                        {
                            throw new ValidationException("给定的节点已拥有给定的动作，无需重复关联");
                        }
                        GetRequiredService<IRepository<NodeElementAction>>().Add(entity);
                    }
                    GetRequiredService<IRepository<NodeElementAction>>().Context.Commit();
                    AcDomain.CommitEventBus();
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除节点")]
        [HttpPost]
        [Guid("462C7447-5C3F-4363-9604-2407CBE22EDB")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveNode, UserSession, id, ',');
        }
    }
}
