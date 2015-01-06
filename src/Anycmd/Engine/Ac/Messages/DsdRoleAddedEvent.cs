
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;

    public class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(DsdRoleBase source, IDsdRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
