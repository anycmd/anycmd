using System;

namespace Anycmd.Edi.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Engine.Edi.Messages;
    using Engine.Hecp;
    using Engine.Host.Ac;
    using Engine.Host.Edi.Entities;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.NodeViewModels;
    using ViewModels.OntologyViewModels;

    /// <summary>
    /// 本体模型视图控制器<see cref="Ontology"/>
    /// </summary>
    [Guid("D69E070B-CE3C-4359-9FF5-964F1D57621E")]
    public class OntologyController : AnycmdController
    {
        #region ViewPages

        /// <summary>
        /// 本体管理
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本体管理")]
        [Guid("1EFA1AF4-E824-43E2-A503-6FA8C52FBC8C")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        /// <summary>
        /// 本体详细信息
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本体详细信息")]
        [Guid("5ECDC0D3-CB7C-444E-B396-B1A5429CBB77")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = new OntologyInfo(AcDomain, base.EntityType.GetData(id));
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
        /// 信息组
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("信息组")]
        [Guid("5D382888-C36E-48E5-8E7B-F3245EAEDF33")]
        public ViewResultBase InfoGroups()
        {
            return ViewResult();
        }

        /// <summary>
        /// 动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("动作")]
        [Guid("91DEF5C6-C2C7-493E-80F6-86174B9346FF")]
        public ViewResultBase Actions()
        {
            return ViewResult();
        }

        /// <summary>
        /// 事件主题
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("事件主题")]
        [Guid("EC526A6E-4A27-49E7-B9EF-3846638BD78D")]
        public ViewResultBase Topics()
        {
            return ViewResult();
        }

        /// <summary>
        /// 本体元素
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("本体元素")]
        [Guid("71C15C09-A5B4-43A1-AFA6-E3E870EFA30B")]
        public ViewResultBase Elements()
        {
            return ViewResult();
        }

        /// <summary>
        /// 目录
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("目录")]
        [Guid("6A8FE076-F94D-407E-AAA4-571427A7EE27")]
        public ViewResultBase Catalogs()
        {
            return ViewResult();
        }

        #endregion

        /// <summary>
        /// 获取本体详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取本体详细信息")]
        [Guid("CCC22C34-29D3-4AF5-B03D-AFC7A6863BEA")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IRepository<Ontology>>().GetByKey(id.Value));
        }

        /// <summary>
        /// 获取本体详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取本体详细信息")]
        [Guid("E3778BA4-1158-47AB-B726-949F50D3230B")]
        public ActionResult GetInfo(Guid? id, string code)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(new OntologyInfo(AcDomain, base.EntityType.GetData(id.Value)));
        }

        /// <summary>
        /// 分页获取本体
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取本体")]
        [Guid("9696420B-E517-4F26-8137-0E2D9A7FEC04")]
        public ActionResult GetPlistOntologies(GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "Ontology"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.Ontology");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Ontology实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = AcDomain.NodeHost.Ontologies.Select(OntologyTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<OntologyTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加新本体
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加新本体")]
        [HttpPost]
        [Guid("CD163195-A1AC-436D-98B8-A33A2ED29189")]
        public ActionResult Create(OntologyCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新本体
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新本体")]
        [HttpPost]
        [Guid("F1FD3B65-D686-48ED-9EFC-F42DCCC8CEAB")]
        public ActionResult Update(OntologyUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 获取信息组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取信息组")]
        [Guid("7CF0821B-BC5C-49F2-AE8A-0CA447B2C4F1")]
        public ActionResult GetInfoGroup(Guid id)
        {
            var data = GetRequiredService<IRepository<Ontology>>().Context.Query<InfoGroup>().FirstOrDefault(a => a.Id == id);

            return this.JsonResult(data);
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取动作")]
        [Guid("842C61A2-1E3E-4714-8B08-91D6A762DB55")]
        public ActionResult GetAction(Guid id)
        {
            var data = GetRequiredService<IRepository<Ontology>>().Context.Query<Action>().FirstOrDefault(a => a.Id == id);

            return this.JsonResult(data);
        }

        /// <summary>
        /// 获取事件主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取事件主题")]
        [Guid("6A23582E-04BA-4DD3-A8FC-4E51D3D19C45")]
        public ActionResult GetTopic(Guid id)
        {
            var data = GetRequiredService<IRepository<Ontology>>().Context.Query<Topic>().FirstOrDefault(a => a.Id == id);

            return this.JsonResult(data);
        }

        /// <summary>
        /// 获取信息组列表
        /// </summary>
        /// <param name="ontologyId"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取信息组列表")]
        [Guid("8A955460-A59E-4A7C-8F8C-CB8A014F38E7")]
        public ActionResult GetInfoGroups(Guid? ontologyId)
        {
            if (!ontologyId.HasValue)
            {
                throw new ValidationException("必须传入本体标识");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体标识" + ontologyId);
            }
            var list = ontology.InfoGroups.ToList();

            return this.JsonResult(new MiniGrid<IInfoGroup> { total = list.Count, data = list });
        }

        /// <summary>
        /// 分页获取信息组列表
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取信息组列表")]
        [Guid("17C868CD-E50A-4343-96E2-8C368D558ECE")]
        public ActionResult GetPlistInfoGroups(Guid? ontologyId, GetPlistResult input)
        {
            if (!ontologyId.HasValue)
            {
                throw new ValidationException("必须传入本体标识");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体标识" + ontologyId);
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "InfoGroup"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.InfoGroup");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的InfoGroup实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = ontology.InfoGroups.AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<IInfoGroup> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 分页获取动作列表
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取动作列表")]
        [Guid("F5BF3E64-CAE8-41EB-80E8-DA0ACD614DB6")]
        public ActionResult GetPlistActions(Guid? ontologyId, GetPlistResult input)
        {
            if (!ontologyId.HasValue)
            {
                throw new ValidationException("必须传入本体标识");
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体标识" + ontologyId);
            }
            EntityTypeState entityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(new Coder("Edi", "Action"), out entityType))
            {
                throw new AnycmdException("意外的实体类型Edi.Action");
            }
            foreach (var filter in input.Filters)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Action实体类型属性" + filter.field);
                }
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = ontology.Actions.Values.Select(ActionTr.Create).AsQueryable();
            foreach (var filter in input.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<ActionTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 分页获取事件主题列表
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("分页获取事件主题列表")]
        [Guid("10062CD6-3A38-4F65-BE64-AACC8EFFB45E")]
        public ActionResult GetPlistTopics(Guid? ontologyId)
        {
            if (!ontologyId.HasValue)
            {
                return this.JsonResult(new MiniGrid<Topic> { total = 0, data = new List<Topic>() });
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("非法的本体标识" + ontologyId);
            }
            var models = ontology.Topics.Values.Select(a => new Topic
            {
                Code = a.Code,
                Id = a.Id,
                IsAllowed = a.IsAllowed,
                OntologyId = a.OntologyId,
                Name = a.Name,
                Description = a.Description
            }).ToList();
            var data = new MiniGrid<Topic> { total = models.Count, data = models };

            return this.JsonResult(data);
        }

        /// <summary>
        /// 获取给定本体的目录
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取给定本体的目录")]
        [Guid("EE184C94-6575-453E-AC29-84FA805D15B4")]
        public ActionResult GetCatalogNodesByParentId(Guid? ontologyId, Guid? parentId)
        {
            if (!ontologyId.HasValue)
            {
                return this.JsonResult(null);
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(ontologyId.Value, out ontology))
            {
                throw new ValidationException("意外的本体标识" + ontologyId);
            }
            string parentCode = null;
            if (parentId.HasValue)
            {
                CatalogState org;
                if (!AcDomain.CatalogSet.TryGetCatalog(parentId.Value, out org))
                {
                    throw new ValidationException("意外的目录标识" + parentId);
                }
                parentCode = org.Code;
            }
            var ontologyOrgDic = ontology.Catalogs;
            var orgs = AcDomain.CatalogSet;
            return this.JsonResult(AcDomain.CatalogSet.Where(a => a != CatalogState.VirtualRoot && string.Equals(a.ParentCode, parentCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Code)
                .Select(a => new
                {
                    a.Id,
                    a.Code,
                    a.Name,
                    ParentId = a.ParentCode,
                    isLeaf = AcDomain.CatalogSet.All(b => !a.Code.Equals(b.ParentCode, StringComparison.OrdinalIgnoreCase)),
                    expanded = false,
                    @checked = ontologyOrgDic.Values.Any(b => b.CatalogId == a.Id),
                    OntologyId = ontologyId.Value
                }).ToList());
        }

        /// <summary>
        /// 获取给定本体目录的动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取给定本体目录的动作")]
        [Guid("7E684D68-3E95-4668-BB19-8BDB508AD986")]
        public ActionResult GetPlistCatalogActions(GetPlistOntologyCatalogActions input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(input.OntologyId, out ontology))
            {
                throw new ValidationException("意外的本体标识" + input.OntologyId);
            }
            CatalogState catalog;
            if (!AcDomain.CatalogSet.TryGetCatalog(input.CatalogId, out catalog))
            {
                throw new ValidationException("意外的目录标识" + input.CatalogId);
            }
            var data = new List<CatalogAssignActionTr>();
            OntologyCatalogState ontologyOrg;
            if (!ontology.Catalogs.TryGetValue(catalog, out ontologyOrg))
            {
                return this.JsonResult(new MiniGrid<CatalogAssignActionTr> { total = 0, data = data });
            }
            IReadOnlyDictionary<Verb, ICatalogAction> actions = ontologyOrg.CatalogActions;
            foreach (var item in AcDomain.NodeHost.Ontologies.GetActons(ontology))
            {
                var action = item.Value;
                Guid id;
                string isAudit;
                string isAllowed;
                if (actions.ContainsKey(item.Key))
                {
                    id = actions[item.Key].Id;
                    isAudit = actions[item.Key].IsAudit;
                    isAllowed = actions[item.Key].IsAllowed;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAudit = AuditType.NotAudit.ToName();
                    isAllowed = AllowType.ExplicitAllow.ToName();
                }
                data.Add(new CatalogAssignActionTr
                {
                    ActionId = action.Id,
                    ActionIsAllowed = action.AllowType.ToName(),
                    ActionIsAudit = action.AuditType.ToName(),
                    Id = id,
                    IsAudit = isAudit,
                    IsAllowed = isAllowed,
                    Name = action.Name,
                    OntologyId = action.OntologyId,
                    CatalogId = input.CatalogId,
                    Verb = action.Verb,
                    OntologyCatalogId = ontologyOrg.Id
                });
            }
            int pageIndex = input.PageIndex;
            int pageSize = input.PageSize;
            var queryable = data.AsQueryable();
            var list = queryable.OrderBy(input.SortField + " " + input.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<CatalogAssignActionTr> { total = queryable.Count(), data = list });
        }

        #region AddOrRemoveCatalogs
        /// <summary>
        /// 添加或移除本体目录
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或移除本体目录")]
        [HttpPost]
        [Guid("407FCC10-8A8E-4EA2-9465-3F321AD9E82A")]
        public ActionResult AddOrRemoveCatalogs(Guid ontologyId, string addCatalogIds, string removeCatalogIds)
        {
            string[] addIDs = addCatalogIds.Split(',');
            string[] removeIDs = removeCatalogIds.Split(',');
            foreach (var item in addIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var catalogId = new Guid(item);
                    AcDomain.Handle(new OntologyCatalogCreateInput
                    {
                        Id = Guid.NewGuid(),
                        OntologyId = ontologyId,
                        CatalogId = catalogId
                    }.ToCommand(AcSession));
                }
            }
            foreach (var item in removeIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var catalogId = new Guid(item);
                    AcDomain.RemoveOntologyCatalog(AcSession, ontologyId, catalogId);
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        // TODO:逻辑移动到应用服务层
        #region AddOrUpdateCatalogActions
        /// <summary>
        /// 添加或更新本体目录级动作权限
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加或更新本体目录级动作权限")]
        [HttpPost]
        [Guid("AEB99E3C-049A-45F0-8996-056255CCBE29")]
        public ActionResult AddOrUpdateCatalogActions()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    var inputModel = new CatalogAction()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        CatalogId = new Guid(row["CatalogId"].ToString()),
                        ActionId = new Guid(row["ActionId"].ToString()),
                        IsAudit = row["IsAudit"].ToString(),
                        IsAllowed = row["IsAllowed"].ToString()
                    };
                    var action = AcDomain.NodeHost.Ontologies.GetAction(inputModel.ActionId);
                    if (action == null)
                    {
                        throw new ValidationException("意外的本体动作标识" + action.Id);
                    }
                    OntologyDescriptor ontology;
                    if (!AcDomain.NodeHost.Ontologies.TryGetOntology(action.Id, out ontology))
                    {
                        throw new ValidationException("意外的动作本体标识" + action.OntologyId);
                    }
                    CatalogState catalog;
                    if (!AcDomain.CatalogSet.TryGetCatalog(inputModel.CatalogId, out catalog))
                    {
                        throw new ValidationException("意外的目录标识");
                    }
                    var ontologyOrgDic = AcDomain.NodeHost.Ontologies.GetOntologyCatalogs(ontology);
                    CatalogAction entity = null;
                    if (ontologyOrgDic.ContainsKey(catalog))
                    {
                        entity = new CatalogAction
                        {
                            ActionId = inputModel.ActionId,
                            IsAllowed = inputModel.IsAllowed,
                            IsAudit = inputModel.IsAudit,
                            Id = inputModel.Id,
                            CatalogId = inputModel.CatalogId
                        };
                        AcDomain.PublishEvent(new CatalogActionUpdatedEvent(AcSession, entity));
                    }
                    else
                    {
                        entity = new CatalogAction();
                        entity.Id = inputModel.Id;
                        entity.CatalogId = inputModel.CatalogId;
                        entity.ActionId = inputModel.ActionId;
                        entity.IsAudit = inputModel.IsAudit;
                        entity.IsAllowed = inputModel.IsAllowed;
                        AcDomain.PublishEvent(new CatalogActionAddedEvent(AcSession, entity));
                    }
                    AcDomain.CommitEventBus();
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        /// <summary>
        /// 获取节点关心的本体
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("获取节点关心的本体")]
        [Guid("7CC8FB86-945C-4BCF-B07E-8965B593D65A")]
        public ActionResult GetNodeOntologyCares(Guid nodeId, GetPlistResult input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            NodeDescriptor node;
            if (!AcDomain.NodeHost.Nodes.TryGetNodeById(nodeId.ToString(), out node))
            {
                throw new ValidationException("意外的节点标识" + nodeId);
            }
            var data = new List<NodeAssignOntologyTr>();
            var nodeOntologyCares = AcDomain.NodeHost.Nodes.GetNodeOntologyCares(node);
            foreach (var ontology in AcDomain.NodeHost.Ontologies)
            {
                var id = Guid.NewGuid();
                var isAssigned = false;
                DateTime? createOn = null;
                var nodeOntologyCare = nodeOntologyCares.FirstOrDefault(a => a.NodeId == nodeId && a.OntologyId == ontology.Ontology.Id);
                if (nodeOntologyCare != null)
                {
                    id = nodeOntologyCare.Id;
                    isAssigned = true;
                    createOn = nodeOntologyCare.CreateOn;
                }
                data.Add(new NodeAssignOntologyTr
                {
                    Code = ontology.Ontology.Code,
                    CreateOn = createOn,
                    Icon = ontology.Ontology.Icon,
                    Id = id,
                    IsAssigned = isAssigned,
                    IsEnabled = ontology.Ontology.IsEnabled,
                    Name = ontology.Ontology.Name,
                    NodeId = nodeId,
                    OntologyId = ontology.Ontology.Id,
                    SortCode = ontology.Ontology.SortCode
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

            return this.JsonResult(new MiniGrid<NodeAssignOntologyTr> { total = queryable.Count(), data = list });
        }

        /// <summary>
        /// 添加信息组
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加信息组")]
        [Resource("InfoGroup")]
        [HttpPost]
        [Guid("E20CBB26-3832-4662-8470-9A1E96011B3E")]
        public ActionResult AddInfoGroup(InfoGroupCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新信息组
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新信息组")]
        [Resource("InfoGroup")]
        [HttpPost]
        [Guid("2E43E657-A81E-44F4-8887-FFBAD549B1CB")]
        public ActionResult UpdateInfoGroup(InfoGroupUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        #region DeleteInfoGroup
        /// <summary>
        /// 删除信息组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除信息组")]
        [Resource("InfoGroup")]
        [HttpPost]
        [Guid("F1DABD8C-8EF6-448B-BD3F-98F2C8441F9E")]
        public ActionResult DeleteInfoGroup(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveInfoGroup, AcSession, id, ',');
        }
        #endregion

        /// <summary>
        /// 添加动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加动作")]
        [Resource("Action")]
        [HttpPost]
        [Guid("477EA0A8-36C2-4AEB-815B-690137807E32")]
        public ActionResult AddAction(ActionCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新动作
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新动作")]
        [Resource("Action")]
        [HttpPost]
        [Guid("0F32F79F-BE08-433A-8CB9-5097291D0040")]
        public ActionResult UpdateAction(ActionUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        #region DeleteAction
        /// <summary>
        /// 删除动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除动作")]
        [Resource("Action")]
        [HttpPost]
        [Guid("5299F593-3164-44B8-8C41-3EF037539033")]
        public ActionResult DeleteAction(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveAction, AcSession, id, ',');
        }
        #endregion

        /// <summary>
        /// 添加事件主题
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("添加事件主题")]
        [Resource("Topic")]
        [HttpPost]
        [Guid("DB618B5A-DFF5-4A22-9D7C-4510C41D5AFE")]
        public ActionResult AddTopic(TopicCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        /// <summary>
        /// 更新事件主题
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新事件主题")]
        [Resource("Topic")]
        [HttpPost]
        [Guid("CE8128C9-9629-4249-8542-B9C532831101")]
        public ActionResult UpdateTopic(TopicUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        #region DeleteTopic
        /// <summary>
        /// 删除事件主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除事件主题")]
        [Resource("Topic")]
        [HttpPost]
        [Guid("328C9EA0-E183-4FF9-9CF6-49D0E3AF752C")]
        public ActionResult DeleteTopic(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveTopic, AcSession, id, ',');
        }
        #endregion

        // TODO:逻辑移动到应用服务层
        #region UpdateOntologies
        /// <summary>
        /// 更新本体配置
        /// </summary>
        /// <returns></returns>
        [By("xuexs")]
        [Description("更新本体配置")]
        [HttpPost]
        [Guid("ACFE007C-DE0D-4D0F-8BA9-6E55511F3E7F")]
        public ActionResult UpdateOntologies()
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
                    var inputModel = new Ontology()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        IsCataloguedEntity = bool.Parse(row["IsCataloguedEntity"].ToString()),
                        IsLogicalDeletionEntity = bool.Parse(row["IsLogicalDeletionEntity"].ToString())
                    };
                    Ontology entity = GetRequiredService<IRepository<Ontology>>().GetByKey(inputModel.Id);
                    if (entity != null)
                    {
                        entity.IsCataloguedEntity = inputModel.IsCataloguedEntity;
                        entity.IsLogicalDeletionEntity = inputModel.IsLogicalDeletionEntity;
                        GetRequiredService<IRepository<Ontology>>().Update(entity);
                        GetRequiredService<IRepository<Ontology>>().Context.Commit();
                        AcDomain.CommitEventBus();
                    }
                    else
                    {
                        throw new AnycmdException("意外的本体");
                    }
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
        [Guid("E136D633-958E-4FF5-8221-112CDC8FBDE3")]
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
                    var inputModel = new NodeOntologyCareCreateInput()
                    {
                        Id = new Guid(row["Id"].ToString()),
                        NodeId = new Guid(row["NodeId"].ToString()),
                        OntologyId = new Guid(row["OntologyId"].ToString())
                    };
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    NodeOntologyCare entity = GetRequiredService<IRepository<NodeOntologyCare>>().GetByKey(inputModel.Id);
                    bool isNew = true;
                    if (entity != null)
                    {
                        isNew = false;
                        if (!isAssigned)
                        {
                            GetRequiredService<IRepository<NodeOntologyCare>>().Remove(entity);
                        }
                    }
                    else
                    {
                        Debug.Assert(inputModel.Id != null, "inputModel.Id != null");
                        entity = new NodeOntologyCare
                        {
                            OntologyId = inputModel.OntologyId,
                            NodeId = inputModel.NodeId,
                            Id = inputModel.Id.Value
                        };
                    }
                    if (isAssigned)
                    {
                        if (isNew)
                        {
                            var count = GetRequiredService<IRepository<NodeOntologyCare>>().AsQueryable().Count(a => a.OntologyId == entity.OntologyId
                                                                                                                     && a.NodeId == entity.NodeId);
                            if (count > 0)
                            {
                                throw new ValidationException("给定的节点已关心给定的本体，无需重复关心");
                            }
                            GetRequiredService<IRepository<NodeOntologyCare>>().Add(entity);
                        }
                        else
                        {
                            GetRequiredService<IRepository<NodeOntologyCare>>().Update(entity);
                        }
                        GetRequiredService<IRepository<NodeOntologyCare>>().Context.Commit();
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除本体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [By("xuexs")]
        [Description("删除本体")]
        [HttpPost]
        [Guid("59E3AA78-A721-4809-88D4-EB5073B9E310")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(AcDomain.RemoveOntology, AcSession, id, ',');
        }
        #endregion
    }
}
