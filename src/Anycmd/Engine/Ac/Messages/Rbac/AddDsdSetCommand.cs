
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddDsdSetCommand : AddEntityCommand<IDsdSetCreateIo>, IAnycmdCommand
    {
        public AddDsdSetCommand(IAcSession acSession, IDsdSetCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
