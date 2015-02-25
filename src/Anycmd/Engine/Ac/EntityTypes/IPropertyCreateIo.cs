
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Engine.InOuts;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建实体属性时的输入或输出参数类型。
    /// </summary>
    public interface IPropertyCreateIo : IEntityCreateInput
    {
        /// <summary>
        /// 
        /// </summary>
        Guid? ForeignPropertyId { get; }
        string Code { get; }
        string Description { get; }
        Guid? DicId { get; }
        Guid EntityTypeId { get; }
        string GroupCode { get; }
        string GuideWords { get; }
        string Icon { get; }
        string InputType { get; }
        bool IsDetailsShow { get; }
        bool IsDeveloperOnly { get; }
        bool IsInput { get; }
        bool IsTotalLine { get; }
        int? MaxLength { get; }
        string Name { get; }
        string Tooltip { get; }
        int SortCode { get; }
    }
}
