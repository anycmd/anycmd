
namespace Anycmd.Util
{
    using Document;
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
        public static ushort NextIndex<T>(this Dictionary<ushort, T> dict)
        {
            ushort next = 0;

            while (dict.ContainsKey(next))
            {
                next++;
            }

            return next;
        }

        public static object Get(this Dictionary<string, object> dict, string name)
        {
            object res;
            if (dict.TryGetValue(name, out res))
                return res;
            
            return null;
        }

        public static BsonValue Get(this Dictionary<string, BsonValue> dict, string name)
        {
            BsonValue res;
            if (dict.TryGetValue(name, out res))
                return res;
            
            return BsonValue.Null;
        }
    }
}
