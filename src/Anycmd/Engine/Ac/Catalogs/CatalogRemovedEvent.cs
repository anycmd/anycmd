
namespace Anycmd.Engine.Ac.Catalogs
{
    using Abstractions.Infra;
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogRemovedEvent : DomainEvent
    {
        public CatalogRemovedEvent(IAcSession acSession, CatalogBase source)
            : base(acSession, source)
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
