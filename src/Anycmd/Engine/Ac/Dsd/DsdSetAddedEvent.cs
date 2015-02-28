
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(IAcSession acSession, DsdSetBase source, IDsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
