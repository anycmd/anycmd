
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemAddedEvent : DomainEvent
    {
        public AppSystemAddedEvent(AppSystemBase source, IAppSystemCreateIo input)
            : base(source)
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