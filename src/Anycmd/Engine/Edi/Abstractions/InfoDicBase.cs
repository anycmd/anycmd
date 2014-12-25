
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;

    public abstract class InfoDicBase : EntityBase, IInfoDic {
        private string _code;

        #region Ctor
        protected InfoDicBase() {
        }
        #endregion

        /// <summary>
        /// 编码
        /// </summary>
        public string Code {
            get { return _code; }
            set {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("信息字典编码不能为空");
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
