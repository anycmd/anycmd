
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class SsdSetAddedEvent: EntityAddedEvent<ISsdSetCreateIo>
    {
        public SsdSetAddedEvent(SsdSetBase source, ISsdSetCreateIo output)
            : base(source, output)
        {
        }
    }
}
