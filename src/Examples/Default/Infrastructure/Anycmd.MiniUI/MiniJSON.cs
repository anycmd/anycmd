
namespace Anycmd.MiniUI
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public sealed class MiniJSON
    {
        public static string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Encode(object o)
        {
            if (o == null || o.ToString() == "null") return null;

            if (o != null && (o is String || o is string))
            {
                return o.ToString();
            }
            var dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = DateTimeFormat;
            return JsonConvert.SerializeObject(o, dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Decode(string json)
        {
            if (String.IsNullOrEmpty(json)) return "";
            object o = JsonConvert.DeserializeObject(json);
            if (o is String || o is string)
            {
                o = JsonConvert.DeserializeObject(o.ToString());
            }
            object v = toObject(o);
            return v;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Decode(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        private static object toObject(object o)
        {
            if (o == null) return null;

            if (o is string)
            {
                //判断是否符合2010-09-02T10:00:00的格式
                string s = o.ToString();
                if (s.Length == 19 && s[10] == 'T' && s[4] == '-' && s[13] == ':')
                {
                    o = System.Convert.ToDateTime(o);
                }
            }
            else
            {
                var jObject = o as JObject;
                if (jObject != null)
                {
                    var jo = jObject;
                    var h = new Hashtable();
                    foreach (KeyValuePair<string, JToken> entry in jo)
                    {
                        h[entry.Key] = toObject(entry.Value);
                    }
                    o = h;
                }
                else
                {
                    var iList = o as IList;
                    if (iList != null)
                    {
                        var list = new ArrayList();
                        list.AddRange(iList);
                        int i = 0, l = list.Count;
                        for (; i < l; i++)
                        {
                            list[i] = toObject(list[i]);
                        }
                        o = list;
                    }
                    else if (typeof(JValue) == o.GetType())
                    {
                        var v = (JValue)o;
                        o = toObject(v.Value);
                    }
                    else
                    {
                    }
                }
            }
            return o;
        }
    }
}
