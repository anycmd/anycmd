
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(IAcSession userSession, DsdRoleBase source, IDsdRoleCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
