
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using DataContracts;
    using Distribute;
    using Engine.Edi;
    using Exceptions;
    using Info;
    using Model;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 命令转移策略抽象基类，提供模板方法用于帮助转移策略的实现
    /// <remarks>转移策略的实现只应做与命令转移相关的工作，除此之外不应做其它工作</remarks>
    /// </summary>
    public abstract class MessageTransferBase : DisposableObject, IMessageTransfer
    {
        private BuiltInResourceKind _resourceType = BuiltInResourceKind.CommandTransfer;

        /// <summary>
        /// 
        /// </summary>
        protected MessageTransferBase() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="description"></param>
        protected MessageTransferBase(Guid id, string title, string author, string description)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Author = author;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Author { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return Title; }
            protected set
            {
                Title = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BuiltInResourceKind BuiltInResourceKind
        {
            get { return _resourceType; }
            protected set
            {
                _resourceType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public abstract string GetAddress(NodeDescriptor actor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract IBeatResponse IsAliveCore(BeatContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<IBeatResponse> IsAliveCoreAsync(BeatContext context);

        /// <summary>
        /// 转移
        /// </summary>
        /// <param name="context">发送事件参数</param>
        protected abstract IMessageDto TransmitCore(DistributeContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<IMessageDto> TransmitCoreAsync(DistributeContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void IsAlive(BeatContext context)
        {
            if (context == null || context.Request == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = IsAliveCore(context);
            if (response != null)
            {
                context.Response.SetData(response);
            }
            else
            {
                if (context.Exception == null)
                {
                    context.Exception = new AnycmdException("远端服务未返回值");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task IsAliveAsync(BeatContext context)
        {
            if (context == null || context.Request == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = await IsAliveCoreAsync(context);
            if (response != null)
            {
                context.Response.SetData(response);
            }
            else
            {
                if (context.Exception == null)
                {
                    context.Exception = new AnycmdException("远端服务未返回值");
                }
            }
        }

        #region Distribute
        /// <summary>
        /// 把给定的命令转移到远端节点
        /// </summary>
        /// <param name="context">转移上下文</param>
        /// <returns></returns>
        public void Transmit(DistributeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = TransmitCore(context);
            if (response != null)
            {
                Status responseStateCode;
                if (!response.Body.Event.Status.TryParse(out responseStateCode))
                {
                    context.Exception = new AnycmdException("响应节点返回了意外的状态码" + response.Body.Event.Status.ToString());
                }
                context.Result.UpdateStatus(responseStateCode, response.Body.Event.Description);
                if (response.Body.InfoValue != null)
                {
                    foreach (var item in response.Body.InfoValue)
                    {
                        context.Result.ResultDataItems.Add(new DataItem(item.Key, item.Value));
                    }
                }
            }
            else
            {
                if (context.Exception == null)
                {
                    context.Exception = new AnycmdException("远端服务未返回值");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task TransmitAsync(DistributeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = await TransmitCoreAsync(context);
            if (response != null)
            {
                Status responseStateCode;
                if (!response.Body.Event.Status.TryParse(out responseStateCode))
                {
                    context.Exception = new AnycmdException("响应节点返回了意外的状态码" + response.Body.Event.Status.ToString());
                }
                context.Result.UpdateStatus(responseStateCode, response.Body.Event.Description);
                if (response.Body.InfoValue != null)
                {
                    foreach (var item in response.Body.InfoValue)
                    {
                        context.Result.ResultDataItems.Add(new DataItem(item.Key, item.Value));
                    }
                }
            }
            else
            {
                if (context.Exception == null)
                {
                    context.Exception = new AnycmdException("远端服务未返回值");
                }
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {

        }
    }
}
