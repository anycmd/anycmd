
namespace Anycmd.Engine.Ac.InOuts
{
    /// <summary>
    /// 表示该接口的实现类是更改账户密码时的输入或输出参数类型。
    /// </summary>
    public interface IPasswordChangeIo
    {
        string LoginName { get; }
        string OldPassword { get; }
        string NewPassword { get; }
    }
}
