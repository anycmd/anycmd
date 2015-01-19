
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class DicAddedEvent : EntityAddedEvent<IDicCreateIo>
    {
        public DicAddedEvent(IUserSession userSession, DicBase source, IDicCreateIo input)
            : base(userSession, source, input)
        {
        }
    }
}