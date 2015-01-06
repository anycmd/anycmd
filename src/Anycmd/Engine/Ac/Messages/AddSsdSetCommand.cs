
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, IAnycmdCommand
    {
        public AddSsdSetCommand(ISsdSetCreateIo input)
            : base(input)
        {

        }
    }
}
