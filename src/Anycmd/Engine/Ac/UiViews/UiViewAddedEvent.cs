
namespace Anycmd.Engine.Ac.UiViews
{

    /// <summary>
    /// 
    /// </summary>
    public class UiViewAddedEvent : EntityAddedEvent<IUiViewCreateIo>
    {
        public UiViewAddedEvent(IAcSession acSession, UiViewBase source, IUiViewCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
