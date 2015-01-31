
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using DataContracts;
    using Hecp;
    using Info;
    using System;

    /// <summary>
    /// 定义命令模型，所有命令必须继承该接口。
    /// <remarks>
    /// 命令完整的描述了一件“事情”，并为“做”提供完整的“输入”。事情分三种：尚未发生、正在发生、已经发生。
    /// 尚未发生的称作Command、正在发生的称作Action、已经发生的称作Event。与其它业务系统对接的时候根据具体需求来正确选择到底是走命令方式还是走事件方式。
    /// </remarks>
    /// <para>
    /// 命令属于操作，是操作实例。即，命令是对“什么”进行“怎样的”操作。
    /// 如“把身份证件号码为4114251988035465的教师的性别修改为男”就是一条命令，但“修改教师”却不是命令因为它没有指定被修改的对象实例。
    /// 为了便于理解，可以将命令与关系数据库的数据操作语句相对应，对于师生数据库数据交换平台来说可以将命令简单的理解为对关系数据库
    /// 数据操作语句的对象级封装，这个说法仅仅是为帮助理解命令概念，两者并非同一概念。命令是对业务操作的描述，是在节点间传递的数据和结构，
    /// 通过一个命令实例可清楚的描述一次操作的具体内容。如果读取一条命令的内容并链接成方便阅读的话的话，则可以表述为：哪一个节点在什么时间
    /// 对哪一个教师或学生进行了什么操作，若该次操作是修改操作，还可以进一步读取到具体被修改的字段以及修改后的新值。
    /// </para>
    /// <para>
    /// 命令的信息标识唯一标识一条记录，命令的执行最多影响一条记录，命令的执行结果要么成功要么失败没有中间态。
    /// 举例：有一条update类型的命令，它要求把信息标识为{Id:’ 672434A4-D4ED-47D2-AFDB-C0FE02CDF14B’}的教师的性别码改为110
    /// （非法性别字典值）和把出生年月改为1984-10-01（合法的），节点接收本条命令时发现性别码是非法的则整条命令就记为失败了，
    /// 不存在性别码更新失败而出生年月更新成功的状态。
    /// </para>
    /// <para>
    /// 命令的非序列性是指：针对不同实体（Entity）的命令之间没有先后顺序；命令的序列性指：针对同一实体（Entity）的命令之间具有先后顺序。
    /// 命令的先后顺序由命令的时间戳指定，该时间戳作为命令传输对象的一部分传输到远程节点，远端节点在整体上按照接收命令的顺序执行命令但在
    /// 局部上按照命令时间戳顺序执行命令，远端节点应保证每一个客户节点发送来的命令均是按照各自的命令时间戳顺序执行的。
    /// </para>
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 版本标识。
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 命令标识。
        /// <remarks>
        /// 命令标识在命令的整个生命周期内是不改变的。命令标识在服务端命令创生时生成之后无论命令处在什么状态它的标识都是不变的。
        /// 借助数据库层来说明这个问题：命令从ReceivedMessage表移到ExecutedMessage表的时候主键是不变的。
        /// </remarks>
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 是否是哑命令。True表示哑的，False非哑的。默认为False。哑命令不会影响服务端实体的状态， 
        /// 但哑命令与对应的非哑命令一样经过相同的流程路径。客户节点程序员可以通过发送哑命令的方式测试对应的非哑命令。
        /// </summary>
        bool IsDumb { get; }

        /// <summary>
        /// 请求类型。必须是Action或Command或Event。Action接收立即执行，Command何时执行由服务端自定但必须执行， 
        /// 如何处理Event由服务端自定，面向EventSourceType、EventSubjectCode、EventStateCode编程。
        /// </summary>
        MessageType MessageType { get; }

        /// <summary>
        /// 请求标识。当MessageType为Event且EventSourceType为Command时这是远端节点向我发送命令时使用的命令消息标识。
        /// 服务端使用该标识来判断自己是否向我发送过命令。
        /// </summary>
        string MessageId { get; }
        /// <summary>
        /// 是一对表达当前消息如何与其它消息关联的值。
        /// </summary>
        string RelatesTo { get; }

        /// <summary>
        /// 
        /// </summary>
        string To { get; }

        /// <summary>
        /// 
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// 
        /// </summary>
        FromData From { get; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        ClientType ClientType { get; }

        /// <summary>
        /// 客户端代理标识。客户端代理是个计算机程序，
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// 本地实体ID
        /// </summary>
        string LocalEntityId { get; }

        /// <summary>
        /// 目录码
        /// </summary>
        string CatalogCode { get; }

        /// <summary>
        /// 接收时间戳
        /// </summary>
        DateTime ReceivedOn { get; }

        /// <summary>
        /// 发生最后一次类型变更时的时间戳
        /// </summary>
        DateTime CreateOn { get; }

        /// <summary>
        /// 命令状态码
        /// </summary>
        int Status { get; }

        /// <summary>
        /// 原因短语
        /// </summary>
        string ReasonPhrase { get; }

        /// <summary>
        /// 命令状态描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 事件主题码。主题码为点号分割的层级结构。 编码为“StateCodeChanged”的主题基本包括命令的所有事件，而“StateCodeChanged.Audit”编码的是审核事件。
        /// </summary>
        string EventSubjectCode { get; }

        /// <summary>
        /// 事件源类型。必须是Command或Entity。Command事件源类型的事件用以告诉远端节点它发送过来的命令在我端的处理状态。 
        /// Entity事件源类型的事件用以告诉远端节点我端的实体发生了某个事件，我端的实体在远端有对应的实体。
        /// </summary>
        string EventSourceType { get; }

        /// <summary>
        /// 命令发起人。对于中心节点该字段记录的是下令者的账户，对于应用节点该字段由应用节点填入，推荐填应用节点的账户标识。
        /// 什么是命令发起人？比如：张三登录一线通节点，在一线通节点更改了自己的手机号码，一线通节点针对这次更改生成了一条
        /// 发送向中心节点的命令，这条命令的“发起人”就是张三。命令发起人通常是应该由应用节点记录在自己本地的，从而如果一条
        /// 命令出问题了（比如违反了什么业务规定，比如张三不是博士，某一条命令把张三改成博士了，后来审计的时候发现这个问题了，UserName就是这个问题的责任人）。
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 命令动作
        /// <example>
        /// create、update、delete
        /// </example>
        /// </summary>
        Verb Verb { get; }

        /// <summary>
        /// 本体编码
        /// <example>教师编码:JS，学生编码:XS</example>
        /// </summary>
        string Ontology { get; }

        /// <summary>
        /// 命令时间戳。
        /// <para>
        /// 本协议出现的时间戳约定为.NET平台下的Utc时间戳，如DateTime.UtcNow.Ticks。请注意：其值是起始于0001-01-01 00:00:00.000的日期和时间的计时周期数，非.NET平台请注意转化。
        /// </para>
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// 数据项集合对。
        /// <remarks>
        /// 命令完整的描述了一件事情并为做提供了完整的输入，而DataTuple字段就是命令的“输入”。
        /// </remarks>
        /// </summary>
        DataItemsTuple DataTuple { get; }
    }
}
