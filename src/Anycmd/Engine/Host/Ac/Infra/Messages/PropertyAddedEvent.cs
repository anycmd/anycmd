
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyAddedEvent : EntityAddedEvent<IPropertyCreateIo>
    {
        public PropertyAddedEvent(PropertyBase source, IPropertyCreateIo input)
            : base(source, input)
        {
        }
    }
}
