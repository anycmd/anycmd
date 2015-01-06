
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateDicItemCommand : UpdateEntityCommand<IDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateDicItemCommand(IDicItemUpdateIo input)
            : base(input)
        {

        }
    }
}
