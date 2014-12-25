
namespace Anycmd.Engine.Edi.Abstractions
{
    public enum ConstraintType
    {
        /// <summary>
        /// 主键约束
        /// </summary>
        PrimaryKey = 1,
        /// <summary>
        /// 唯一约束
        /// </summary>
        Unique = 2,
        /// <summary>
        /// 引用约束
        /// </summary>
        ForeignKey = 3,
        /// <summary>
        /// 
        /// </summary>
        Check = 4,
        /// <summary>
        /// 
        /// </summary>
        Default = 5
    }
}
