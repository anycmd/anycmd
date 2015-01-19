
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class OrganizationRemovedEvent : DomainEvent
    {
        public OrganizationRemovedEvent(IUserSession userSession, OrganizationBase source)
            : base(userSession, source)
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
