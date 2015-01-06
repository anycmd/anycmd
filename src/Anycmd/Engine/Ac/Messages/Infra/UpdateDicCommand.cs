
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateDicCommand : UpdateEntityCommand<IDicUpdateIo>, IAnycmdCommand
    {
        public UpdateDicCommand(IDicUpdateIo input)
            : base(input)
        {

        }
    }
}
