
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(IAcSession acSession, DsdSetBase source, IDsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
