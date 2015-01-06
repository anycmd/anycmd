
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemovePrivilegeCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePrivilegeCommand(Guid privilegeBigramId)
            : base(privilegeBigramId)
        {

        }
    }
}
