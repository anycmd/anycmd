
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IMenuCreateIo input)
            : base(input)
        {

        }
    }
}
