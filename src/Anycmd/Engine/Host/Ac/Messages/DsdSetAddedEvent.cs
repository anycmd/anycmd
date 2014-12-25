
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(DsdSetBase source, IDsdSetCreateIo output)
            : base(source, output)
        {
        }
    }
}
