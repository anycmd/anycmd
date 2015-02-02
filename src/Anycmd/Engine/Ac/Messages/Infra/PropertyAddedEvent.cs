
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyAddedEvent : EntityAddedEvent<IPropertyCreateIo>
    {
        public PropertyAddedEvent(IAcSession acSession, PropertyBase source, IPropertyCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
