
namespace Anycmd.Engine.Info
{
    using DataContracts;
    using System.Collections.Generic;
    using System.Linq;

    public static class KeyValueExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static KeyValue[] ToDto(this IEnumerable<DataItem> source)
        {
            return source == null ? null : source.Select(a => new KeyValue(a.Key, a.Value)).ToArray();
        }
    }
}
