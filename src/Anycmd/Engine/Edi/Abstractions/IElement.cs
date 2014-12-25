
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 本体元素（Element）。“教师”二字标识了一个本体，当A告诉B“张老师去年是教语文的今年教数学了”，
    /// B说“我跟他是大学同学，他是数学系的”，A说“原来如此”。这两个沟通中的人能够互相明白对方的意思
    /// 首先是因为“老师”二字界定了本体，“张老师”三字定位了“实体”。而“教语文的”“教数学的”“数学系”
    /// 是张老师的“属性值”，而“属性”在此命名为“本体元素”，如教师本体有“所教学科”、“学历”、“专业”、“从教年月”等本体元素。
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 引用本体元素
        /// </summary>
        Guid? ForeignElementId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Actions { get; }
        /// <summary>
        /// 
        /// </summary>
        string UniqueElementIds { get; }
        /// <summary>
        /// 
        /// </summary>
        string InfoChecks { get; }
        /// <summary>
        /// 
        /// </summary>
        string InfoRules { get; }
        /// <summary>
        /// 
        /// </summary>
        string Triggers { get; }
        /// <summary>
        /// 
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        string FieldCode { get; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        string Ref { get; }
        /// <summary>
        /// 
        /// </summary>
        int? MaxLength { get; }
        /// <summary>
        /// 
        /// </summary>
        string OType { get; }
        /// <summary>
        /// 
        /// </summary>
        bool Nullable { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid? InfoDicId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Regex { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsInfoIdItem { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 
        /// </summary>
        int DeletionStateCode { get; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; }

        #region ElementUi
        /// <summary>
        /// 
        /// </summary>
        Guid? GroupId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Tooltip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsDetailsShow { get; }
        /// <summary>
        /// 是否导出
        /// </summary>
        bool IsExport { get; }
        /// <summary>
        /// 是否导入
        /// </summary>
        bool IsImport { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsInput { get; }
        /// <summary>
        /// 
        /// </summary>
        string InputType { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsTotalLine { get; }
        /// <summary>
        /// 
        /// </summary>
        int? InputWidth { get; }
        /// <summary>
        /// 
        /// </summary>
        int? InputHeight { get; }
        #endregion

        #region ElementColumnUI
        /// <summary>
        /// 
        /// </summary>
        bool IsGridColumn { get; }
        /// <summary>
        /// 
        /// </summary>
        int Width { get; }
        /// <summary>
        /// 
        /// </summary>
        bool AllowSort { get; }
        /// <summary>
        /// 
        /// </summary>
        bool AllowFilter { get; }
        #endregion
    }
}
