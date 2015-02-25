
namespace Anycmd.Engine.Ac.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是系统功能集。
    /// </summary>
    public interface IFunctionSet : IEnumerable<FunctionState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="functionCode"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        bool TryGetFunction(CatalogState resource, string functionCode, out FunctionState function);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        bool TryGetFunction(Guid functionId, out FunctionState function);
    }
}
