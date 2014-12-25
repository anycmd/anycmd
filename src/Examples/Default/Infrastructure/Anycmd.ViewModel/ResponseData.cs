
namespace Anycmd.ViewModel
{
    /// <summary>
    /// 表示请求响应事务中的响应数据的模型。
    /// </summary>
    public class ResponseData : IViewModel
    {
        private string _resultType = "success";
        private bool _success = true;

        /// <summary>
        /// 读取或设置表示请求响应事务中的服务端业务过程是否成功了的业务状态，True表示成功了。
        /// </summary>
        public bool success
        {
            get { return _success; }
            set
            {
                _success = value;
                if (!_success && _resultType == "success")
                {
                    _resultType = "";
                }
            }
        }

        /// <summary>
        /// 取值：success、info、warning、danger
        /// </summary>
        public string resultType
        {
            get
            {
                return _resultType;
            }
            private set { _resultType = value; }
        }

        /// <summary>
        /// 表示请求响应事务服务端返回的业务特定说明文字。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 一般只有在表格内编辑模式的时候才用赋此值
        /// 为处理表格内编辑模式（RowEditor）游离对象时返回Id
        /// </summary>
        public object id { get; set; }

        /// <summary>
        /// 将当前对象的resultType属性值置为info并返回当前对象。
        /// </summary>
        /// <returns></returns>
        public ResponseData Info()
        {
            this.resultType = "info";
            return this;
        }

        /// <summary>
        /// 将当前对象的resultType属性值置为warning并返回当前对象。
        /// </summary>
        /// <returns></returns>
        public ResponseData Warning()
        {
            this.resultType = "warning";
            return this;
        }

        /// <summary>
        /// 将当前对象的resultType属性值置为danger并返回当前对象。
        /// </summary>
        /// <returns></returns>
        public ResponseData Danger()
        {
            this.resultType = "danger";
            return this;
        }

        /// <summary>
        /// 将当前对象的resultType属性值置为error并返回当前对象。
        /// </summary>
        /// <returns></returns>
        public ResponseData Error()
        {
            this.resultType = "error";
            this.success = false;
            return this;
        }
    }
}
