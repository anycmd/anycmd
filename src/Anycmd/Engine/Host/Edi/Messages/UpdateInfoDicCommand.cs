
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateInfoDicCommand : UpdateEntityCommand<IInfoDicUpdateIo>, ISysCommand
    {
        public UpdateInfoDicCommand(IInfoDicUpdateIo input)
            : base(input)
        {

        }
    }
}
