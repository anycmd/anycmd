
namespace Anycmd.Engine.Ac.Catalogs
{
    using Events;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CatalogRemovedEvent : DomainEvent
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

        internal CatalogRemovedEvent(IAcSession acSession, CatalogBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        public string CatalogCode { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
