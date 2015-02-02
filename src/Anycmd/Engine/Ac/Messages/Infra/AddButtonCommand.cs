
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IAcSession acSession, IButtonCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
