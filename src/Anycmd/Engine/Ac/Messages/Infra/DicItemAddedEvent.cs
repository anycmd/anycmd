
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class DicItemAddedEvent : EntityAddedEvent<IDicItemCreateIo>
    {
        public DicItemAddedEvent(IUserSession userSession, DicItemBase source, IDicItemCreateIo input)
            : base(userSession, source, input)
        {
        }
    }
}