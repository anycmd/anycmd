
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class OrganizationRemovedEvent : DomainEvent
    {
        public OrganizationRemovedEvent(OrganizationBase source)
            : base(source)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }
            this.OrganizationCode = source.Code;
        }

        public string OrganizationCode { get; private set; }
    }
}
