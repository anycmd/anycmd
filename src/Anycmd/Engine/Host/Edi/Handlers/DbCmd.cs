
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;
    using Info;

    /// <summary>
    /// 数据库命令。数据库命令是什么？我们知道一个Command是一个完整的描述了一件事情（事情分Action、Command、Event三种）的对象。
    /// 而并非所有的命令都会深入到数据库层的。比如可以定义一个动作码编码为Audit的命令，服务端收到该类型的命令后随即就转发给了审计系统了，
    /// 这就不涉及数据访问层。DbCommand类似一个筛子，只有需要数据库层处理的命令才会通过这个筛子。
    /// </summary>
    public sealed class DbCmd
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "actionType"></param>
        /// <param name="ontology"></param>
        /// <param name="isDumb"></param>
        /// <param name="client"></param>
        /// <param name="commandId"></param>
        /// <param name="localEntityId"></param>
        /// <param name="infoValue"></param>
        internal DbCmd(DbActionType actionType,
            OntologyDescriptor ontology, bool isDumb, NodeDescriptor client,
            string commandId, string localEntityId, InfoItem[] infoValue)
        {
            this.ActionType = actionType;
            this.Ontology = ontology;
            this.IsDumb = isDumb;
            this.Client = client;
            this.CommandId = commandId;
            this.LocalEntityId = localEntityId;
            this.InfoValue = infoValue;
        }

        /// <summary>
        /// 数据库动作类型。
        /// </summary>
        public DbActionType ActionType { get; private set; }

        /// <summary>
        /// 本体
        /// </summary>
        public OntologyDescriptor Ontology { get; private set; }
        /// <summary>
        /// 是否是哑命令
        /// </summary>
        public bool IsDumb { get; private set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public NodeDescriptor Client { get; private set; }
        /// <summary>
        /// 命令标识
        /// </summary>
        public string CommandId { get; private set; }
        /// <summary>
        /// 本体实体标识
        /// </summary>
        public string LocalEntityId { get; private set; }
        /// <summary>
        /// 信息值元组
        /// </summary>
        public InfoItem[] InfoValue { get; private set; }
    }
}
