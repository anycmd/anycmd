
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义信息项验证器访问接口。
    /// </summary>
    public interface IInfoRuleSet : IEnumerable<InfoRuleState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="infoRule"></param>
        /// <returns></returns>
        bool TryGetInfoRule(Guid id, out InfoRuleState infoRule);
    }
}
