
namespace Anycmd.Logging
{
    using Model;
    using System;
    using System.Data;

    /// <summary>
    /// AnyLog不是为普通用户设计的，而是为程序员设计的。
    /// AnyLog使用一种固定的结构和程序员明白的词语完整的描述一件事情。
    /// 它的结构等于AnyCommandRequestData + AnyCommandResponseData。
    /// 更多信息参考数据交换接口文档。
    /// </summary>
    public interface IAnyLog : IEntity
    {
        /// <summary>
        /// 机器
        /// </summary>
        string Machine { get; }
        /// <summary>
        /// 进程
        /// </summary>
        string Process { get; }
        /// <summary>
        /// 基地址
        /// </summary>
        string BaseDirectory { get; }
        /// <summary>
        /// 动态地址
        /// </summary>
        string DynamicDirectory { get; }
        /// <summary>
        /// 请求版本号
        /// </summary>
        string Req_Version { get; }

        /// <summary>
        /// 是哑的。True表示哑的，False非哑的。
        /// </summary>
        bool Req_IsDumb { get; }

        /// <summary>
        /// 请求类型。必须是Action或Command或Event。Action接收立即执行，Command接收后周期执行， 如何处理Event由服务端自定。
        /// 面向EventSourceType、EventSubjectCode、EventStateCode/EventReasonPhrase编程。
        /// </summary>
        string Req_MessageType { get; }

        /// <summary>
        /// 请求标识。当Type为Event且EventSourceType为Command时这是远端节点向我分发命令时使用的MessageID。
        /// </summary>
        string Req_MessageId { get; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        string Req_ClientType { get; }

        /// <summary>
        /// 客户标识
        /// </summary>
        string Req_ClientId { get; }

        /// <summary>
        /// 本地实体ID
        /// </summary>
        string LocalEntityId { get; }

        /// <summary>
        /// 目录码
        /// </summary>
        string OrganizationCode { get; }

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
        int Req_Status { get; }

        /// <summary>
        /// 原因短语
        /// </summary>
        string Req_ReasonPhrase { get; }

        /// <summary>
        /// 命令状态描述
        /// </summary>
        string Req_Description { get; }

        /// <summary>
        /// 事件主题码。主题码为点号分割的层级结构。 编码为“StateCodeChanged”的主题基本包括命令的所有事件，而“StateCodeChanged.Audit”编码的是审核事件。
        /// </summary>
        string Req_EventSubjectCode { get; }

        /// <summary>
        /// 事件源类型。必须是Command或Entity。Command事件源类型的事件用以告诉远端节点它发送过来的命令在我端的处理状态。 
        /// Entity事件源类型的事件用以告诉远端节点我端的实体发生了某个事件，我端的实体在远端有对应的实体。
        /// </summary>
        string Req_EventSourceType { get; }

        /// <summary>
        /// 命令发起人。对于中心节点该字段记录的是下令者的账户，对于应用节点该字段由应用节点填入，推荐填应用节点的账户标识。
        /// 什么是命令发起人？比如：张三登录一线通节点，在一线通节点更改了自己的手机号码，一线通节点针对这次更改生成了一条
        /// 发送向中心节点的命令，这条命令的“发起人”就是张三。命令发起人通常是应该由应用节点记录在自己本地的，从而如果一条
        /// 命令出问题了（比如违反了什么业务规定，比如张三不是博士，某一条命令把张三改成博士了，后来审计的时候发现这个问题了，UserName就是这个问题的责任人）。
        /// </summary>
        string Req_UserName { get; }

        /// <summary>
        /// 命令动作
        /// <example>
        /// create、update、delete
        /// </example>
        /// </summary>
        string Req_Verb { get; }

        /// <summary>
        /// 本体编码
        /// <example>教师编码:JS，学生编码:XS</example>
        /// </summary>
        string Req_Ontology { get; }

        /// <summary>
        /// 命令时间戳。
        /// <para>
        /// 本协议出现的时间戳约定为.NET平台下的Utc时间戳，如DateTime.UtcNow.Ticks。请注意：其值是起始于0001-01-01 00:00:00.000的日期和时间的计时周期数，非.NET平台请注意转化。
        /// </para>
        /// </summary>
        DateTime Req_TimeStamp { get; }

        /// <summary>
        /// 
        /// </summary>
        string Req_QueryList { get; }

        /// <summary>
        /// 
        /// </summary>
        string InfoFormat { get; }

        /// <summary>
        /// 
        /// </summary>
        string Req_InfoId { get; }

        /// <summary>
        /// 
        /// </summary>
        string Req_InfoValue { get; }
        /// <summary>
        /// 
        /// </summary>
        int Res_StateCode { get; }
        /// <summary>
        /// 
        /// </summary>
        string Res_ReasonPhrase { get; }
        /// <summary>
        /// 
        /// </summary>
        string Res_Description { get; }
        /// <summary>
        /// 
        /// </summary>
        string Res_InfoValue { get; }

        DataRow ToDataRow(DataTable dt);
    }
}
