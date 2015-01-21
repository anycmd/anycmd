
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbTableColumnCommand : Command, IAnycmdCommand
    {
        public UpdateDbTableColumnCommand(IUserSession userSession, IDbTableColumnUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IUserSession UserSession { get; private set; }

        public IDbTableColumnUpdateInput Input { get; private set; }
    }
}
