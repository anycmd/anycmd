
namespace Anycmd.Engine.Rdb.Messages
{
    using Commands;
    using Engine.Messages;
    using InOuts;
    using System;

    public class UpdateDbTableColumnCommand : Command, IAnycmdCommand
    {
        public UpdateDbTableColumnCommand(IAcSession acSession, IDbTableColumnUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public IDbTableColumnUpdateInput Input { get; private set; }
    }
}
