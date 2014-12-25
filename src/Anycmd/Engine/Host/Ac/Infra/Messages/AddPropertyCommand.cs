
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
