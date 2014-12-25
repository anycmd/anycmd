
namespace Anycmd.Engine.Ac.Abstractions
{
    using Exceptions;
    using Model;

    /// <summary>
    /// 动态职责分离角色集基类。
    /// </summary>
    public abstract class DsdSetBase : EntityBase, IDsdSet
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("名称是必须的");
                }
                _name = value;
            }
        }

        /// <summary>
        /// 阀值
        /// </summary>
        public int DsdCard { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        public string Description { get; set; }
    }
}
