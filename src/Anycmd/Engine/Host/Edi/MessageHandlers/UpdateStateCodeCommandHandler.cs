
namespace Anycmd.Engine.Host.Edi.MessageHandlers
{
    using Commands;
    using Engine.Edi.Messages;
    using Exceptions;

    public class UpdateStateCodeCommandHandler : CommandHandler<UpdateStateCodeCommand>
    {
        private readonly IAcDomain _acDomain;

        public UpdateStateCodeCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(UpdateStateCodeCommand command)
        {
            throw new ValidationException("暂不支持修改");
        }
    }
}
