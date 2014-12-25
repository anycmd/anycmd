
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Engine.Ac;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是组织结构集。
    /// </summary>
    public interface IOrganizationSet : IEnumerable<OrganizationState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        bool TryGetOrganization(Guid organizationId, out OrganizationState organization);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationCode"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        bool TryGetOrganization(string organizationCode, out OrganizationState organization);
    }
}
