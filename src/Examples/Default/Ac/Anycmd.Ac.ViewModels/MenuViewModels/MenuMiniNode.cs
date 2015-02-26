
namespace Anycmd.Ac.ViewModels.MenuViewModels
{
    using Engine.Ac.UiViews;
    using System;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class MenuMiniNode
    {
        private readonly IAcDomain _acDomain;

        public MenuMiniNode(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public static MenuMiniNode Create(IAcDomain acDomain, IMenu menu)
        {
            return new MenuMiniNode(acDomain)
            {
                Id = menu.Id,
                expanded = false,
                img = menu.Icon,
                Name = menu.Name,
                ParentId = menu.ParentId,
                SortCode = menu.SortCode,
                Url = menu.Url
            };
        }

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
        public bool isLeaf
        {
            get
            {
                return _acDomain.MenuSet.All(a => a.ParentId != this.Id);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool expanded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string img { get; set; }
    }
}
