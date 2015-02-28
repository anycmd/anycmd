
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(IAcSession acSession, SsdRoleBase source, ISsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
