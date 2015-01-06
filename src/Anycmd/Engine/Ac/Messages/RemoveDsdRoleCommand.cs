
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDsdRoleCommand(Guid dsdRoleId)
            : base(dsdRoleId)
        {

        }
    }
}
