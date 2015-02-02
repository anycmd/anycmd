
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(IAcSession userSession, DsdSetBase source, IDsdSetCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
