
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class MenuAddedEvent : EntityAddedEvent<IMenuCreateIo>
    {
        public MenuAddedEvent(IAcSession acSession, MenuBase source, IMenuCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}