
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class DicUpdatedEvent : DomainEvent
    {
        public DicUpdatedEvent(IUserSession userSession, DicBase source, IDicUpdateIo input)
            : base(userSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IDicUpdateIo Input { get; private set; }
    }
}