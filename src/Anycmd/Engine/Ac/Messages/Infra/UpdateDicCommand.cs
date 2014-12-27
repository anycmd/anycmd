
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateDicCommand : UpdateEntityCommand<IDicUpdateIo>, IAnycmdCommand
    {
        public UpdateDicCommand(IDicUpdateIo input)
            : base(input)
        {

        }
    }
}
