
namespace Anycmd.Engine.Info
{
    using Host;
    using Host.Edi;
    using System;

    /// <summary>
    /// 信息项验证器。
    /// <remarks>
    /// 注意：不要在验证器中针对特定的本体元素码验证。验证手机号码的验证器就是验证手机号码，
    /// 就是验证传入的值是不是手机号码，不要绑定到具体的数据来源。把验证器关联到具体的本体元素是由ElementInfoRule模块来配置的。
    /// </remarks>
    /// </summary>
    public interface IInfoRule : IWfResource, IDisposable
    {
        /// <summary>
        /// 插件标识
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// 插件标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 插件描述
        /// </summary>
        new string Description { get; }

        /// <summary>
        /// 插件作者。如xuexs
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ProcessResult Valid(string value);
    }
}
