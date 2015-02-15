
namespace Anycmd.Engine.Edi.InOuts
{
    using Engine.InOuts;
    using System;

    public interface IElementCreateIo : IEntityCreateInput
    {
        string OType { get; }
        bool Nullable { get; }
        bool AllowFilter { get; }
        bool AllowSort { get; }
        string Code { get; }
        Guid? ForeignElementId { get; }
        string DbType { get; }
        string Description { get; }
        string FieldCode { get; }
        Guid? GroupId { get; }
        string Icon { get; }
        Guid? InfoDicId { get; }
        int? InputHeight { get; }
        string InputType { get; }
        int? InputWidth { get; }
        bool IsDetailsShow { get; }
        int IsEnabled { get; }
        bool IsExport { get; }
        bool IsGridColumn { get; }
        bool IsImport { get; }
        bool IsInfoIdItem { get; }
        bool IsInput { get; }
        bool IsTotalLine { get; }
        int? MaxLength { get; }
        string Name { get; }
        Guid OntologyId { get; }
        string Ref { get; }
        string Regex { get; }
        int SortCode { get; }
        int Width { get; }
        string Tooltip { get; }
    }
}
