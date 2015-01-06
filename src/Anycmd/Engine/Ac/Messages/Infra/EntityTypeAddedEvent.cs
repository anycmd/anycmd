
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeAddedEvent : EntityAddedEvent<IEntityTypeCreateIo>
    {
        public EntityTypeAddedEvent(EntityTypeBase source, IEntityTypeCreateIo input)
            : base(source, input)
        {
        }
    }
}
