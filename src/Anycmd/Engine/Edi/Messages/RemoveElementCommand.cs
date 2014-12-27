
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveElementCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveElementCommand(Guid elementId)
            : base(elementId)
        {

        }
    }
}
