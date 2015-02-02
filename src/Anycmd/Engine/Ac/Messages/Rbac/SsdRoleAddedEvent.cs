
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(IAcSession userSession, SsdRoleBase source, ISsdRoleCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
