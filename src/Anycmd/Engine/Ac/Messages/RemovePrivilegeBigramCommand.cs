
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemovePrivilegeBigramCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePrivilegeBigramCommand(Guid privilegeBigramId)
            : base(privilegeBigramId)
        {

        }
    }
}
