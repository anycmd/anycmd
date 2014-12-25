
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;

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