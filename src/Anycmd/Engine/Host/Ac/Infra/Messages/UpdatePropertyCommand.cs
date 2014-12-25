
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdatePropertyCommand : UpdateEntityCommand<IPropertyUpdateIo>, ISysCommand
    {
        public UpdatePropertyCommand(IPropertyUpdateIo input)
            : base(input)
        {

        }
    }
}
