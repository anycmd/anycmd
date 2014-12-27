
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdatedEvent : DomainEvent
    {
        public UiViewButtonUpdatedEvent(UiViewButtonBase source, IUiViewButtonUpdateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IUiViewButtonUpdateIo Input { get; private set; }
    }
}
