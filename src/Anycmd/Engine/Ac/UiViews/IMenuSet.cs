
namespace Anycmd.Engine.Ac.UiViews
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是系统菜单集。
    /// </summary>
    public interface IMenuSet : IEnumerable<MenuState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        bool TryGetMenu(Guid menuId, out MenuState menu);
    }
}
