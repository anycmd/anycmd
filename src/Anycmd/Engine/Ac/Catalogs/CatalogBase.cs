
namespace Anycmd.Engine.Ac.Catalogs
{
    using Exceptions;
    using Model;

    /// <summary>
    /// 目录基类<see cref="ICatalog"/>
    /// </summary>
    public abstract class CatalogBase : EntityBase, ICatalog
    {
        private string _code;
        private string _name;

        #region Ctor
        protected CatalogBase() { }
        #endregion

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _code = value;
            }
        }

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
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
