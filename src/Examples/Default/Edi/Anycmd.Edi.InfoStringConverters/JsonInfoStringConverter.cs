
namespace Anycmd.Edi.InfoStringConverters
{
    using Engine.Info;
    using Model;
    using ServiceStack.Text;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IInfoStringConverter))]
    public sealed class JsonInfoStringConverter : DisposableObject, IInfoStringConverter
    {
        private static readonly Guid id = new Guid("1B541773-7D71-4A16-9691-D8D160670B67");
        private const string title = "json格式信息值转化器";
        private const string description = "json格式信息值转化器";
        private const string author = "xuexs";
        private static readonly DataItem[] EmptyKeyValues = new DataItem[0];
        private static readonly string[] EmptyStringArray = new string[0];

        /// <summary>
        /// 
        /// </summary>
        public string InfoFormat
        {
            get
            {
                return "json";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id
        {
            get { return id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Author
        {
            get { return author; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoString"></param>
        /// <returns></returns>
        public string[] ToStringArray(string infoString)
        {
            if (string.IsNullOrEmpty(infoString))
            {
                return EmptyStringArray;
            }
            var infoValues = JsonSerializer.DeserializeFromString<string[]>(infoString);

            return infoValues.Length == 0 ? EmptyStringArray : infoValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public string ToInfoString(string[] stringArray)
        {
            if (stringArray == null || stringArray.Length == 0)
            {
                return string.Empty;
            }

            return JsonSerializer.SerializeToString(stringArray);
        }

        /// <summary>
        /// 将给定的json字符串转化为键值对字典返回
        /// </summary>
        /// <param name="infoValueString">json格式的字符串</param>
        /// <returns>键值对字典</returns>
        public DataItem[] ToDataItems(string infoValueString)
        {
            if (string.IsNullOrEmpty(infoValueString))
            {
                return EmptyKeyValues;
            }
            Guid id;
            if (Guid.TryParse(infoValueString, out id))
            {
                return new DataItem[] { new DataItem("Id", infoValueString) };
            }
            var infoValues = JsonSerializer.DeserializeFromString<Dictionary<string, string>>(infoValueString);
            if (infoValues.Count == 0)
            {
                return EmptyKeyValues;
            }

            return infoValues.Select(item => new DataItem(item.Key, item.Value)).ToArray();
        }

        /// <summary>
        /// 根据给定的命令描述对象返回转化得到的信息字符串
        /// </summary>
        /// <param name="infoItems"></param>
        /// <returns></returns>
        public string ToInfoString(IEnumerable<DataItem> infoItems)
        {
            if (infoItems == null)
            {
                return "{}";
            }
            var infoValueDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in infoItems.Where(a => a != null && a.Key != null))
            {
                if (!infoValueDic.ContainsKey(item.Key))
                {
                    infoValueDic.Add(item.Key, item.Value);
                }
            }

            return JsonSerializer.SerializeToString(infoValueDic);
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
