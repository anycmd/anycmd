
namespace Anycmd.Engine.Host
{

    /// <summary>
    /// 操作列表导入
    /// </summary>
    public interface IFunctionListImport
    {
        /// <summary>
        /// 导入指定程序集中的操作
        /// </summary>
        void Import(IAcDomain acDomain, IAcSession acSession, string appSystemCode);
    }
}
