
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Ac;
    using Engine.Ac;
    using Engine.Edi;
    using Exceptions;
    using Hecp;
    using Host;
    using Info;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 命令上下文。命令上下文是个什么东西？我们知道，一条命令是一个完整的描述了一次操作的对象。命令对象基本上是扁平的，
    /// 它是通过命令上的本体码、节点标识等关联数据交换运行时上下文的。OntologyCode、NodeID等这些字符串类型和Guid类型的
    /// 信息在CommandContext上会响应的变成对数据交换上下文对象的引用。CommandContext对象上缓存了一些数据，虽然设计的目的
    /// 并非缓存但是它看起来像缓存（有人说走起来像鸭子、叫起来像鸭子、看起来像鸭子的东西就是鸭子。我们不否认这样的说法，
    /// 但要明确的知道我们的目的并非鸭子），从而不至于每次使用的时候都去数据交换宿主上下文中去索引。
    /// </summary>
    public sealed class MessageContext : IStackTrace
    {
        #region private fields
        private TowInfoTuple _towInfoTuple = null;
        private bool _localEntityIdDetected = false;
        private string _localEntityId;
        private bool _catalogCodeDetected = false;
        private string _catalogCode;
        private bool _isValid = false;
        private bool _isValidated = false;
        private bool _isAudit = false;
        private bool _isAuditDetected = false;
        private InfoTuplePair _infoTuple = null;
        private OntologyDescriptor _ontology = null;
        private NodeDescriptor _clientAgent = null;
        private readonly HashSet<WfAct> _acts = new HashSet<WfAct>();
        private int _actsCount = 0;
        private string _stackTrace = null;
        private IStackTraceFormater _stackTraceFormater = null;
        private readonly IAcDomain _acDomain;
        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="command"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessageContext(IAcDomain acDomain, MessageBase command)
        {
            if (command == null)
            {
                this.Exception = new ArgumentNullException("command");
                throw this.Exception;
            }
            this._acDomain = acDomain;
            this.Command = command;
            this.Result = new QueryResult(command) {ResultDataItems = new List<DataItem>()};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="context"></param>
        public static MessageContext Create(IAcDomain acDomain, HecpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var commandContext = new MessageContext(acDomain, AnyMessage.Create(context.Request, acDomain.NodeHost.Nodes.ThisNode));
            foreach (var act in context)
            {
                commandContext.Trace(act);
            }
            return commandContext;
        }
        #endregion

        #region public Properties
        public IAcDomain Host
        {
            get { return _acDomain; }

        }
        /// <summary>
        /// 命令实体。
        /// </summary>
        public MessageBase Command { get; private set; }

        #region TowInfoTuple
        /// <summary>
        /// 数据ID对对象，用以封装两个数据ID。
        /// 在以多列联合信息标识换取单列Guid信息标识的时候需要从数据库中查出两条数据（即Top 2）返回的就是这个类型
        /// </summary>
        internal TowInfoTuple TowInfoTuple
        {
            get
            {
                if (this._towInfoTuple != null) return this._towInfoTuple;
                if (this.Ontology == null || this.InfoTuplePair.IdTuple == null || this.InfoTuplePair.IdTuple.Length == 0)
                {
                    return null;
                }
                IEnumerable<InfoItem> infoIDs = null;
                if (this.InfoTuplePair.IsSingleGuid)
                {
                    infoIDs = new InfoItem[] { InfoItem.Create(this.Ontology.IdElement, this.InfoTuplePair.SingleGuidItem.Value) };
                }
                else if (!string.IsNullOrEmpty(this.LocalEntityId))
                {
                    infoIDs = new InfoItem[] { InfoItem.Create(this.Ontology.IdElement, this.LocalEntityId) };
                }
                else
                {
                    infoIDs = this.InfoTuplePair.IdTuple;
                }
                var selectElements = new OrderedElementSet();
                selectElements.Add(this.Ontology.IdElement);
                selectElements.Add(this.Ontology.EntityActionElement);
                selectElements.Add(this.Ontology.EntityElementActionElement);
                selectElements.Add(this.Ontology.EntityAclPolicyElement);
                if (this.Command.Verb == Verb.Update)
                {
                    foreach (var node in _acDomain.NodeHost.Nodes)
                    {
                        if (node.Node.IsEnabled == 1 && node.Node.IsProduceEnabled && node.IsCareForOntology(this.Ontology))
                        {
                            bool isCare = false;
                            foreach (var item in this.InfoTuplePair.ValueTuple)
                            {
                                if (node.IsCareforElement(item.Element))
                                {
                                    isCare = true;
                                    selectElements.Add(item.Element);
                                }
                            }
                            if (isCare)
                            {
                                foreach (var element in node.GetInfoIdElements())
                                {
                                    selectElements.Add(element);
                                }
                            }
                        }
                    }
                }
                if (this.Ontology.Ontology.IsCataloguedEntity)
                {
                    selectElements.Add(this.Ontology.Elements["ZZJGM"]);
                }

                this._towInfoTuple = this.Ontology.EntityProvider.GetTopTwo(this.Ontology, infoIDs, selectElements);
                return this._towInfoTuple;
            }
        }
        #endregion

        #region IsValid
        /// <summary>
        /// 查看该命令是否合法。
        /// <remarks>
        /// 首先验证输入，接着验证前四级权限，再验证实体的存在性，接着验证目录和实体级权限。
        /// 注意：Level5CatalogAction级和后续级别的验证是以实体的存在性为前提的。
        /// </remarks>
        /// </summary>
        /// <returns>True表示合法，False表示非法</returns>
        internal bool IsValid
        {
            get
            {
                if (this._isValidated) return this._isValid;
                this._isValidated = true;
                #region 输入验证
                var inputValidator = _acDomain.RetrieveRequiredService<IInputValidator>();
                if (inputValidator == null)
                {
                    throw new AnycmdException("没有配置命令输入验证器");
                }
                var result = inputValidator.Validate(this);
                this._isValid = result.IsSuccess;
                if (!this._isValid)
                {
                    this.Result.UpdateStatus(result.StateCode, result.Description);
                    return false;
                }
                #endregion
                var permissionValidator = _acDomain.RetrieveRequiredService<IPermissionValidator>();
                if (permissionValidator == null)
                {
                    throw new AnycmdException("没有配置权限验证器");
                }
                result = permissionValidator.Validate(this);
                this._isValid = result.IsSuccess;
                if (!this._isValid)
                {
                    this.Result.UpdateStatus(result.StateCode, result.Description);
                    return false;
                }
                #region 检验信息标识标识的实体的存在性
                using (var act = new WfAct(_acDomain, this, this.Ontology.EntityProvider, "检验信息标识标识的实体的存在性"))
                {
                    if (this.TowInfoTuple == null)
                    {
                        this.Result.UpdateStatus(Status.NotExist, "给定的信息标识标识的实体记录不存在");
                        this._isValid = false;
                        return false;
                    }
                    if (Verb.Create.Equals(this.Command.Verb))
                    {
                        if (this.TowInfoTuple.HasValue)
                        {
                            this.Result.UpdateStatus(Status.AlreadyExist, "给定的信息标识标识的" + this.Ontology.Ontology.Name + "已经存在");
                            this._isValid = false;
                            return false;
                        }
                    }
                    else
                    {
                        if (this.TowInfoTuple.BothNoValue)
                        {
                            this.Result.UpdateStatus(Status.NotExist, "给定的信息标识标识的" + this.Ontology.Ontology.Name + "不存在");
                            this._isValid = false;
                            return false;
                        }
                        else if (this.TowInfoTuple.BothHasValue)
                        {
                            this.Result.UpdateStatus(Status.UnUnique, "根据给定的信息标识不能唯一标识一个" + this.Ontology.Ontology.Name);
                            this._isValid = false;
                            return false;
                        }
                    }
                }
                if (!this.Ontology.Ontology.IsCataloguedEntity) return this._isValid;
                CatalogState org;
                result = this.ValidCatalogCode(out org);
                this._isValid = result.IsSuccess;
                if (!this._isValid)
                {
                    this.Result.UpdateStatus(result.StateCode, result.Description);
                    return false;
                }
                // Level5CatalogAction
                #region Level5CatalogAction 验证目录级动作权限
                OntologyCatalogState ontologyOrg;
                if (!this.Ontology.Catalogs.TryGetValue(org, out ontologyOrg))
                {
                    this.Result.UpdateStatus(Status.InvalidCatalog, "目录" + org.Name + "/" + org.Code + "对于" + this.Ontology.Ontology.Name + "来说是非法的");
                    this._isValid = false;
                    return false;
                }
                var oorgActions = ontologyOrg.CatalogActions;
                if (oorgActions != null && oorgActions.ContainsKey(this.Command.Verb))
                {
                    var oorgAction = oorgActions[this.Command.Verb];
                    if (oorgAction.AllowType == AllowType.NotAllow)
                    {
                        this.Result.UpdateStatus(Status.NoPermission, "系统已禁止" + this.Ontology.Actions[this.Command.Verb].Name + org.Name + "下的" + this.Ontology.Ontology.Name);
                        this._isValid = false;
                        return false;
                    }
                }
                #endregion

                // Level6EntityAction

                // Level7EntityElementAction

                #endregion

                return this._isValid;
            }
        }
        #endregion

        #region NeedAudit
        /// <summary>
        /// 查看该命令是否需要审计
        /// </summary>
        public bool NeedAudit
        {
            get
            {
                if (!this._isAuditDetected)
                {
                    this._isAuditDetected = true;
                    var auditDiscriminator = _acDomain.RetrieveRequiredService<IAuditDiscriminator>();
                    if (auditDiscriminator == null)
                    {
                        throw new AnycmdException("未配置命令审核鉴别器");
                    }
                    var result = auditDiscriminator.IsNeedAudit(this);
                    this._isAudit = result.IsYes;
                    if (result.IsYes)
                    {
                        this.Result.UpdateStatus(Status.ToAudit, "待审核");
                    }
                }

                return this._isAudit;
            }
        }
        #endregion

        #region ClientAgent
        /// <summary>
        /// 该命令所来自的业务节点。
        /// <remarks>只要当前命令的客户端类型和标识合法就不会返回null，否则可能返回null。</remarks>
        /// </summary>
        public NodeDescriptor ClientAgent
        {
            get
            {
                if (this._clientAgent == null)
                {
                    _acDomain.NodeHost.Nodes.TryGetNodeById(this.Command.ClientId, out this._clientAgent);
                }
                return this._clientAgent;
            }
        }
        #endregion

        #region Ontology
        /// <summary>
        /// 本体元素，当命令的本体元素码非法时会抛出异常。
        /// <remarks>只要当前命令的本体码合法该属性就不会为null，否则可能返回null。</remarks>
        /// </summary>
        public OntologyDescriptor Ontology
        {
            get
            {
                if (this._ontology == null)
                {
                    _acDomain.NodeHost.Ontologies.TryGetOntology(this.Command.Ontology, out this._ontology);
                }
                return this._ontology;
            }
        }
        #endregion

        #region InfoTuplePair
        /// <summary>
        /// 由命令的信息标识元组和信息值元组结合组成的信息元组夫妻。
        /// </summary>
        public InfoTuplePair InfoTuplePair
        {
            get
            {
                if (this._infoTuple == null)
                {
                    if (this.Ontology == null)
                    {
                        this._infoTuple = new InfoTuplePair(null, null);
                        return this._infoTuple;
                    }
                    var elementDic = this.Ontology.Elements;
                    IList<InfoItem> infoIdItems = new List<InfoItem>();
                    IList<InfoItem> infoValueItems = new List<InfoItem>();
                    if (this.Command.DataTuple.IdItems != null)
                    {
                        foreach (var item in this.Command.DataTuple.IdItems.Items)
                        {
                            if (item != null && item.Key != null && elementDic.ContainsKey(item.Key) && elementDic[item.Key].Element.IsEnabled == 1)
                            {
                                infoIdItems.Add(InfoItem.Create(elementDic[item.Key], item.Value));
                            }
                        }
                    }
                    if (Verb.Update.Equals(this.Command.Verb)
                        || Verb.Create.Equals(this.Command.Verb))
                    {
                        if (this.Command.DataTuple.ValueItems != null)
                        {
                            foreach (var item in this.Command.DataTuple.ValueItems.Items)
                            {
                                if (item != null && item.Key != null && elementDic.ContainsKey(item.Key))
                                {
                                    infoValueItems.Add(InfoItem.Create(elementDic[item.Key], item.Value));
                                }
                            }
                        }
                    }
                    this._infoTuple = new InfoTuplePair(infoIdItems.ToArray(), infoValueItems.ToArray());
                }
                return this._infoTuple;
            }
        }
        #endregion

        #region LocalEntityID
        /// <summary>
        /// 查看本地实体标识
        /// </summary>
        internal string LocalEntityId
        {
            get
            {
                if (!this._localEntityIdDetected)
                {
                    this._localEntityIdDetected = true;
                    if (this.Ontology == null)
                    {
                        this._localEntityId = null;
                    }
                    else if (string.IsNullOrEmpty(this.Command.LocalEntityId))
                    {
                        bool thisNodeIsCenterNode = _acDomain.NodeHost.Nodes.ThisNode == _acDomain.NodeHost.Nodes.CenterNode;
                        bool requestNodeIsCenterNode = this.ClientAgent == _acDomain.NodeHost.Nodes.CenterNode;
                        if (thisNodeIsCenterNode || requestNodeIsCenterNode)
                        {
                            if (this.InfoTuplePair.IsSingleGuid)
                            {
                                this.Command.LocalEntityId = this.InfoTuplePair.SingleGuidItem.Value;
                            }
                            else if (Verb.Create.Equals(this.Command.Verb))
                            {
                                this.Command.LocalEntityId = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                if (this.TowInfoTuple != null && !this.TowInfoTuple.BothHasValue && !this.TowInfoTuple.BothNoValue)
                                {
                                    this.Command.LocalEntityId = this.TowInfoTuple.SingleInfoTuple.Single(a => a.Element == this.Ontology.IdElement).Value;
                                }
                            }
                        }
                        else if (Verb.Create.Equals(this.Command.Verb))
                        {
                            this.Command.LocalEntityId = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            if (this.TowInfoTuple != null && !this.TowInfoTuple.BothHasValue && !this.TowInfoTuple.BothNoValue)
                            {
                                this.Command.LocalEntityId = this.TowInfoTuple.SingleInfoTuple.Single(a => a.Element == this.Ontology.IdElement).Value;
                            }
                        }
                        this._localEntityId = this.Command.LocalEntityId;
                    }
                    else
                    {
                        this._localEntityId = this.Command.LocalEntityId;
                    }
                }
                return this._localEntityId;
            }
        }
        #endregion

        #region CatalogCode
        /// <summary>
        /// 查看本地目录码。
        /// <remarks>
        /// 首先，如果当前命令所属的本体不是目录型的本体则返回的是null。
        /// 然后，如果是创建型命令则返回的是客户端输入提供的目录码。
        /// </remarks>
        /// </summary>
        internal string CatalogCode
        {
            get
            {
                if (!this._catalogCodeDetected)
                {
                    this._catalogCodeDetected = true;
                    if (this.Ontology == null)
                    {
                        this._catalogCode = null;
                    }
                    else if (!this.Ontology.Ontology.IsCataloguedEntity)
                    {
                        this._catalogCode = null;
                    }
                    else if (string.IsNullOrEmpty(this.Command.CatalogCode))
                    {
                        if (Verb.Create.Equals(this.Command.Verb))
                        {
                            var orgCode = this.InfoTuplePair.ValueTuple
                                        .Where(a => "zzjgm".Equals(a.Key, StringComparison.OrdinalIgnoreCase))
                                        .Select(a => a.Value).FirstOrDefault();
                            if (!string.IsNullOrEmpty(orgCode))
                            {
                                this.Command.CatalogCode = orgCode;
                            }
                        }
                        else
                        {
                            if (this.TowInfoTuple != null && !this.TowInfoTuple.BothHasValue && !this.TowInfoTuple.BothNoValue)
                            {
                                ElementDescriptor zzjgmElement = this.Ontology.Elements["ZZJGM"];
                                this.Command.CatalogCode = this.TowInfoTuple.SingleInfoTuple.Single(a => a.Element == zzjgmElement).Value;
                            }
                        }
                        this._catalogCode = this.Command.CatalogCode;
                    }
                    else
                    {
                        this._catalogCode = this.Command.CatalogCode;
                    }
                }
                return this._catalogCode;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public QueryResult Result { get; private set; }

        /// <summary>
        /// 异常。命令转移器遇到异常时通过赋值该属性向下传递异常。
        /// </summary>
        public Exception Exception { get; set; }

        #region StackTrace
        /// <summary>
        /// 命令描述对象所经历的处理栈
        /// </summary>
        public string StackTrace
        {
            get
            {
                if (_acts == null || _acts.Count == _actsCount)
                {
                    return _stackTrace;
                }
                if (_stackTraceFormater == null)
                {
                    _stackTraceFormater = _acDomain.RetrieveRequiredService<IStackTraceFormater>();
                }
                _actsCount = _acts.Count;
                _stackTrace = _stackTrace + _stackTraceFormater.Format(_acts);
                return _stackTrace;
            }
            set { _stackTrace = value; }
        }
        #endregion
        #endregion

        /// <summary>
        /// 跟踪活动
        /// </summary>
        /// <param name="act"></param>
        public void Trace(WfAct act)
        {
            this._acts.Add(act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<WfAct> GetEnumerator()
        {
            return ((IEnumerable<WfAct>) _acts).GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _acts.GetEnumerator();
        }

        #region ToDbCommand
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal DbCommand ToDbCommand()
        {
            if (string.IsNullOrEmpty(this.Command.Verb.Code))
            {
                throw new AnycmdException("当前命令的动作码为空");
            }
            DbActionType actionType;
            Verb actionCode = this.Command.Verb;
            if (actionCode == Verb.Create)
            {
                actionType = DbActionType.Insert;
            }
            else if (actionCode == Verb.Update)
            {
                actionType = DbActionType.Update;
            }
            else if (actionCode == Verb.Delete)
            {
                actionType = DbActionType.Delete;
            }
            else
            {
                throw new AnycmdException("无效的数据库动作类型" + this.Command.Verb);
            }
            return new DbCommand(actionType, this.Ontology, this.Command.IsDumb, this.ClientAgent, this.Command.Id.ToString(),
                this.LocalEntityId, this.InfoTuplePair.ValueTuple);
        }
        #endregion

        #region ValidCatalogCode
        private ProcessResult ValidCatalogCode(out CatalogState org)
        {
            if (string.IsNullOrEmpty(this.CatalogCode))
            {
                org = CatalogState.Empty;
                return new ProcessResult(false, Status.InvalidCatalog, "目录码为空");
            }
            if (!_acDomain.CatalogSet.TryGetCatalog(this.CatalogCode, out org))
            {
                return new ProcessResult(false, Status.InvalidCatalog, string.Format("非法的目录码{0}", this.CatalogCode));
            }
            OntologyCatalogState oorg;
            if (!this.Ontology.Catalogs.TryGetValue(org, out oorg))
            {
                return new ProcessResult(false, Status.InvalidCatalog, string.Format("对于{0}来说{1}是非法的目录码", this.Ontology.Ontology.Name, org.Code));
            }
            var orgCode = org.Code;
            return _acDomain.CatalogSet.Any(o => orgCode.Equals(o.ParentCode, StringComparison.OrdinalIgnoreCase)) ? new ProcessResult(false, Status.InvalidCatalog, string.Format("{0}不是叶节点，不能容纳" + this.Ontology.Ontology.Name, org.Name)) : ProcessResult.Ok;

        }
        #endregion
    }
}