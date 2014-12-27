
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewUpdatedEvent : DomainEvent
    {
        public UiViewUpdatedEvent(UiViewBase source, IUiViewUpdateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IUiViewUpdateIo Input { get; private set; }
    }
}
