
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    public class MenuAddedEvent : EntityAddedEvent<IMenuCreateIo>
    {
        public MenuAddedEvent(MenuBase source, IMenuCreateIo input)
            : base(source, input)
        {
        }
    }
}