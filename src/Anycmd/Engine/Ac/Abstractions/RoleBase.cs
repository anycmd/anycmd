
namespace Anycmd.Engine.Ac.Abstractions
{
    using Exceptions;
    using Model;

    /// <summary>
    /// 角色基类
    /// </summary>
    public abstract class RoleBase : EntityBase, IRole
    {
        private string _name;

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public int NumberId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
