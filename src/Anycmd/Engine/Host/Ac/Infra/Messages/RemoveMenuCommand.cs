
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveMenuCommand(Guid menuId)
            : base(menuId)
        {

        }
    }
}
