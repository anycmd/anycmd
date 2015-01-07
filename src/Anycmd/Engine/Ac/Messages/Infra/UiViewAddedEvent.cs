
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewAddedEvent : EntityAddedEvent<IUiViewCreateIo>
    {
        public UiViewAddedEvent(UiViewBase source, IUiViewCreateIo input)
            : base(source, input)
        {
        }
    }
}
