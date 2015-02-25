
namespace Anycmd.Engine.Ac.AppSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是应用系统集。
    /// </summary>
    public interface IAppSystemSet : IEnumerable<AppSystemState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        AppSystemState SelfAppSystem { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSystemId"></param>
        /// <returns></returns>
        bool ContainsAppSystem(Guid appSystemId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSystemCode"></param>
        /// <returns></returns>
        bool ContainsAppSystem(string appSystemCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSystemId"></param>
        /// <param name="appSystem"></param>
        /// <returns></returns>
        bool TryGetAppSystem(Guid appSystemId, out AppSystemState appSystem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSystemCode"></param>
        /// <param name="appSystem"></param>
        /// <returns></returns>
        bool TryGetAppSystem(string appSystemCode, out AppSystemState appSystem);
    }
}
