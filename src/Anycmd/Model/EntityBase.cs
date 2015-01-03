
namespace Anycmd.Model
{
    using Exceptions;
    using Util;
    using System;

    /// <summary>
    /// 实体模型基类型
    /// <remarks>
    /// 在EntityObject的基础上增加了CreateOn、CreateUserId、
    /// CreateBy、ModifiedOn、ModifiedUserID、ModifiedBy六个属性。
    /// 注意：
    /// 1， CreateOn和InsertedOn是不同的。CreateOn不是存储层的概念而是业务层的概念，
    /// 存储层可以有InsertedOn这样的概念，但是存储层的InsertedOn概念是无需让应用层知道的。
    /// 2， CreateUserId和InsertedUserID是不同的。CreateUserId不是存储层的概念而是业务层的概念，
    /// 存储层可以有InsertedUserID这样的概念，但是存储层的InsertedUserID概念是无需让应用层知道的。
    /// 存储层的InsertedUserID对应的是“sa”这样的数据库用户名。
    /// 3， ModifiedOn和UpdatedOn是不同的。ModifiedOn不是存储层的概念而是业务层的概念，
    /// 存储层可以有UpdatedOn这样的概念，但是存储层的UpdatedOn概念是无需让应用层知道的。
    /// 4， ModifiedUserID和UpdatedUserID是不同的。ModifiedUserID不是存储层的概念而是业务层的概念，
    /// 存储层可以有UpdatedUserID这样的概念，但是存储层的UpdatedUserID概念是无需让应用层知道的。
    /// 存储层的UpdatedUserID对应的是“sa”这样的数据库用户名。
    /// </remarks>
    /// </summary>
    public abstract class EntityBase : EntityObject, IEntityBase
    {
        private DateTime? _createOn;
        private DateTime? _modifiedOn;
        private Guid? _createUserId;
        private Guid? _modifiedUserId;
        private string _createBy;
        private string _modifiedBy;

        /// <summary>
        /// 实体创生时间。
        /// <remarks>
        /// 注意：CreateOn和InsertedOn是不同的。CreateOn不是存储层的概念而是业务层的概念，
        /// 存储层可以有InsertedOn这样的概念，但是存储层的InsertedOn概念是无需让应用层知道的。
        /// </remarks>
        /// </summary>
        public DateTime? CreateOn
        {
            get { return _createOn; }
            protected set
            {
                if (_createOn.HasValue && _createOn != value)
                {
                    throw new ValidationException("创建时间不能更改");
                }
                _createOn = value;
            }
        }

        /// <summary>
        /// 创建人表示。
        /// <remarks>
        /// 注意：CreateUserId和InsertedUserID是不同的。CreateUserId不是存储层的概念而是业务层的概念，
        /// 存储层可以有InsertedUserID这样的概念，但是存储层的InsertedUserID概念是无需让应用层知道的。
        /// 存储层的InsertedUserID对应的是“sa”这样的数据库用户名。
        /// </remarks>
        /// </summary>
        public Guid? CreateUserId
        {
            get { return _createUserId; }
            protected set
            {
                if (_createUserId.HasValue && _createUserId != value)
                {
                    throw new ValidationException("创建人不能更改");
                }
                _createUserId = value;
            }
        }

        /// <summary>
        /// 创建人名称。
        /// </summary>
        public string CreateBy
        {
            get { return _createBy; }
            protected set { _createBy = value; }
        }

        /// <summary>
        /// 实体最后修改时间。
        /// <remarks>
        /// 注意：ModifiedOn和UpdatedOn是不同的。ModifiedOn不是存储层的概念而是业务层的概念，
        /// 存储层可以有UpdatedOn这样的概念，但是存储层的UpdatedOn概念是无需让应用层知道的。
        /// </remarks>
        /// </summary>
        public DateTime? ModifiedOn
        {
            get { return _modifiedOn; }
            protected set
            {
                if (value != null)
                {
                    if (value.Value.IsNotValid())
                    {
                        throw new ValidationException("ModifiedOn值不合法" + value);
                    }
                }
                _modifiedOn = value;
            }
        }

        /// <summary>
        /// 实体最后修改人标识。
        /// <remarks>
        /// 注意：ModifiedUserID和UpdatedUserID是不同的。ModifiedUserID不是存储层的概念而是业务层的概念，
        /// 存储层可以有UpdatedUserID这样的概念，但是存储层的UpdatedUserID概念是无需让应用层知道的。
        /// 存储层的UpdatedUserID对应的是“sa”这样的数据库用户名。
        /// </remarks>
        /// </summary>
        public Guid? ModifiedUserId
        {
            get { return _modifiedUserId; }
            protected set
            {
                if (value != null)
                {
                    if (_modifiedUserId != null && value.Value == Guid.Empty)
                    {
                        throw new ValidationException("最后修改人标识错误" + value);
                    }
                }
                _modifiedUserId = value;
            }
        }

        /// <summary>
        /// 实体最后修改人名称。
        /// </summary>
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            protected set { _modifiedBy = value; }
        }

        public Guid RowGuid { get; protected set; }

        #region 显示实现
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? IEntityBase.CreateOn
        {
            get { return _createOn; }
            set
            {
                this.CreateOn = value;
            }
        }

        /// <summary>
        /// 创建人标识
        /// </summary>
        Guid? IEntityBase.CreateUserId
        {
            get { return _createUserId; }
            set
            {
                this.CreateUserId = value;
            }
        }

        /// <summary>
        /// 创建人[姓名|登录名]
        /// </summary>
        string IEntityBase.CreateBy
        {
            get { return _createBy; }
            set { this.CreateBy = value; }
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime? IEntityBase.ModifiedOn
        {
            get { return _modifiedOn; }
            set
            {
                this.ModifiedOn = value;
            }
        }

        /// <summary>
        /// 最后修改人标识
        /// </summary>
        Guid? IEntityBase.ModifiedUserId
        {
            get { return _modifiedUserId; }
            set
            {
                this.ModifiedUserId = value;
            }
        }

        /// <summary>
        /// 最后修改人[姓名|登录名]
        /// </summary>
        string IEntityBase.ModifiedBy
        {
            get { return _modifiedBy; }
            set { this.ModifiedBy = value; }
        }
        #endregion
    }
}
