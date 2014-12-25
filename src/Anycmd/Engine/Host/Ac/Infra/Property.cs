
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 表示实体属性数据访问实体。
    /// <remarks>该模型是程序开发模型，被程序员使用，最终用户不关心本概念。</remarks>
    /// </summary>
    public class Property : PropertyBase, IAggregateRoot
    {
        public Property() { }

        public static Property Create(IPropertyCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Property
            {
                Id = input.Id.Value,
                Code = input.Code,
                Name = input.Name,
                DicId = input.DicId,
                Description = input.Description,
                EntityTypeId = input.EntityTypeId,
                GroupCode = input.GroupCode,
                ForeignPropertyId = input.ForeignPropertyId,
                Icon = input.Icon,
                GuideWords = input.GuideWords,
                InputType = input.InputType,
                IsDetailsShow = input.IsDetailsShow,
                IsDeveloperOnly = input.IsDeveloperOnly,
                IsInput = input.IsInput,
                IsTotalLine = input.IsTotalLine,
                SortCode = input.SortCode,
                Tooltip = input.Tooltip,
                MaxLength = input.MaxLength,
                CreateOn = DateTime.Now
            };
        }

        public void Update(IPropertyUpdateIo input)
        {
            this.ForeignPropertyId = input.ForeignPropertyId;
            this.Code = input.Code;
            this.DicId = input.DicId;
            this.Description = input.Description;
            this.GuideWords = input.GuideWords;
            this.Icon = input.Icon;
            this.InputType = input.InputType;
            this.IsDetailsShow = input.IsDetailsShow;
            this.IsDeveloperOnly = input.IsDeveloperOnly;
            this.IsInput = input.IsInput;
            this.IsTotalLine = input.IsTotalLine;
            this.MaxLength = input.MaxLength;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
