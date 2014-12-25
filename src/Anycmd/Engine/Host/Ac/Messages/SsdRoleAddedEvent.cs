
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(SsdRoleBase source, ISsdRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
