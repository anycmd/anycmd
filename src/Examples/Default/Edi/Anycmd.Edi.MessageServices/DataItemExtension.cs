
namespace Anycmd.Edi.MessageServices
{
    using DataContracts;
    using Engine.Info;

    /// <summary>
    /// 扩展<see cref="DataItem"/>数组对象，提供转成数据传输对象<see cref="KeyValue"/>数组的方法。
    /// </summary>
    public static class DataItemExtension
    {
        /// <summary>
        /// 转化为数据传输对象数组。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static KeyValue[] ToKeyValueArray(this DataItem[] source)
        {
            if (source == null)
            {
                return new KeyValue[0];
            }
            var data = new KeyValue[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != null)
                {
                    data[i] = new KeyValue
                    {
                        Key = source[i].Key,
                        Value = source[i].Value
                    };
                }
            }

            return data;
        }
    }
}
