
namespace Anycmd.Engine.Rdb.Messages
{
    using Engine;
    using InOuts;

    public class UpdateDatabaseCommand : UpdateEntityCommand<IDatabaseUpdateInput>, IAnycmdCommand
    {
        public UpdateDatabaseCommand(IAcSession acSession, IDatabaseUpdateInput input)
            : base(acSession, input)
        {

        }
    }
}
