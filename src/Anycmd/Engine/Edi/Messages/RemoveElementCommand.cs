
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveElementCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveElementCommand(Guid elementId)
            : base(elementId)
        {

        }
    }
}
