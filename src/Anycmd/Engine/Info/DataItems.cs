
namespace Anycmd.Engine.Info
{
    using Exceptions;
    using System;

    /// <summary>
    /// 数据项集合
    /// </summary>
    public sealed class DataItems
    {
        private DataItem[] _dataItems;
        private string _infoString;
        private readonly IInfoStringConverter _converter;

        private DataItems()
        {
        }

        /// <summary>
        /// 命令信息项集合构造。两个参数不能同时为null
        /// </summary>
        /// <param name="dataItems">信息字符串所对应的数据项数组</param>
        /// <param name="infoString">数据项数组所对应的信息字符串</param>
        /// <param name="converter">信息格式：如json、xml</param>
        public DataItems(DataItem[] dataItems, string infoString, IInfoStringConverter converter)
        {
            if (dataItems == null && infoString == null)
            {
                throw new AnycmdException("dataItems和infoString不能同时为null");
            }
            if ((dataItems == null || infoString == null) && converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            this._converter = converter;
            this.Items = dataItems;
            this.InfoString = infoString;
        }

        /// <summary>
        /// 数据项数组所对应的信息字符串
        /// </summary>
        public string InfoString
        {
            get { return _infoString ?? (_infoString = _converter.ToInfoString(_dataItems)); }
            private set { _infoString = value; }
        }

        /// <summary>
        /// 信息字符串所对应的数据项数组
        /// </summary>
        public DataItem[] Items
        {
            get
            {
                if (_dataItems == null)
                {
                    _dataItems = _converter.ToDataItems(_infoString);
                    if (_dataItems == null)
                    {
                        throw new AnycmdException("信息字符串转化器返回意外的null数组。");
                    }
                }
                return _dataItems;
            }
            private set { _dataItems = value; }
        }

        /// <summary>
        /// 判断集合是否是空的，即集合中没有命令信息项。
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (_dataItems == null)
                {
                    return Items.Length == 0;
                }
                return _dataItems.Length == 0;
            }
        }
    }
}
