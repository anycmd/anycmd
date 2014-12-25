
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class InfoDicItemBase : EntityBase, IInfoDicItem {
        private string _code;

        protected InfoDicItemBase() { }

        /// <summary>
        /// 字典值级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 本地方言编码
        /// </summary>
        public string Code {
            get { return _code; }
            set {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("编码不能为空");
                }
                value = value.Trim();
                if (value != _code) {
                    _code = value;
                }
            }
        }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 字典主键
        /// </summary>
        public Guid InfoDicId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }
    }
}
