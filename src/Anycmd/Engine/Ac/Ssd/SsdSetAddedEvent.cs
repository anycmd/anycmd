
namespace Anycmd.Engine.Ac.Ssd
{

    public class SsdSetAddedEvent: EntityAddedEvent<ISsdSetCreateIo>
    {
        public SsdSetAddedEvent(IAcSession acSession, SsdSetBase source, ISsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
