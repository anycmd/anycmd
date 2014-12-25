
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemovePrivilegeBigramCommand : RemoveEntityCommand, ISysCommand
    {
        public RemovePrivilegeBigramCommand(Guid privilegeBigramId)
            : base(privilegeBigramId)
        {

        }
    }
}
