
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using Engine.Edi;
    using Exceptions;
    using Hecp;
    using Info;
    using System;

    /// <summary>
    /// 命令分发上下文。单条命令
    /// </summary>
    public sealed class DistributeContext
    {
        private NodeDescriptor _clientAgent;
        private OntologyDescriptor _ontology;
        private readonly IAcDomain _acDomain;

        /// <summary>
        /// 命令分发上下文。单条命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name = "responder"></param>
        public DistributeContext(MessageBase command, NodeDescriptor responder)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (responder == null)
            {
                throw new ArgumentNullException("responder");
            }
            this._acDomain = responder.AcDomain;
            this.Command = command;
            this.Responder = responder;
            #region 如果是head命令类型去掉信息值以减小体积
            DataItem[] infoId = command.DataTuple.IdItems.Items;
            DataItem[] infoValue;
            // 使用ID映射字典转化ID，同时如果是head命令类型去掉信息值以减小体积
            if (Verb.Get.Equals(command.Verb)
                || Verb.Head.Equals(command.Verb))
            {
                infoValue = null;
            }
            else
            {
                infoValue = command.DataTuple.ValueItems.Items;
            }
            #endregion
            this.Result = new QueryResult(command);
        }

        /// <summary>
        /// 待转移命令
        /// </summary>
        public MessageBase Command { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public NodeDescriptor Responder { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public NodeDescriptor ClientAgent
        {
            get
            {
                if (_clientAgent == null)
                {
                    if (this.Command.ClientType == ClientType.Undefined)
                    {
                        throw new AnycmdException("意外的客户端类型" + this.Command.ClientType.ToString());
                    }
                    switch (this.Command.ClientType)
                    {
                        case ClientType.Undefined:
                            throw new AnycmdException("意外的客户端类型");
                        case ClientType.Node:
                            NodeDescriptor node;
                            if (!Ontology.Host.NodeHost.Nodes.TryGetNodeById(this.Command.ClientId, out node))
                            {
                                throw new AnycmdException("意外的请求节点标识" + this.Command.ClientId);
                            }
                            _clientAgent = node;
                            break;
                        case ClientType.App:
                            throw new NotSupportedException("意外的客户端类型");
                        case ClientType.Monitor:
                            throw new NotSupportedException("意外的客户端类型");
                        default:
                            throw new AnycmdException("意外的客户端类型" + this.Command.ClientType.ToString());
                    }
                }
                return _clientAgent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public OntologyDescriptor Ontology
        {
            get
            {
                if (_ontology == null)
                {
                    if (!_acDomain.NodeHost.Ontologies.TryGetOntology(this.Command.Ontology, out _ontology))
                    {
                        throw new AnycmdException("意外的本体码");
                    }
                }
                return _ontology;
            }
        }

        /// <summary>
        /// 异常。命令转移器遇到异常时通过赋值该属性向下传递异常。
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 命令转移反馈结果
        /// </summary>
        public QueryResult Result { get; private set; }
    }
}
