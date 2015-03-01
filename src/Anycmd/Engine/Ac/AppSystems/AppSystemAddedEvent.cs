
namespace Anycmd.Engine.Ac.AppSystems
{
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AppSystemAddedEvent : DomainEvent
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

        internal AppSystemAddedEvent(IAcSession acSession, AppSystemBase source, IAppSystemCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IAppSystemCreateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}