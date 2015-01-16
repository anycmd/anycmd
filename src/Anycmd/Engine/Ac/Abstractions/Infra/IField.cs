
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    public interface IField
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid ResourceTypeId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }
    }
}
