
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeAddedEvent : EntityAddedEvent<IEntityTypeCreateIo>
    {
        public EntityTypeAddedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
