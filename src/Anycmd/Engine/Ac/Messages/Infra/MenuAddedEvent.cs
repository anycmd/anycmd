
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    public class MenuAddedEvent : EntityAddedEvent<IMenuCreateIo>
    {
        public MenuAddedEvent(IAcSession userSession, MenuBase source, IMenuCreateIo input)
            : base(userSession, source, input)
        {
        }
    }
}