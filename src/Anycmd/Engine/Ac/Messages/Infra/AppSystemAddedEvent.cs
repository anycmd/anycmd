
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemAddedEvent : DomainEvent
    {
        public AppSystemAddedEvent(IUserSession userSession, AppSystemBase source, IAppSystemCreateIo input)
            : base(userSession, source)
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