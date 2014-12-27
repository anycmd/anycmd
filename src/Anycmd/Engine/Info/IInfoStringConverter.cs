
namespace Anycmd.Engine.Info
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义信息字符串转化器。
    /// </summary>
    public interface IInfoStringConverter
    {
        /// <summary>
        /// 插件标识
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 插件标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 插件描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 插件作者。如xuexs
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 信息格式码：如json、xml。
        /// <remarks>
        /// 一个信息字符串转化器只能注册一种格式。
        /// </remarks>
        /// </summary>
        string InfoFormat { get; }

        /// <summary>
        /// 将给定的字符串反序列化为字符串数组。
        /// </summary>
        /// <param name="infoString"></param>
        /// <returns></returns>
        string[] ToStringArray(string infoString);

        /// <summary>
        /// 将给定的字符串数组序列化为字符串。
        /// <remarks>
        /// 举例，如果信息格式是json则序列化一个字符串数组得到的字符串形如["A","B","C"]。
        /// </remarks>
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        string ToInfoString(string[] stringArray);

        /// <summary>
        /// 将给定的信息字符串转化为信息项集合
        /// <remarks>
        /// 返回空数组而不返回null
        /// </remarks>
        /// </summary>
        /// <param name="infoString">信息字符串</param>
        /// <returns>返回空数组而不返回null</returns>
        DataItem[] ToDataItems(string infoString);

        /// <summary>
        /// 将给定的信息项集合转户为信息字符串
        /// </summary>
        /// <param name="infoItems"></param>
        /// <returns></returns>
        string ToInfoString(IEnumerable<DataItem> infoItems);
    }
}
