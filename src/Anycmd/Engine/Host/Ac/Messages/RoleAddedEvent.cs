
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(RoleBase source, IRoleCreateIo output)
            : base(source, output)
        {
        }
    }
}
