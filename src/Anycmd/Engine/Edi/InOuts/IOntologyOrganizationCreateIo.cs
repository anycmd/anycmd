using Anycmd.Model;
using System;

namespace Anycmd.Engine.Edi.InOuts
{
    public interface IOntologyOrganizationCreateIo : IEntityCreateInput
    {
        /// <summary>
        /// 
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid OrganizationId { get; }
    }
}
