
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateDicCommand : UpdateEntityCommand<IDicUpdateIo>, ISysCommand
    {
        public UpdateDicCommand(IDicUpdateIo input)
            : base(input)
        {

        }
    }
}
