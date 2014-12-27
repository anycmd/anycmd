
namespace Anycmd.Edi.MessageTransfers
{
    using DataContracts;
    using Engine.Info;
    using System.Linq;

    public static class KeyValueExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataItem[] ToKeyValuePairs(this KeyValue[] source)
        {
            return source == null ? new DataItem[0] : source.Where(a => a != null).Select(a => new DataItem(a.Key, a.Value)).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static KeyValue[] ToKeyValues(this DataItem[] source)
        {
            return source == null ? new KeyValue[0] : source.Select(a => new KeyValue(a.Key, a.Value)).ToArray();
        }
    }
}
