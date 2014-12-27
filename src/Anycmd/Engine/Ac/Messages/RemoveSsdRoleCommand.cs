
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveSsdRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveSsdRoleCommand(Guid ssdRoleId)
            : base(ssdRoleId)
        {

        }
    }
}
