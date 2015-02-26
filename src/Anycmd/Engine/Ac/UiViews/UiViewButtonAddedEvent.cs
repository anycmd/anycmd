
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonAddedEvent : EntityAddedEvent<IUiViewButtonCreateIo>
    {
        public UiViewButtonAddedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
