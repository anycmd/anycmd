
namespace Anycmd.Engine.Ac.Ssd
{

    public class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(IAcSession acSession, SsdRoleBase source, ISsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
