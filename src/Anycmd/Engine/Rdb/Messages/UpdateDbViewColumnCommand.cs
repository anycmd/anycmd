
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbViewColumnCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewColumnCommand(IUserSession userSession, IDbViewColumnUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IUserSession UserSession { get; private set; }

        public IDbViewColumnUpdateInput Input { get; private set; }
    }
}
