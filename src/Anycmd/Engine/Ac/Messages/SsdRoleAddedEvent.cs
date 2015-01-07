
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using InOuts;

    public class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(SsdRoleBase source, ISsdRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
