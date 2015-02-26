
namespace Anycmd.Engine.Ac.Privileges
{
    /// <summary>
    /// 表示实现该接口的类为Ac记录
    /// </summary>
    public interface IAcRecord
    {
        /// <summary>
        /// 记录类型
        /// </summary>
        AcRecordType AcRecordType { get; }
    }
}
