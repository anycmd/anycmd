
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemovePrivilegeBigramCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePrivilegeBigramCommand(Guid privilegeBigramId)
            : base(privilegeBigramId)
        {

        }
    }
}
