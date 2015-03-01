
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class MenuAddedEvent : EntityAddedEvent<IMenuCreateIo>
    {
        public MenuAddedEvent(IAcSession acSession, MenuBase source, IMenuCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}