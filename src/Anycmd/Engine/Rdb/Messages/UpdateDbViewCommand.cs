
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbViewCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewCommand(IUserSession userSession, IDbViewUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IUserSession UserSession { get; private set; }

        public IDbViewUpdateInput Input { get; private set; }
    }
}
