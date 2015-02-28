
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine.Messages;
    using InOuts;
    using System;

    public class UpdateDbViewColumnCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewColumnCommand(IAcSession acSession, IDbViewColumnUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbViewColumnUpdateInput Input { get; private set; }
    }
}
