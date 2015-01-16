
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddDsdSetCommand : AddEntityCommand<IDsdSetCreateIo>, IAnycmdCommand
    {
        public AddDsdSetCommand(IDsdSetCreateIo input)
            : base(input)
        {

        }
    }
}
