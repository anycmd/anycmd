
namespace Anycmd.Engine.Ac.Accounts
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是为账户分配密码时的输入或输出参数类型。
    /// </summary>
    public interface IPasswordAssignIo : IAnycmdInput
    {
        Guid Id { get; set; }
        string LoginName { get; }
        string Password { get; }
    }
}
