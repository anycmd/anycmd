
namespace Anycmd.Engine.Host.Edi.Handlers
{

    /// <summary>
    /// 数据库命令是什么？通常一个本体下会有编码为Create、Update、Get、Head、Delete等的动作，我们通常使用“动作码+命令”的形式来程序命令为
    /// Create型命令、Update型命令、Get型命令、Head型命令等，但不是每一种类型的命令都能映射为数据库命令的。数据库命令是什么？对于RDB和sql来说就是inert型、
    /// update型、delete型、select型等sql命令语句。那么DbCommand模型就类似一个筛子，有些动作型的命令可以通过这个筛子而有些会被阻挡在数据库层之外。
    /// </summary>
    public enum DbActionType
    {
        /// <summary>
        /// 非法的数据库动作类型
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 插入
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3
    }
}
