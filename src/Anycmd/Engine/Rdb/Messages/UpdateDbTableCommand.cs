
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbTableCommand : Command, IAnycmdCommand
    {
        public UpdateDbTableCommand(IUserSession userSession, IDbTableUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IUserSession UserSession { get; private set; }

        public IDbTableUpdateInput Input { get; private set; }
    }
}
