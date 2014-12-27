
namespace Anycmd.Engine.Info
{
    using Host.Edi;

    /// <summary>
    /// 数据元组。数据元组是什么？在关系数据库领域中，数据元组就是一行记录。这里的数据元组对应的就是一行记录，它不一定是完整的一行记录，可以是一行记录的投影。
    /// </summary>
    public interface IDataTuples
    {
        /// <summary>
        /// 排序的本体元素集合。该集合的顺序决定Tuples数组的顺序。
        /// <remarks>映射关系数据库中表中的列来理解该属性。</remarks>
        /// </summary>
        OrderedElementSet Columns { get; }

        /// <summary>
        /// 数据元组。借助关系数据库的概念来理解的话，这里的每一个对象对应关系数据库表中的一个单元格中的值。
        /// <remarks>
        /// 数组顺序取决于Columns属性的顺序。映射关系数据库表中的单元格来理解该属性。
        /// </remarks>
        /// </summary>
        object[][] Tuples { get; }
    }
}
