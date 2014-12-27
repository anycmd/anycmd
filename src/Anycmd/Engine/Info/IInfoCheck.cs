
namespace Anycmd.Engine.Info
{
    using Host;
    using Host.Edi;
    using System;

    public interface IInfoCheck : IWfResource, IDisposable
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

        Guid[] ElementIDs { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ProcessResult Valid(InfoItem[] data);
    }
}
