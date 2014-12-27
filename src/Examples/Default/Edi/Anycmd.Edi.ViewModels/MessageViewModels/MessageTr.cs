
namespace Anycmd.Edi.ViewModels.MessageViewModels
{
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Hecp;
    using Engine.Host.Edi.Handlers;
    using Engine.Info;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Util;

    /// <summary>
    /// 实现命令视图模型的抽象泛型基类
    /// </summary>
    public class MessageTr : IMessageView
    {
        private MessageEntity _command;
        private string _clientName = null;
        private string _ontologyName = null;
        OntologyDescriptor _ontology;
        private string _actionName = null;
        private string _organizationName = null;
        private IList<InfoItem> _infoValueItems;
        private static readonly List<InfoItem> EmptyInfoValueItems = new List<InfoItem>();
        private string _commandInfo = null;
        private bool _isSelfDetected = false;
        private bool _isSelf = false;
        private bool _isCenterDetected = false;
        private bool _isCenter = false;
        private readonly IAcDomain _host;

        protected internal MessageTr(IAcDomain host)
        {
            this._host = host;
        }

        /// <summary>
        /// 板上组装。提供给子类实现的模板方法。
        /// </summary>
        /// <param name="command"></param>
        protected virtual void PopulateCore(MessageEntity command)
        {

        }

        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static MessageTr Create(IAcDomain host, MessageEntity command)
        {
            var t = new MessageTr(host);
            t.Populate(command);

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelf
        {
            get
            {
                if (!_isSelfDetected)
                {
                    _isSelfDetected = true;
                    _isSelf = _host.Config.ThisNodeId.Equals(this.ClientId, StringComparison.OrdinalIgnoreCase);
                }
                return _isSelf;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCenter
        {
            get
            {
                if (!_isCenterDetected)
                {
                    _isCenterDetected = true;
                    _isCenter = _host.Config.CenterNodeId.Equals(this.ClientId, StringComparison.OrdinalIgnoreCase);
                }
                return _isCenter;
            }
        }

        public Guid Id { get; private set; }
        public string MessageId { get; private set; }
        public string MessageType { get; private set; }
        public string ClientType { get; private set; }
        public string ClientId { get; private set; }
        public string Verb { get; private set; }
        public string InfoFormat { get; private set; }
        public string InfoValue { get; private set; }
        public string Ontology { get; private set; }
        public string OrganizationCode { get; private set; }
        public string InfoId { get; private set; }
        public string LocalEntityId { get; private set; }
        public DateTime ReceivedOn { get; set; }
        public DateTime TimeStamp { get; private set; }
        public string UserName { get; private set; }
        public int StateCode { get; private set; }
        public string ReasonPhrase { get; private set; }
        public string Description { get; private set; }

        public string EventSubjectCode { get; set; }
        public string EventSourceType { get; set; }

        /// <summary>
        /// 账户标识
        /// </summary>
        public string Principal { get; set; }
        public DateTime CreateOn { get; private set; }

        public string OrganizationName
        {
            get
            {
                if (_organizationName == null)
                {
                    OrganizationState org;
                    if (_host.OrganizationSet.TryGetOrganization(this.OrganizationCode, out org))
                    {
                        _organizationName = org.Name;
                    }
                    else
                    {
                        _organizationName = string.Empty;
                    }
                    if (_organizationName == null)
                    {
                        _organizationName = string.Empty;
                    }
                }
                return _organizationName;
            }
        }

        #region ClientName
        public string ClientName
        {
            get
            {
                if (_clientName == null)
                {
                    NodeDescriptor node;
                    if (_host.NodeHost.Nodes.TryGetNodeById(this.ClientId, out node))
                    {
                        _clientName = node.Node.Name;
                    }
                    else
                    {
                        _clientName = string.Empty;
                    }
                }
                return _clientName;
            }
        }
        #endregion

        #region OntologyName
        public string OntologyName
        {
            get
            {
                if (_ontologyName == null)
                {
                    _ontologyName = this.OntologyDescriptor.Ontology.Name;
                }
                return _ontologyName;
            }
        }
        #endregion

        public string ActionName
        {
            get
            {
                if (_actionName == null)
                {
                    ActionState action;
                    if (!this.OntologyDescriptor.Actions.TryGetValue(new Verb(this.Verb), out action))
                    {
                        throw new AnycmdException("意外的" + this.OntologyName + "动作码" + this.Verb);
                    }
                    _actionName = action.Name;
                }
                return _actionName;
            }
        }

        #region HumanInfo
        /// <summary>
        /// 命令信息，经过翻译的
        /// </summary>
        public string HumanInfo
        {
            get
            {
                if (_commandInfo == null)
                {
                    var sb = new StringBuilder();
                    sb.Append(this.ActionName);
                    sb.Append("：");
                    int l = sb.Length;
                    foreach (var item in InfoValueItems)
                    {
                        if (sb.Length != l)
                        {
                            sb.Append(";");
                        }
                        sb.Append(item.Element.Element.Name)
                            .Append("=>").Append(item.Element.TranslateValue(item.Value));
                    }
                    _commandInfo = sb.ToString();
                }
                return _commandInfo;
            }
            set
            {
                _commandInfo = value;
            }
        }
        #endregion

        private OntologyDescriptor OntologyDescriptor
        {
            get
            {
                if (_ontology != null) return _ontology;
                if (!_host.NodeHost.Ontologies.TryGetOntology(this.Ontology, out _ontology))
                {
                    throw new AnycmdException("意外的本体码" + this.Ontology);
                }
                return _ontology;
            }
        }

        /// <summary>
        /// 根据传入的命令对象组装命令展示对象
        /// </summary>
        /// <param name="command"></param>
        private void Populate(MessageEntity command)
        {
            this._command = command;
            this.Id = command.Id;
            this.MessageId = command.MessageId;
            this.MessageType = command.MessageType.ToName();
            this.Verb = command.Verb.Code;
            this.InfoFormat = command.DataTuple.InfoFormat;
            this.InfoValue = command.DataTuple.ValueItems.InfoString;
            this.Ontology = command.Ontology;
            this.OrganizationCode = command.OrganizationCode;
            this.InfoId = command.DataTuple.IdItems.InfoString;
            this.LocalEntityId = command.LocalEntityId;
            this.ClientType = command.ClientType.ToName();
            this.ClientId = command.ClientId;
            this.ReceivedOn = command.ReceivedOn;
            this.TimeStamp = command.TimeStamp;
            this.CreateOn = command.CreateOn;
            this.UserName = command.UserName;
            this.StateCode = command.Status;
            this.ReasonPhrase = command.ReasonPhrase;
            this.Description = command.Description;
            this.Principal = command.UserName;
            this.EventSubjectCode = command.EventSubjectCode;
            this.EventSourceType = command.EventSourceType;
            this.PopulateCore(command);
        }

        #region private InfoValueItems
        private IList<InfoItem> InfoValueItems
        {
            get
            {
                if (_infoValueItems == null)
                {
                    OntologyDescriptor ontology;
                    if (!_host.NodeHost.Ontologies.TryGetOntology(this.Ontology, out ontology))
                    {
                        return EmptyInfoValueItems;
                    }
                    _infoValueItems = new List<InfoItem>();
                    foreach (var item in this._command.DataTuple.ValueItems.Items)
                    {
                        _infoValueItems.Add(InfoItem.Create(ontology.Elements[item.Key], item.Value));
                    }
                }

                return _infoValueItems;
            }
            set
            {
                _infoValueItems = value;
            }
        }
        #endregion
    }
}
