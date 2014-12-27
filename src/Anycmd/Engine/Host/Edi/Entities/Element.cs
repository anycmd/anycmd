
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Engine.Edi.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示本体元素数据访问实体。
    /// <remarks>
    /// “教师”二字标识了一个本体，当A告诉B“张老师去年是教语文的今年教数学了”，B说“我跟他是大学同学，他是数学系的”，
    /// A说“原来如此”。这两个沟通中的人能够互相明白对方的意思首先是因为“老师”二字界定了本体，“张老师”三字定位了“实体”。
    /// 而“教语文的”“教数学的”“数学系”是张老师的“属性值”，而“属性”在此命名为“本体元素”，
    /// 如教师本体有“所教学科”、“学历”、“专业”、“从教年月”等本体元素。
    /// </remarks>
    /// </summary>
    public class Element : ElementBase, IAggregateRoot
    {
        public Element() { }

        public static Element Create(IElementCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Element
            {
                Id = input.Id.Value,
                Code = input.Code,
                AllowFilter = input.AllowFilter,
                AllowSort = input.AllowSort,
                Description = input.Description,
                GroupId = input.GroupId,
                Icon = input.Icon,
                FieldCode = input.FieldCode,
                ForeignElementId = input.ForeignElementId,
                IsInfoIdItem = input.IsInfoIdItem,
                InfoDicId = input.InfoDicId,
                InputHeight = input.InputHeight,
                InputType = input.InputType,
                IsInput = input.IsInput,
                Actions = string.Empty,
                DeletionStateCode = 0,
                InfoChecks = string.Empty,
                InfoRules = string.Empty,
                IsDetailsShow = input.IsDetailsShow,
                InputWidth = input.InputWidth,
                Width = input.Width,
                IsEnabled = input.IsEnabled,
                IsExport = input.IsExport,
                IsGridColumn = input.IsGridColumn,
                IsImport = input.IsImport,
                Name = input.Name,
                IsTotalLine = input.IsTotalLine,
                MaxLength = input.MaxLength,
                Ref = input.Ref,
                OntologyId = input.OntologyId,
                Regex = input.Regex,
                SortCode = input.SortCode,
                Tooltip = input.Tooltip,
                Triggers = string.Empty,
                UniqueElementIds = string.Empty,
                OType = input.OType,
                Nullable = input.Nullable
            };
        }

        public void Update(IElementUpdateIo input)
        {
            this.Nullable = input.Nullable;
            this.AllowFilter = input.AllowFilter;
            this.AllowSort = input.AllowSort;
            this.Code = input.Code;
            this.Description = input.Description;
            this.FieldCode = input.FieldCode;
            this.GroupId = input.GroupId;
            this.Icon = input.Icon;
            this.InfoDicId = input.InfoDicId;
            this.InputHeight = input.InputHeight;
            this.InputType = input.InputType;
            this.InputWidth = input.InputWidth;
            this.IsDetailsShow = input.IsDetailsShow;
            this.IsEnabled = input.IsEnabled;
            this.IsExport = input.IsExport;
            this.IsGridColumn = input.IsGridColumn;
            this.IsExport = input.IsExport;
            this.IsInfoIdItem = input.IsInfoIdItem;
            this.IsInput = input.IsInput;
            this.IsTotalLine = input.IsTotalLine;
            this.MaxLength = input.MaxLength;
            this.Name = input.Name;
            this.Ref = input.Ref;
            this.Regex = input.Regex;
            this.SortCode = input.SortCode;
            this.Width = input.Width;
            this.Tooltip = Tooltip;
        }
    }
}
