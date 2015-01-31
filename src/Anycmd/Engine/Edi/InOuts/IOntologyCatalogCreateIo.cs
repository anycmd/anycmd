using System;

namespace Anycmd.Engine.Edi.InOuts
{
    public interface IOntologyCatalogCreateIo : IEntityCreateInput
    {
        /// <summary>
        /// 
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid CatalogId { get; }
    }
}
