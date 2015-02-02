
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdatePropertyCommand : UpdateEntityCommand<IPropertyUpdateIo>, IAnycmdCommand
    {
        public UpdatePropertyCommand(IAcSession acSession, IPropertyUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
