
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示按钮数据访问实体。
    /// <remarks>该模型是程序开发模型，被程序员使用，用户不关心本概念。</remarks>
    /// </summary>
    public class Button : ButtonBase, IAggregateRoot
    {
        #region Ctor
        public Button()
        {
        }
        #endregion

        public static Button Create(IButtonCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Button
            {
                Id = input.Id.Value,
                Code = input.Code,
                Name = input.Name,
                Icon = input.Icon,
                Description = input.Description,
                CategoryCode = input.CategoryCode,
                IsEnabled = input.IsEnabled,
                SortCode = input.SortCode
            };
        }

        public void Update(IButtonUpdateIo input)
        {
            this.Code = input.Code;
            this.CategoryCode = input.CategoryCode;
            this.Description = input.Description;
            this.Icon = input.Icon;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
