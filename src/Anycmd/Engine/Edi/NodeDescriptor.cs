
namespace Anycmd.Engine.Edi
{
    using Exceptions;
    using Hecp;
    using Host.Ac;
    using Host.Edi;
    using Host.Edi.Handlers;
    using Host.Edi.Handlers.Distribute;
    using Logging;
    using System;
    using System.Collections.Generic;
    using Util;

    public sealed class NodeDescriptor
    {
        private bool _isNetException = false;
        private bool _serviceIsNotAlive = false;
        private DateTime _lastIsAliveRequestOn = SystemTime.MinDate;
        private IMessageTransfer _transfer = null;

        private static readonly object Locker = new object();
        private readonly IAcDomain _acDomain;

        #region Ctor
        public NodeDescriptor(IAcDomain acDomain, NodeState node)
        {
            this._acDomain = acDomain;
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            this.Node = node;
        }
        #endregion

        #region Public Properties
        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get { return this.Node.Id; } }

        /// <summary>
        /// 节点模型
        /// </summary>
        public NodeState Node { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IMessageTransfer Transfer
        {
            get
            {
                if (_transfer == null)
                {
                    if (!AcDomain.NodeHost.Transfers.TryGetTransfer(this.Node.TransferId, out _transfer))
                    {
                        throw new AnycmdException("意外的命令转移器");
                    }
                }
                return _transfer;
            }
        }

        /// <summary>
        /// 查看服务节点是否网络异常。读取此属性没有性能损失。
        /// </summary>
        public bool IsNetException
        {
            get
            {
                IsAlive();
                return _isNetException;
            }
            set { _isNetException = value; }
        }

        /// <summary>
        /// 服务是否可用。读取此属性没有性能损失。
        /// </summary>
        public bool ServiceIsNotAlive
        {
            get
            {
                IsAlive();
                return _serviceIsNotAlive;
            }
            set { _serviceIsNotAlive = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BeatPeriod
        {
            get
            {
                int m = this.AcDomain.Config.BeatPeriod;
                if (this.Node.BeatPeriod.HasValue)
                {
                    m = this.Node.BeatPeriod.Value;
                }
                return m;
            }
        }

        public string Name
        {
            get { return this.Node.Name; }
        }

        public string AnycmdApiAddress
        {
            get { return this.Node.AnycmdApiAddress; }
        }

        public string AnycmdWsAddress
        {
            get { return this.Node.AnycmdWsAddress; }
        }

        public bool IsEnabled
        {
            get { return this.Node.IsEnabled == 1; }
        }

        public string PublicKey
        {
            get { return this.Node.PublicKey; }
        }

        public string SecretKey
        {
            get { return this.Node.SecretKey; }
        }

        public bool IsDistributeEnabled
        {
            get { return this.Node.IsDistributeEnabled; }
        }

        public ClientType ClientType
        {
            get { return ClientType.Node; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ElementDescriptor> GetInfoIdElements()
        {
            return AcDomain.NodeHost.Nodes.GetInfoIdElements(this);
        }

        public bool IsInfoIdElement(ElementDescriptor element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return AcDomain.NodeHost.Nodes.IsInfoIdElement(this, element);
        }

        #region IsCareforElement
        /// <summary>
        /// 判断本节点是否关心给定的本体元素。
        /// <remarks>关心本体元素的前提是关心了元素所属的本体，否则必返回false表示不关心。</remarks>
        /// </summary>
        /// <param name="element">本体元素码</param>
        /// <returns>True表示关心，False表示不关心</returns>
        public bool IsCareforElement(ElementDescriptor element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return AcDomain.NodeHost.Nodes.IsCareforElement(this, element);
        }
        #endregion

        #region IsCareForOntology
        /// <summary>
        /// 判断本节点是否关心给定的本体
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns>True表示关心，False表示不关心</returns>
        public bool IsCareForOntology(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            return AcDomain.NodeHost.Nodes.IsCareForOntology(this, ontology);
        }
        #endregion

        #region GetOntologyPermission
        /// <summary>
        /// 判断本节点是否具有对给定的本体执行给定的类型的动作的权限
        /// </summary>
        /// <param name="ontology">本体</param>
        /// <param name="actionCode">动作码</param>
        /// <returns>True表示有权限，False表示无权限</returns>
        public AllowType GetOntologyPermission(OntologyDescriptor ontology, Verb actionCode)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (actionCode == null)
            {
                throw new ArgumentNullException("actionCode");
            }
            if (!ontology.Actions.ContainsKey(actionCode))
            {
                throw new AnycmdException("非法的动作码");
            }
            var nodeActions = this.Node.NodeActions;
            if (!nodeActions.ContainsKey(ontology) || !nodeActions[ontology].ContainsKey(actionCode))
            {
                return AllowType.NotAllow;
            }

            return nodeActions[ontology][actionCode].AllowType;
        }
        #endregion

        #region GetElementPermission
        /// <summary>
        /// 判断本节点是否具有对给定的本体元素执行给定的类型的动作的权限
        /// </summary>
        /// <param name="element">本体元素码</param>
        /// <param name="actionCode">动作码</param>
        /// <returns>True表示有权限，False表示无权限</returns>
        public AllowType GetElementPermission(ElementDescriptor element, Verb actionCode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (actionCode == null)
            {
                throw new ArgumentNullException("actionCode");
            }
            if (element == element.Ontology.IdElement)
            {
                return AllowType.ExplicitAllow;
            }
            var nodeElementActions = element.GetActions(this);
            if (!nodeElementActions.ContainsKey(actionCode))
            {
                return AllowType.NotAllow;
            }
            if (nodeElementActions[actionCode].IsAllowed)
            {
                return AllowType.ImplicitAllow;
            }
            else
            {
                return AllowType.NotAllow;
            }
        }
        #endregion

        #region GetOntologyAudit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        public AuditType GetOntologyAudit(OntologyDescriptor ontology, Verb actionCode)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (actionCode == null)
            {
                throw new ArgumentNullException("actionCode");
            }
            var nodeActions = this.Node.NodeActions;
            if (!nodeActions.ContainsKey(ontology) || !nodeActions[ontology].ContainsKey(actionCode))
            {
                return AuditType.NotAudit;
            }

            return nodeActions[ontology][actionCode].AuditType;
        }
        #endregion

        #region GetElementAudit
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        public AuditType GetElementAudit(ElementDescriptor element, Verb actionCode)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (actionCode == null)
            {
                throw new ArgumentNullException("actionCode");
            }
            var nodeElementActions = element.GetActions(this);
            if (!nodeElementActions.ContainsKey(actionCode))
            {
                return AuditType.NotAudit;
            }
            return nodeElementActions[actionCode].IsAudit ? AuditType.ImplicitAudit : AuditType.NotAudit;

        }
        #endregion

        #region Private Method IsAlive
        private void IsAlive()
        {
            lock (Locker)
            {
                if (_lastIsAliveRequestOn.AddMinutes(this.BeatPeriod) < DateTime.Now)
                {
                    var isAliveContext = new BeatContext(this, BeatRequest.V1Request);

                    this.Transfer.IsAlive(isAliveContext);

                    if (isAliveContext.Exception != null)
                    {
                        this.IsNetException = true;
                        this.ServiceIsNotAlive = true;
                        var anyLog = new AnyLog(Guid.NewGuid())
                        {
                            Req_Ontology = "IsAlive",
                            Req_Verb = string.Empty,
                            Req_ClientId = string.Empty,
                            Req_ClientType = string.Empty,
                            CreateOn = DateTime.Now,
                            Req_Description = string.Empty,
                            Req_EventSourceType = string.Empty,
                            Req_EventSubjectCode = string.Empty,
                            InfoFormat = "json",
                            Req_InfoId = string.Empty,
                            Req_InfoValue = string.Empty,
                            Req_UserName = string.Empty,
                            Req_IsDumb = false,
                            LocalEntityId = string.Empty,
                            CatalogCode = string.Empty,
                            Req_ReasonPhrase = Status.NodeException.ToName(),
                            ReceivedOn = DateTime.Now,
                            Req_MessageId = string.Empty,
                            Req_MessageType = string.Empty,
                            Req_QueryList = string.Empty,
                            Req_Status = (int)Status.NodeException,
                            Req_TimeStamp = DateTime.Now,
                            Req_Version = isAliveContext.Request.Version,
                            Res_InfoValue = string.Empty,
                            Res_Description = isAliveContext.Exception.Message,
                            Res_ReasonPhrase = Status.NodeException.ToName(),
                            Res_StateCode = (int)Status.NodeException
                        };
                        AcDomain.LoggingService.Log(anyLog);
                    }
                    else
                    {
                        this.ServiceIsNotAlive = !isAliveContext.Response.IsAlive;
                    }
                }
                _lastIsAliveRequestOn = DateTime.Now;
            }
        }
        #endregion

        public override int GetHashCode()
        {
            return this.Node.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is NodeDescriptor))
            {
                return false;
            }
            var left = this;
            var right = (NodeDescriptor)obj;

            return left.Node == right.Node;
        }

        public static bool operator ==(NodeDescriptor a, NodeDescriptor b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(NodeDescriptor a, NodeDescriptor b)
        {
            return !(a == b);
        }
    }
}
