
namespace Anycmd.Engine.Info
{
    using Exceptions;

    /// <summary>
    /// 数据项集合对
    /// </summary>
    public sealed class DataItemsTuple
    {
        readonly IInfoStringConverter _converter;
        private string _getElementString;
        private string[] _getElement;
        private readonly IAcDomain _acDomain;

        private DataItemsTuple(
            IAcDomain acDomain,
            DataItem[] dataIdItems, string idString,
            DataItem[] dataValueItems, string valueString,
            string[] getElement, string getElementString,
            string infoFormat)
        {
            this._acDomain = acDomain;
            if (dataIdItems == null && idString == null)
            {
                dataIdItems = new DataItem[0];
            }
            if (dataValueItems == null && valueString == null)
            {
                dataValueItems = new DataItem[0];
            }
            if (string.IsNullOrEmpty(infoFormat))
            {
                throw new AnycmdException("infoFormat不能为空");
            }
            if (!acDomain.NodeHost.InfoStringConverters.TryGetInfoStringConverter(infoFormat, out _converter))
            {
                throw new AnycmdException("意外的信息格式" + infoFormat);
            }
            this.InfoFormat = infoFormat;
            this.QueryList = getElement;
            this.QueryList = getElement;
            this.QueryListString = getElementString;
            this.IdItems = new DataItems(dataIdItems, idString, _converter);
            this.ValueItems = new DataItems(dataValueItems, valueString, _converter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="dataIdItems"></param>
        /// <param name="dataValueItems"></param>
        /// <param name = "getElement"></param>
        /// <param name="infoFormat"></param>
        /// <returns></returns>
        public static DataItemsTuple Create(
            IAcDomain acDomain,
            DataItem[] dataIdItems,
            DataItem[] dataValueItems,
            string[] getElement,
            string infoFormat)
        {
            return new DataItemsTuple(acDomain, dataIdItems, null, dataValueItems, null, getElement, null, infoFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="idString"></param>
        /// <param name="valueString"></param>
        /// <param name = "getElementString"></param>
        /// <param name="infoFormat"></param>
        /// <returns></returns>
        public static DataItemsTuple Create(
            IAcDomain acDomain,
            string idString,
            string valueString,
            string getElementString,
            string infoFormat)
        {
            return new DataItemsTuple(acDomain, null, idString, null, valueString, null, getElementString, infoFormat);
        }

        public IAcDomain Host
        {
            get { return _acDomain; }
        }

        /// <summary>
        /// 查看当前信息项集合对的信息格式
        /// </summary>
        public string InfoFormat { get; private set; }

        /// <summary>
        /// 本体元素码数组，指示当前命令的ActionInfoResult响应项。对于get型命令来说null或空数组表示返回所有当前client有权get的字段，
        /// 对于非get型命令来说null或空数组表示不返回ActionInfoResult值。
        /// </summary>
        public string[] QueryList
        {
            get
            {
                if (_getElement == null && _getElementString == null)
                {
                    return null;
                }
                return _getElement ?? (_getElement = _converter.ToStringArray(_getElementString));
            }
            private set { _getElement = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string QueryListString
        {
            get
            {
                if (_getElement == null && _getElementString == null)
                {
                    return null;
                }
                return _getElementString ?? (_getElementString = _converter.ToInfoString(_getElement));
            }
            private set { _getElementString = value; }
        }

        /// <summary>
        /// 信息标识项。不可能为null
        /// </summary>
        public DataItems IdItems { get; private set; }

        /// <summary>
        /// 信息值项。不可能为null
        /// </summary>
        public DataItems ValueItems { get; private set; }
    }
}
