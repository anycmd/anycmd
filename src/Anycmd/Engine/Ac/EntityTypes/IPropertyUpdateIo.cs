
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是更新实体属性时的输入或输出参数类型。
    /// </summary>
    public interface IPropertyUpdateIo : IEntityUpdateInput
    {
        Guid? ForeignPropertyId { get; }
        string Code { get; }
        string Description { get; }
        Guid? DicId { get; }
        string GuideWords { get; }
        string Icon { get; }
        string InputType { get; }
        bool IsDetailsShow { get; }
        bool IsDeveloperOnly { get; }
        bool IsInput { get; }
        bool IsTotalLine { get; }
        int? MaxLength { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
