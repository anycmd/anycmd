
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;
    using Exceptions;
    using Hecp;
    using Info;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 待分发命令工厂，工厂同时为多个节点建造命令所以称为工厂，
    /// 而待分发命令建造器只为给定的节点建造命令所以称为建造器
    /// </summary>
    public sealed class DefaultMessageProducer : IMessageProducer
    {
        private readonly Guid _id = new Guid("C35CBF3B-BD0A-49A6-BCDE-039E05402CE0");
        private readonly string _name = "命令工厂";
        private readonly string _description = "命令工厂";

        /// <summary>
        /// 
        /// </summary>
        public DefaultMessageProducer()
        {
        }

        /// <summary>
        /// 资源标识
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 资源类型：CommandFactory
        /// </summary>
        public BuiltInResourceKind BuiltInResourceKind
        {
            get { return BuiltInResourceKind.CommandFactory; }
        }

        /// <summary>
        /// 资源说明
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// 根据给定的成功接收的命令消息创建待分发命令
        /// </summary>
        /// <param name="tuple">成功接收的命令</param>
        /// <returns></returns>
        public IList<MessageEntity> Produce(MessageTuple tuple)
        {
            if (tuple == null)
            {
                throw new ArgumentNullException("tuple");
            }
            var list = new List<MessageEntity>();
            foreach (CommandBuilder builder in this.CommandBuilders(tuple.Context.Host))
            {
                // 不闭合
                if (IsProduce(tuple.Context, builder.ToNode))
                {
                    var result = builder.Build(tuple);
                    if (result != null)
                    {
                        list.Add(result);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="toNode"></param>
        /// <returns></returns>
        public IList<MessageEntity> Produce(MessageTuple tuple, NodeDescriptor toNode)
        {
            if (tuple == null)
            {
                throw new ArgumentNullException("tuple");
            }
            if (toNode == null)
            {
                throw new ArgumentNullException("toNode");
            }
            if (!IsProduce(tuple.Context, toNode))
            {
                return new List<MessageEntity>();
            }
            var list = new List<MessageEntity>();
            var builder = new CommandBuilder(toNode);
            var result = builder.Build(tuple);
            if (result != null)
            {
                list.Add(result);
            }

            return list;
        }

        public bool IsProduce(MessageContext context, NodeDescriptor toNode)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (toNode == null)
            {
                throw new ArgumentNullException("toNode");
            }
            if (toNode.Node.IsEnabled != 1
                || toNode.Node.IsProduceEnabled == false
                || !toNode.IsCareForOntology(context.Ontology)
                || (context.Command.Verb == Verb.Update
                    && context.InfoTuplePair.ValueTuple.All(a => !toNode.IsCareforElement(a.Element))))
            {
                return false;
            }
            return true;
        }

        private IEnumerable<CommandBuilder> CommandBuilders(IAcDomain acDomain)
        {
            foreach (var node in acDomain.NodeHost.Nodes)
            {
                if (node != acDomain.NodeHost.Nodes.ThisNode)
                {
                    yield return new CommandBuilder(node);
                }
            }
        }

        /// <summary>
        /// 待分发命令建造器，待分发命令建造器负责为设定的节点建造命令
        /// </summary>
        private sealed class CommandBuilder
        {
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="toNode">向该节点建造待分发命令</param>
            public CommandBuilder(NodeDescriptor toNode)
            {
                this.ToNode = toNode;
            }

            /// <summary>
            /// 向该节点建造待分发命令
            /// </summary>
            public NodeDescriptor ToNode { get; private set; }

            /// <summary>
            /// 根据给定的接收成功的命令建造待分发命令
            /// </summary>
            /// <param name="tuple">已成功接收的命令类型的命令描述对象</param>
            /// <exception cref="AnycmdException">
            /// 当<seealso cref="ToNode"/>是自己时发生，不能建造分发向自己的命令消息
            /// </exception>
            /// <returns>待分发命令集合，可能为null</returns>
            public MessageEntity Build(MessageTuple tuple)
            {
                if (tuple == null)
                {
                    throw new ArgumentNullException("tuple");
                }
                #region 演出
                if (ToNode == ToNode.AcDomain.NodeHost.Nodes.ThisNode)
                {
                    throw new AnycmdException("不能建造分发向自己的命令消息");
                }
                if (tuple.Context.ClientAgent == ToNode)
                {
                    throw new AnycmdException("不能基于来源节点的命令建造分发向来源节点的命令");
                }
                var actionCode = tuple.Context.Command.Verb;
                if (actionCode == Verb.Create || actionCode == Verb.Update)
                {
                    var infoValueCares = new List<DataItem>();
                    foreach (var item in tuple.Context.InfoTuplePair.ValueTuple)
                    {
                        if (item.Element != tuple.Context.Ontology.IdElement
                            && !item.Element.IsRuntimeElement
                            && ToNode.IsCareforElement(item.Element))
                        {
                            infoValueCares.Add(new DataItem(item.Key, item.Value));
                        }
                    }
                    DataItem[] infoIdItems = null;
                    if (tuple.Tuple == null
                        || ToNode.GetInfoIdElements().Any(a =>
                            a != tuple.Context.Ontology.IdElement
                            && a.Element.IsInfoIdItem
                            && !tuple.Tuple.Any(b => a.Element.Code.Equals(b.Key, StringComparison.OrdinalIgnoreCase))))
                    {
                        if (actionCode == Verb.Create)
                        {
                            var selectElements = new OrderedElementSet();
                            foreach (var item in ToNode.GetInfoIdElements())
                            {
                                if (item.Element.IsInfoIdItem)
                                {
                                    selectElements.Add(item);
                                }
                            }
                            infoIdItems = tuple.Context.Ontology.EntityProvider.GetTopTwo(tuple.Context.Ontology,
                                new InfoItem[] { InfoItem.Create(tuple.Context.Ontology.IdElement, tuple.Context.LocalEntityId) },
                                selectElements).SingleInfoTuple.Select(e => new DataItem(e.Key, e.Value)).ToArray();
                        }
                        else
                        {
                            infoIdItems = tuple.Context.TowInfoTuple.SingleInfoTuple
                                .Where(e => e.Element.Element.IsInfoIdItem)
                                .Select(e => new DataItem(e.Key, e.Value)).ToArray();
                        }
                    }
                    else
                    {
                        infoIdItems = tuple.Tuple.Where(e => e.Element.Element.IsInfoIdItem && ToNode.IsInfoIdElement(e.Element)).ToArray();
                    }
                    DataItemsTuple dataTuple = DataItemsTuple.Create(
                        ToNode.AcDomain,
                        infoIdItems,
                        infoValueCares.ToArray(),
                        tuple.Context.Command.DataTuple.QueryList,
                        tuple.Context.Command.DataTuple.InfoFormat);

                    if (infoValueCares.Count > 0)
                    {
                        return new MessageEntity(MessageTypeKind.Distribute, Guid.NewGuid(), dataTuple)
                        {
                            ClientId = ToNode.Node.Id.ToString(),
                            Verb = tuple.Context.Command.Verb,
                            LocalEntityId = tuple.Context.LocalEntityId,
                            CatalogCode = tuple.Context.CatalogCode,
                            ReceivedOn = tuple.Context.Command.ReceivedOn,
                            CreateOn = DateTime.Now,
                            Ontology = tuple.Context.Command.Ontology,
                            ClientType = tuple.Context.Command.ClientType,
                            TimeStamp = tuple.Context.Command.TimeStamp,
                            MessageType = tuple.Context.Command.MessageType,
                            MessageId = tuple.Context.Command.MessageId,
                            Status = tuple.Context.Result.Status,
                            ReasonPhrase = tuple.Context.Result.ReasonPhrase,
                            Description = tuple.Context.Result.Description,
                            EventSourceType = tuple.Context.Command.EventSourceType,
                            EventSubjectCode = tuple.Context.Command.EventSubjectCode,
                            UserName = tuple.Context.Command.UserName,
                            IsDumb = tuple.Context.Command.IsDumb,
                            Version = tuple.Context.Command.Version
                        };
                    }
                }
                else if (actionCode == Verb.Delete)
                {
                    if (ToNode.IsCareForOntology(tuple.Context.Ontology))
                    {
                        DataItem[] infoIdItems = tuple.Context.TowInfoTuple.SingleInfoTuple
                            .Where(e => e.Element.Element.IsInfoIdItem)
                            .Select(e => new DataItem(e.Key, e.Value)).ToArray();
                        DataItemsTuple dataTuple = DataItemsTuple.Create(ToNode.AcDomain, infoIdItems, null, tuple.Context.Command.DataTuple.QueryList, tuple.Context.Command.DataTuple.InfoFormat);
                        return new MessageEntity(MessageTypeKind.Distribute, Guid.NewGuid(), dataTuple)
                        {
                            ClientId = ToNode.Node.Id.ToString(),
                            Verb = tuple.Context.Command.Verb,
                            LocalEntityId = tuple.Context.LocalEntityId,
                            CatalogCode = tuple.Context.CatalogCode,
                            ReceivedOn = tuple.Context.Command.ReceivedOn,
                            CreateOn = DateTime.Now,
                            Ontology = tuple.Context.Command.Ontology,
                            ClientType = tuple.Context.Command.ClientType,
                            TimeStamp = tuple.Context.Command.TimeStamp,
                            MessageType = tuple.Context.Command.MessageType,
                            MessageId = tuple.Context.Command.MessageId,
                            Status = tuple.Context.Result.Status,
                            ReasonPhrase = tuple.Context.Result.ReasonPhrase,
                            Description = tuple.Context.Result.Description,
                            EventSubjectCode = tuple.Context.Command.EventSubjectCode,
                            EventSourceType = tuple.Context.Command.EventSourceType,
                            UserName = tuple.Context.Command.UserName,
                            IsDumb = tuple.Context.Command.IsDumb,
                            Version = tuple.Context.Command.Version
                        };
                    }
                }
                #endregion

                return null;
            }
        }
    }
}
