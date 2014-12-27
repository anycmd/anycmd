
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveMenuCommand(Guid menuId)
            : base(menuId)
        {

        }
    }
}
