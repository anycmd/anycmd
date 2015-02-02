
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine;
    using InOuts;
    using System;

    public class UpdateDbViewColumnCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewColumnCommand(IAcSession userSession, IDbViewColumnUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = userSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbViewColumnUpdateInput Input { get; private set; }
    }
}
