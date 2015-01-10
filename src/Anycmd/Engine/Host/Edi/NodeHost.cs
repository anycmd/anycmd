
namespace Anycmd.Engine.Host.Edi
{
    using Engine.Edi.Abstractions;
    using Handlers;
    using Hecp;
    using System;
    using System.Collections.Generic;

    public abstract class NodeHost : INodeHost
    {
        protected NodeHost()
        {
            this.PreHecpRequestFilters = new List<Func<HecpContext, ProcessResult>>();
            this.GlobalEdiMessageHandingFilters = new List<Func<MessageContext, ProcessResult>>();
            this.GlobalEdiMessageHandledFilters = new List<Func<MessageContext, ProcessResult>>();
            this.GlobalHecpResponseFilters = new List<Func<HecpContext, ProcessResult>>();
        }

        public IStateCodeSet StateCodes { get; protected set; }

        /// <summary>
        /// 本节点数据交换进程上下文。进程列表。
        /// </summary>
        public IProcesseSet Processs { get; protected set; }

        /// <summary>
        /// 节点上下文
        /// </summary>
        public INodeSet Nodes { get; protected set; }

        /// <summary>
        /// 信息字典上下文
        /// </summary>
        public IInfoDicSet InfoDics { get; protected set; }

        /// <summary>
        /// 本体上下文
        /// </summary>
        public IOntologySet Ontologies { get; protected set; }

        public IHecpHandler HecpHandler { get; protected set; }

        /// <summary>
        /// 信息字符串转化器上下文
        /// </summary>
        public IInfoStringConverterSet InfoStringConverters { get; protected set; }

        /// <summary>
        /// 信息项验证器上下文
        /// </summary>
        public IInfoRuleSet InfoRules { get; protected set; }

        /// <summary>
        /// 命令提供程序上下文
        /// </summary>
        public IMessageProviderSet MessageProviders { get; protected set; }

        /// <summary>
        /// 命令生产者
        /// </summary>
        public IMessageProducer MessageProducer { get; protected set; }

        /// <summary>
        /// 数据提供程序上下文
        /// </summary>
        public IEntityProviderSet EntityProviders { get; protected set; }

        /// <summary>
        /// 命令转移器上下文
        /// </summary>
        public IMessageTransferSet Transfers { get; protected set; }

        /// <summary>
        /// 添加请求过滤器, 这些过滤器在Http请求被转化为Hecp请求后应用
        /// </summary>
        public List<Func<HecpContext, ProcessResult>> PreHecpRequestFilters { get; protected set; }

        /// <summary>
        /// 添加命令过滤器。这些过滤器在Command验证通过但被处理前应用
        /// </summary>
        public List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandingFilters { get; protected set; }

        /// <summary>
        /// 添加命令过滤器。这些过滤器在Command验证通过并被处理后应用
        /// </summary>
        public List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandledFilters { get; protected set; }

        /// <summary>
        /// 添加响应过滤器。这些过滤器在Hecp响应末段应用
        /// </summary>
        public List<Func<HecpContext, ProcessResult>> GlobalHecpResponseFilters { get; protected set; }

        /// <summary>
        /// 应用Hecp管道过滤器，通过返回结果表达当前Hecp请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        public ProcessResult ApplyPreHecpRequestFilters(HecpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var result = new ProcessResult(context.Response.IsSuccess, context.Response.Body.Event.Status, context.Response.Body.Event.Description);

            foreach (var requestFilter in PreHecpRequestFilters)
            {
                result = requestFilter(context);
                if (context.Response.IsClosed) break;
            }

            return result;
        }

        /// <summary>
        /// 应用Command管道过滤器，通过返回结果表达当前Command请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        public ProcessResult ApplyEdiMessageHandingFilters(MessageContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var result = new ProcessResult(context.Result.IsSuccess, context.Result.Status, context.Result.Description);

            // 执行全局命令过滤器
            foreach (var processedFilter in GlobalEdiMessageHandingFilters)
            {
                result = processedFilter(context);
                if (context.Result.IsClosed) break; ;
            }

            return result;
        }

        /// <summary>
        /// 应用Command管道过滤器，通过返回结果表达当前Command请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        public ProcessResult ApplyEdiMessageHandledFilters(MessageContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var result = new ProcessResult(context.Result.IsSuccess, context.Result.Status, context.Result.Description);

            // 执行全局命令过滤器
            foreach (var processedFilter in GlobalEdiMessageHandledFilters)
            {
                result = processedFilter(context);
                if (context.Result.IsClosed) break; ;
            }

            return result;
        }

        /// <summary>
        /// 应用Hecp管道过滤器，通过返回结果表达当前Hecp请求是否被处理过了，如果处理过了则就转到响应流程了。
        /// </summary>
        /// <returns></returns>
        public ProcessResult ApplyHecpResponseFilters(HecpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var result = new ProcessResult(context.Response.IsSuccess, context.Response.Body.Event.Status, context.Response.Body.Event.Description);

            //Exec global filters
            foreach (var responseFilter in GlobalHecpResponseFilters)
            {
                result = responseFilter(context);
                if (context.Response.IsClosed) break;
            }

            return result;
        }
    }
}
