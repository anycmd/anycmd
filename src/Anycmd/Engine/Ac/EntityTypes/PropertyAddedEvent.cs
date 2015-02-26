
namespace Anycmd.Engine.Ac.EntityTypes
{
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
