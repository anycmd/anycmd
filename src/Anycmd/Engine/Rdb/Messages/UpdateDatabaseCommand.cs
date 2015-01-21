
namespace Anycmd.Engine.Rdb.Messages
{
    using Engine;
    using InOuts;

    public class UpdateDatabaseCommand: UpdateEntityCommand<IDatabaseUpdateInput>, IAnycmdCommand
    {
        public UpdateDatabaseCommand(IUserSession userSession, IDatabaseUpdateInput input)
            : base(userSession, input)
        {

        }
    }
}
