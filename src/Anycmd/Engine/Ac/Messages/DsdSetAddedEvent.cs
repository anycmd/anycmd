
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using InOuts;

    public class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(DsdSetBase source, IDsdSetCreateIo output)
            : base(source, output)
        {
        }
    }
}
