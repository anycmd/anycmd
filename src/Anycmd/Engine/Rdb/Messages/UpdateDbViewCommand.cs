
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine.Messages;
    using InOuts;
    using System;

    public class UpdateDbViewCommand: Command, IAnycmdCommand
    {
        public UpdateDbViewCommand(IAcSession acSession, IDbViewUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbViewUpdateInput Input { get; private set; }
    }
}
