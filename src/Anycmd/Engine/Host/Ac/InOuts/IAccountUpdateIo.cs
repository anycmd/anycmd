
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;
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
        string OrganizationCode { get; }
        string QQ { get; }
        string QuickQuery { get; }
        string QuickQuery1 { get; }
        string QuickQuery2 { get; }
        string Telephone { get; }
    }
}
