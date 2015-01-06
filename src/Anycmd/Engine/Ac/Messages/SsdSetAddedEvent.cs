
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;

    public class SsdSetAddedEvent: EntityAddedEvent<ISsdSetCreateIo>
    {
        public SsdSetAddedEvent(SsdSetBase source, ISsdSetCreateIo output)
            : base(source, output)
        {
        }
    }
}
