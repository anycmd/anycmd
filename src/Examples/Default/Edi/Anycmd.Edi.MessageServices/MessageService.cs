  
namespace Anycmd.Edi.MessageServices
{
    using Engine.Hecp;
    using Engine.Host.Edi;
    using ServiceModel.Operations;
    using ServiceStack;
    using System;
    using Util;

    /// <summary>
    /// 命令服务。
    /// 操作只有两种：命令和查询。所以该服务只有两个操作方法就功能完备了。
    /// </summary>
    public sealed class MessageService : Service
    {
        #region Ctor
        /// <summary>
        /// 默认构造函数。默认从数据交换上下文中获取IHecpHandlerFactory（Hecp处理器工厂）
        /// </summary>
        public MessageService()
        {
        }
        #endregion

        private IAcDomain host
        {
            get
            {
                return System.Web.HttpContext.Current.Application["AcDomainInstance"] as IAcDomain;
            }
        }

        #region AnyIsAlive
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IsAliveResponse Any(IsAlive request)
        {
            if (request == null)
            {
                return new IsAliveResponse
                {
                    Description = "传入的参数错误",
                    Status = (int)Status.InvalidArgument,
                    ReasonPhrase = Status.InvalidArgument.ToName(),
                    IsAlive = false
                };
            }
            ApiVersion apiVersion;
            if (string.IsNullOrEmpty(request.Version) || !request.Version.TryParse(out apiVersion))
            {
                return new IsAliveResponse
                {
                    Description = "非法的api版本号",
                    Status = (int)Status.InvalidApiVersion,
                    ReasonPhrase = Status.InvalidApiVersion.ToName(),
                    IsAlive = false
                };
            }

            return new IsAliveResponse
            {
                IsAlive = true,
                Status = (int)Status.Ok,
                ReasonPhrase = Status.Ok.ToName(),
                Description = "服务可用"
            };
        }
        #endregion

        #region AnyCommand
        /// <summary>
        /// 处理任何命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public Message Any(Message request)
        {
            try
            {
                var context = new HecpContext(host, HecpRequest.Create(host, request));
                host.NodeHost.HecpHandler.Process(context);

                return context.Response.ToMessage();
            }
            catch (Exception ex)
            {
                host.LoggingService.Error(ex);
                var r = new Message
                {
                    MessageType = MessageType.Event.ToName(),
                    MessageId = string.Empty
                };
                r.Body.Event.Description = "服务器内部逻辑异常";
                r.Body.Event.Status = 500;
                r.Body.Event.ReasonPhrase = Status.InternalServerError.ToString();// 不使用ToName扩展方法以避免造成新的异常

                return r;
            }
        }
        #endregion
    }
}
