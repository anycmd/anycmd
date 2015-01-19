
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeAddedEvent : EntityAddedEvent<IEntityTypeCreateIo>
    {
        public EntityTypeAddedEvent(IUserSession userSession, EntityTypeBase source, IEntityTypeCreateIo input)
            : base(userSession, source, input)
        {
        }
    }
}
