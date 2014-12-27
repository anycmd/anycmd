
namespace Anycmd.Util
{
    using Exceptions;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 承载扩展的用以完成字符串和枚举的相互转换的方法。
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 返回当前枚举值的字符串形式。
        /// <remarks>字符串形式是枚举对应的名称</remarks>
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <typeparam name="T">枚举类型参数</typeparam>
        /// <returns>对应于枚举值的枚举名</returns>
        public static string ToName<T>(this T value) where T : struct
        {
            return EnumDic<T>.Current[value];
        }

        /// <summary>
        /// 尝试把给定的枚举名转化为枚举类型值。
        /// </summary>
        /// <param name="name">枚举名</param>
        /// <param name="value">枚举值</param>
        /// <typeparam name="T">枚举类型参数</typeparam>
        /// <returns>转化成功返回True，返回False表示转化不成功</returns>
        public static bool TryParse<T>(this string name, out T value) where T : struct
        {
            if (name != null) return EnumDic<T>.TryGetValue(name, out value);
            value = default(T);
            return false;
        }

        /// <summary>
        /// 枚举字典。用于高效第完成枚举类型和字符串类型的转换。
        /// </summary>
        /// <typeparam name="T">枚举类型参数</typeparam>
        public class EnumDic<T>
            where T : struct
        {
            public static readonly EnumDic<T> Current = new EnumDic<T>();

            private readonly Dictionary<ValueType, string>
                _dicByValue = new Dictionary<ValueType, string>();
            private readonly Dictionary<string, T>
                _dicByString = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

            static EnumDic()
            {
                Init();
            }

            private EnumDic()
            {
                if (typeof(Enum) != typeof(T).BaseType)
                {
                    throw new Exception("类型参数不合法，当前泛型类的类型参数必须为枚举类型");
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stateCode"></param>
            /// <returns></returns>
            public string this[T stateCode]
            {
                get
                {
                    return _dicByValue[stateCode];
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static bool TryGetValue(string name, out T value)
            {
                return Current._dicByString.TryGetValue(name, out value);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stateCode"></param>
            /// <returns></returns>
            public string this[ValueType stateCode]
            {
                get
                {
                    if (!_dicByValue.ContainsKey(stateCode))
                    {
                        throw new AnycmdException("意外的枚举值:" + stateCode.ToString());
                    }

                    return _dicByValue[stateCode];
                }
            }

            #region private methods
            private static void Init()
            {
                var names = Enum.GetNames(typeof(T));
                var values = Enum.GetValues(typeof(T)) as IEnumerable;
                var i = 0;
                foreach (var item in values)
                {
                    Current._dicByValue.Add((ValueType)item, names[i]);
                    Current._dicByString.Add(names[i], (T)item);
                    i++;
                }
            }
            #endregion
        }
    }
}
