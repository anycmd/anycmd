
namespace Anycmd.Engine.Host.Edi
{
    using Engine.Edi.Abstractions;
    using Handlers;
    using Hecp;
    using System;
    using System.Collections.Generic;

    public interface INodeHost
    {

        IStateCodeSet StateCodes { get; }

        /// <summary>
        /// 本节点数据交换进程上下文。进程列表。
        /// </summary>
        IProcesseSet Processs { get; }

        /// <summary>
        /// 节点上下文
        /// </summary>
        INodeSet Nodes { get; }

        /// <summary>
        /// 信息字典上下文
        /// </summary>
        IInfoDicSet InfoDics { get; }

        /// <summary>
        /// 本体上下文
        /// </summary>
        IOntologySet Ontologies { get; }

        /// <summary>
        /// 信息字符串转化器上下文
        /// </summary>
        IInfoStringConverterSet InfoStringConverters { get; }

        /// <summary>
        /// 信息项验证器上下文
        /// </summary>
        IInfoRuleSet InfoRules { get; }

        /// <summary>
        /// 命令提供程序上下文
        /// </summary>
        IMessageProviderSet MessageProviders { get; }

        /// <summary>
        /// 数据提供程序上下文
        /// </summary>
        IEntityProviderSet EntityProviders { get; }

        IMessageProducer MessageProducer { get; }

        IHecpHandler HecpHandler { get; }

        /// <summary>
        /// 命令转移器
        /// </summary>
        IMessageTransferSet Transfers { get; }

        /// <summary>
        /// 添加请求过滤器, 这些过滤器在Http请求被转化为Hecp请求后应用
        /// </summary>
        List<Func<HecpContext, ProcessResult>> PreHecpRequestFilters { get; }

        /// <summary>
        /// 添加命令过滤器。这些过滤器在Command验证通过但被处理前应用
        /// </summary>
        List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandingFilters { get; }

        /// <summary>
        /// 添加命令过滤器。这些过滤器在Command验证通过并被处理后应用
        /// </summary>
        List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandledFilters { get; }

        /// <summary>
        /// 添加响应过滤器。这些过滤器在Hecp响应末段应用
        /// </summary>
        List<Func<HecpContext, ProcessResult>> GlobalHecpResponseFilters { get; }

        /// <summary>
        /// 应用Hecp管道过滤器，通过返回结果表达当前Hecp请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        ProcessResult ApplyPreHecpRequestFilters(HecpContext context);

        /// <summary>
        /// 应用Command管道过滤器，通过返回结果表达当前Command请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        ProcessResult ApplyEdiMessageHandingFilters(MessageContext context);

        /// <summary>
        /// 应用Command管道过滤器，通过返回结果表达当前Command请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        ProcessResult ApplyEdiMessageHandledFilters(MessageContext context);

        /// <summary>
        /// 应用Hecp管道过滤器，通过返回结果表达当前Hecp请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        ProcessResult ApplyHecpResponseFilters(HecpContext context);
    }
}
