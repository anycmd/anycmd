
namespace Anycmd.Edi.Client
{
    using DataContracts;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 键值对数组构建器
    /// </summary>
    public class KeyValueBuilder
    {
        private readonly List<KeyValue> _list = new List<KeyValue>();

        public KeyValueBuilder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        public KeyValueBuilder(string[] keys, string[] values)
        {
            if (keys == null || values == null)
            {
                throw new ArgumentNullException();
            }
            if (keys.Length != values.Length)
            {
                throw new ArgumentException();
            }
            for (var i = 0; i < keys.Length; i++)
            {
                var infoValue = new KeyValue {Key = keys[i], Value = values[i]};
                _list.Add(infoValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kvs"></param>
        public KeyValueBuilder(Dictionary<string, string> kvs)
        {
            if (kvs == null)
            {
                throw new ArgumentNullException();
            }
            foreach (var item in kvs)
            {
                var infoValue = new KeyValue {Key = item.Key, Value = item.Value};
                _list.Add(infoValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public KeyValueBuilder(IEnumerable<KeyValue> source)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    _list.Add(new KeyValue(item.Key, item.Value));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public KeyValueBuilder Append(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.Append(new KeyValue(key, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kv"></param>
        /// <returns></returns>
        public KeyValueBuilder Append(KeyValue kv)
        {
            if (kv == null)
            {
                throw new ArgumentNullException("kv");
            }
            _list.Add(kv);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyValue[] ToArray()
        {
            return _list.ToArray();
        }
    }
}
