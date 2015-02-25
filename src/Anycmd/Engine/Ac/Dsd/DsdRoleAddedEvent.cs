
namespace Anycmd.Engine.Ac.Dsd
{
    using Abstractions.Rbac;
    using InOuts;

    public class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(IAcSession acSession, DsdRoleBase source, IDsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
