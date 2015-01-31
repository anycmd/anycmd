
namespace Anycmd.Engine.Host.Edi.Handlers
{

    /// <summary>
    /// 验证Level1Action、Level2ElementAction、Level3ClientAction、Level4ClientElementAction级权限。
    /// <remarks>
    /// 上面四级权限验证的输入来自客户端命令，而Level5CatalogAction级权限验证就不是来自客户端输入了。
    /// 验证Level5CatalogAction级权限的前提是已经定位了目标记录，目录码是从目标记录上得去的。
    /// </remarks>
    /// </summary>
    public interface IPermissionValidator
    {
        /// <summary>
        /// 验证Level1Action、Level2ElementAction、Level3ClientAction、Level4ClientElementAction级权限。
        /// <remarks>
        /// 其它级权限不要在此验证。
        /// </remarks>
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <returns></returns>
        ProcessResult Validate(MessageContext context);
    }
}
