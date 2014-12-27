
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    /// <summary>
    /// 本体元素业务实体模型基类。本体元素业务实体模型必须继承该类。
    /// <remarks>
    /// 业务实体模型充当了数据访问模型。
    /// </remarks>
    /// </summary>
    public abstract class ElementBase : EntityBase, IElement
    {
        private Guid _ontologyId;
        private string _code;
        private string _fieldCode;

        #region Ctor
        protected ElementBase()
        {
        }
        #endregion

        /// <summary>
        /// 引用本体元素
        /// </summary>
        public Guid? ForeignElementId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Actions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UniqueElementIds { get; set; }

        public string InfoChecks { get; set; }

        public string InfoRules { get; set; }

        public string Triggers { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != null)
                {
                    if (_code != value)
                    {
                        throw new AnycmdException("本体元素码会映射到接口上，不能更改");
                    }
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new AnycmdException("本体元素码不能为空");
                }
                value = value.Trim();
                _code = value;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string FieldCode
        {
            get { return _fieldCode; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _fieldCode = value;
            }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public int? MaxLength { get; set; }
        public bool Nullable { get; set; }
        public string OType { get; set; }
        /// <summary>
        /// 字典主键
        /// </summary>
        public Guid? InfoDicId { get; set; }
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 是信息标识项
        /// </summary>
        public bool IsInfoIdItem { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public string Ref { get; set; }
        /// <summary>
        /// 本体主键
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value == _ontologyId) return;
                if (_ontologyId != Guid.Empty)
                {
                    throw new AnycmdException("所属本体不能更改");
                }
                _ontologyId = value;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }

        /// <summary>
        /// 展示分组
        /// </summary>
        public Guid? GroupId { get; set; }
        /// <summary>
        /// 本体元素帮助
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 展示在详细信息页
        /// </summary>
        public bool IsDetailsShow { get; set; }
        public bool IsExport { get; set; }
        /// <summary>
        /// 是输入值
        /// </summary>
        public bool IsInput { get; set; }
        public bool IsImport { get; set; }
        /// <summary>
        /// 输入类型
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 占据整行
        /// </summary>
        public bool IsTotalLine { get; set; }
        /// <summary>
        /// 输入框宽度
        /// </summary>
        public int? InputWidth { get; set; }
        /// <summary>
        /// 输入框高度
        /// </summary>
        public int? InputHeight { get; set; }

        /// <summary>
        /// 是列显字段
        /// </summary>
        public bool IsGridColumn { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 允许排序
        /// </summary>
        public bool AllowSort { get; set; }
        /// <summary>
        /// 允许筛选
        /// </summary>
        public bool AllowFilter { get; set; }
    }
}
