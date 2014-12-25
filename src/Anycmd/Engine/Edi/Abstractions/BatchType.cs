
namespace Anycmd.Engine.Edi.Abstractions
{
    using System.ComponentModel;

    /// <summary>
    /// 批类型
    /// </summary>
    public enum BatchType : byte
    {
        /// <summary>
        /// 构建创建型命令
        /// </summary>
        [Description("建造Create型命令")]
        BuildCreateCommand = 1,
        /// <summary>
        /// 构建修改型命令
        /// </summary>
        [Description("建造Update型命令")]
        BuildUpdateCommand = 2,
        /// <summary>
        /// 构建删除型命令
        /// </summary>
        [Description("建造Delete型命令")]
        BuildDeleteCommand = 3
    }
}
