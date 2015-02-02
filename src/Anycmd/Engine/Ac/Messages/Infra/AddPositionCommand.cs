
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddPositionCommand : AddEntityCommand<IPositionCreateIo>, IAnycmdCommand
    {
        public AddPositionCommand(IAcSession acSession, IPositionCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
