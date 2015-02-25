
namespace Anycmd.Ac.ViewModels.ButtonViewModels
{
    using Engine.Ac;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonTr
    {
        public ButtonTr() { }

        public static ButtonTr Create(ButtonState button)
        {
            return new ButtonTr
            {
                Code = button.Code,
                CreateOn = button.CreateOn,
                Icon = button.Icon,
                Id = button.Id,
                IsEnabled = button.IsEnabled,
                Name = button.Name,
                CategoryCode = button.CategoryCode,
                SortCode = button.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
