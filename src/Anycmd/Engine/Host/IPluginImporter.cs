
namespace Anycmd.Engine.Host
{
    using System.Collections.Generic;

    /// <summary>
    /// 插件导入器。将插件dll导入应用程序域并返回插件列表。
    /// </summary>
    public interface IPluginImporter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetPlugins();
    }
}
