
namespace Anycmd.Edi.MessageTransfers
{
    using DataContracts;
    using Engine.Edi;
    using Engine.Hecp;
    using Engine.Host.Edi.Handlers;
    using Engine.Host.Edi.Handlers.Distribute;
    using Engine.Info;
    using ServiceModel.Operations;
    using ServiceStack;
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Util;

    /// <summary>
    /// 基于WebApi的命令发送策略
    /// </summary>
    [Export(typeof(IMessageTransfer))]
    public sealed class WebApiMessageTransfer : MessageTransferBase
    {
        private const string ActorId = "E8AC7575-EA84-419C-B4A0-0A2B62ACF8E7";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "WebApi";
        private const string description = "WebApi传输器。调用命令接收节点的WebApi服务传送命令。";
        private const string author = "xuexs";

        public WebApiMessageTransfer() : base(id, title, author, description) { }

        public override string GetAddress(NodeDescriptor actor)
        {
            return actor.AnycmdApiAddress;
        }

        #region IsServiceAliveCore
        protected override IBeatResponse IsAliveCore(BeatContext context)
        {
            try
            {
                var client = new JsonServiceClient(context.Node.Node.AnycmdApiAddress);
                return client.Get(GetRequest(context));
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                return null;
            }
        }

        protected async override Task<IBeatResponse> IsAliveCoreAsync(BeatContext context)
        {
            try
            {
                var client = new JsonServiceClient(context.Node.Node.AnycmdApiAddress);
                return await client.GetAsync(GetRequest(context));
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                return null;
            }
        }
        #endregion

        #region TransmitCore
        protected sealed override IMessageDto TransmitCore(DistributeContext context)
        {
            try
            {
                var client = new JsonServiceClient(context.Responder.AnycmdApiAddress);
                return client.Get(GetRequest(context));
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                return null;
            }
        }

        protected async override Task<IMessageDto> TransmitCoreAsync(DistributeContext context)
        {
            try
            {
                var client = new JsonServiceClient(context.Responder.AnycmdApiAddress);
                return await client.GetAsync(GetRequest(context));
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                return null;
            }
        }

        private IsAlive GetRequest(BeatContext context)
        {
            return new IsAlive()
            {
                Version = context.Request.Version
            };
        }

        private Message GetRequest(DistributeContext context)
        {
            var command = context.Command;
            var ticks = DateTime.UtcNow.Ticks;
            var request = new Message
            {
                Verb = command.Verb.Code,
                Ontology = command.Ontology,
                MessageType = command.MessageType.ToName(),
                Body = new BodyData(command.DataTuple.IdItems.Items.ToDto(), command.DataTuple.ValueItems.Items.ToDto())
                {
                    Event = new EventData
                    {
                        SourceType = command.EventSourceType,
                        Subject = command.EventSubjectCode,
                        ReasonPhrase = command.ReasonPhrase,
                        Status = command.Status
                    }
                },
                TimeStamp = command.TimeStamp.ToUniversalTime().Ticks,
                MessageId = command.MessageId,
                Version = command.Version,
                IsDumb = command.IsDumb,
                Credential = new CredentialData
                {
                    ClientType = ClientType.Node.ToName(),
                    UserType = UserType.None.ToName(),
                    CredentialType = CredentialType.Signature.ToName(),
                    SignatureMethod = SignatureMethod.HMAC_SHA1.ToName(),
                    ClientId = context.ClientAgent.PublicKey,
                    UserName = command.UserName,// UserName
                    Ticks = ticks
                }
            };
            // 签名
            request.Credential.Password = Signature.Sign(request.ToOrignalString(request.Credential), context.ClientAgent.SecretKey);

            return request;
        }
        #endregion
    }
}
