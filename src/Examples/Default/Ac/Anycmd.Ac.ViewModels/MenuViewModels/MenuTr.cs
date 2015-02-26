
namespace Anycmd.Ac.ViewModels.MenuViewModels
{
    using Engine.Ac.UiViews;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class MenuTr
    {
        public MenuTr() { }

        public static MenuTr Create(IMenu menu)
        {
            return new MenuTr
            {
                Icon = menu.Icon,
                Id = menu.Id,
                Name = menu.Name,
                ParentId = menu.ParentId,
                SortCode = menu.SortCode,
                Url = menu.Url
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
    }
}
