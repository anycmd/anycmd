
namespace Anycmd.Engine.Ac.AppSystems
{
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemAddedEvent : DomainEvent
    {
        public AppSystemAddedEvent(IAcSession acSession, AppSystemBase source, IAppSystemCreateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IAppSystemCreateIo Input { get; private set; }
    }
}