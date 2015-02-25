
namespace Anycmd.Engine.Ac.Accounts
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新账户时的输入或输出参数类型。
    /// </summary>
    public interface IAccountUpdateIo : IEntityUpdateInput
    {
        DateTime? AllowEndTime { get; }
        DateTime? AllowStartTime { get; }
        string AuditState { get; }
        string Description { get; }
        int IsEnabled { get; }
        DateTime? LockEndTime { get; }
        DateTime? LockStartTime { get; }
        string Code { get; }
        string Email { get; }
        string Mobile { get; }
        string Name { get; }
        string Nickname { get; }
        string CatalogCode { get; }
        string Qq { get; }
        string QuickQuery { get; }
        string QuickQuery1 { get; }
        string QuickQuery2 { get; }
        string Telephone { get; }
        string BlogUrl { get; }
    }
}
