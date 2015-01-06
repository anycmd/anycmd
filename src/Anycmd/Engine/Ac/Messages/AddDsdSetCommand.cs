
namespace Anycmd.Engine.Ac.Messages
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
