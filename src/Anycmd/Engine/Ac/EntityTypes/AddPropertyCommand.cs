
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;

    public class AddPropertyCommand : AddEntityCommand<IPropertyCreateIo>, IAnycmdCommand
    {
        public AddPropertyCommand(IAcSession acSession, IPropertyCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
