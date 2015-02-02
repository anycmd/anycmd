
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbTableCommand : Command, IAnycmdCommand
    {
        public UpdateDbTableCommand(IAcSession userSession, IDbTableUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = userSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbTableUpdateInput Input { get; private set; }
    }
}
