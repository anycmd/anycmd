
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddPropertyCommand : AddEntityCommand<IPropertyCreateIo>, ISysCommand
    {
        public AddPropertyCommand(IPropertyCreateIo input)
            : base(input)
        {

        }
    }
}
