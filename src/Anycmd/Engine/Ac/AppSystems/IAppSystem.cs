
namespace Anycmd.Engine.Ac.AppSystems
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是应用系统。
    /// </summary>
    public interface IAppSystem
    {
        Guid Id { get; }

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
        Guid PrincipalId { get; }

        int IsEnabled { get; }

        string SsoAuthAddress { get; }

        string Icon { get; }
    }
}
