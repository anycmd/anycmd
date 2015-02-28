
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(IAcSession acSession, DsdRoleBase source, IDsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
