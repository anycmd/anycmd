
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// <see cref="IArchive"/>
    /// </summary>
    public abstract class ArchiveBase : EntityBase, IArchive
    {
        private int _numberId;
        private Guid _ontologyId;

        public string RdbmsType { get; set; }

        /// <summary>
        /// 源
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 数字标识
        /// </summary>
        public int NumberId
        {
            get { return _numberId; }
            set
            {
                if (value == _numberId) return;
                if (_numberId != default(int))
                {
                    throw new ValidationException("数字标识不能更改");
                }
                _numberId = value;
            }
        }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 本体
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value == _ontologyId) return;
                if (_ontologyId != Guid.Empty)
                {
                    throw new CoreException("不能更改所属本体");
                }
                _ontologyId = value;
            }
        }
        /// <summary>
        /// 归档时间
        /// </summary>
        public DateTime ArchiveOn { get; set; }
    }
}
