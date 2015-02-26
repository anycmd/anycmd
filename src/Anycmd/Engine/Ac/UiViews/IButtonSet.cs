
namespace Anycmd.Engine.Ac.UiViews
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是系统按钮集。
    /// </summary>
    public interface IButtonSet : IEnumerable<ButtonState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        bool ContainsButton(Guid buttonId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonCode"></param>
        /// <returns></returns>
        bool ContainsButton(string buttonCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonId"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        bool TryGetButton(Guid buttonId, out ButtonState button);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonCode"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        bool TryGetButton(string buttonCode, out ButtonState button);
    }
}
