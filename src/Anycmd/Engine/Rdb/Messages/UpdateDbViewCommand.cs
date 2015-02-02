
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbViewCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewCommand(IAcSession userSession, IDbViewUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = userSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbViewUpdateInput Input { get; private set; }
    }
}
