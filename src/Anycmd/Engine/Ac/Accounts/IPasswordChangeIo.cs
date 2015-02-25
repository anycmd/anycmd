
namespace Anycmd.Engine.Ac.Accounts
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是更改账户密码时的输入或输出参数类型。
    /// </summary>
    public interface IPasswordChangeIo : IAnycmdInput
    {
        string LoginName { get; }
        string OldPassword { get; }
        string NewPassword { get; }
    }
}
