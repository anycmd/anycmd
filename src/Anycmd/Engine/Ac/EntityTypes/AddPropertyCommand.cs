
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddPropertyCommand : AddEntityCommand<IPropertyCreateIo>, IAnycmdCommand
    {
        public AddPropertyCommand(IAcSession acSession, IPropertyCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
