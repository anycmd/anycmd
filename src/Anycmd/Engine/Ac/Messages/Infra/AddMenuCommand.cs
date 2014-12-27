
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, ISysCommand
    {
        public AddMenuCommand(IMenuCreateIo input)
            : base(input)
        {

        }
    }
}
