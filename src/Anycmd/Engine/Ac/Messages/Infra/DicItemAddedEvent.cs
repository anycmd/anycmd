
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class DicItemAddedEvent : EntityAddedEvent<IDicItemCreateIo>
    {
        public DicItemAddedEvent(DicItemBase source, IDicItemCreateIo input)
            : base(source, input)
        {
        }
    }
}