
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogRemovedEvent : DomainEvent
    {
        public CatalogRemovedEvent(IAcSession userSession, CatalogBase source)
            : base(userSession, source)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }
            this.CatalogCode = source.Code;
        }

        public string CatalogCode { get; private set; }
    }
}
