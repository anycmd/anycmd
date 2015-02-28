
namespace Anycmd.Engine.Ac.AppSystems
{
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppSystemUpdatedEvent : DomainEvent
    {
        public AppSystemUpdatedEvent(IAcSession acSession, AppSystemBase source, IAppSystemUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IAppSystemUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}