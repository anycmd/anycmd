
namespace Anycmd.Engine.Host.Ac.Infra
{

    /// <summary>
    /// 流程深度
    /// </summary>
    public enum ConfigLevel : byte
    {
        /// <summary>
        /// 非法的配置深度
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 本体动作级
        /// </summary>
        Level1Action = 1,
        /// <summary>
        /// 元素级
        /// </summary>
        Level2ElementAction = 2,
        /// <summary>
        /// 客户端动作级
        /// </summary>
        Level3ClientAction = 3,
        /// <summary>
        /// 客户端元素级
        /// </summary>
        Level4ClientElementAction = 4,
        /// <summary>
        /// 目录级。
        /// </summary>
        Level5CatalogAction = 5,
        /// <summary>
        /// 实体级动
        /// </summary>
        Level6EntityAction = 6,
        /// <summary>
        /// 实体元素级
        /// </summary>
        Level7EntityElementAction = 7
    }
}
