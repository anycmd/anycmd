
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveMenuCommand(Guid menuId)
            : base(menuId)
        {

        }
    }
}
