
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemoveSsdRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveSsdRoleCommand(Guid ssdRoleId)
            : base(ssdRoleId)
        {

        }
    }
}
