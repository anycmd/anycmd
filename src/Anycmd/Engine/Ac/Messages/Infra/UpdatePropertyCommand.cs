
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdatePropertyCommand : UpdateEntityCommand<IPropertyUpdateIo>, IAnycmdCommand
    {
        public UpdatePropertyCommand(IPropertyUpdateIo input)
            : base(input)
        {

        }
    }
}
