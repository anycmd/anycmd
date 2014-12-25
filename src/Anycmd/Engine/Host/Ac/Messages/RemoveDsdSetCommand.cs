
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveDsdSetCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveDsdSetCommand(Guid dsdSetId)
            : base(dsdSetId)
        {

        }
    }
}
