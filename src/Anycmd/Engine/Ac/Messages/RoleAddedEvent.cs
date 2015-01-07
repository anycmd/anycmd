
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using InOuts;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(RoleBase source, IRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
