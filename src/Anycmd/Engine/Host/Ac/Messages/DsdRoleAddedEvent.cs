
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(DsdRoleBase source, IDsdRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
