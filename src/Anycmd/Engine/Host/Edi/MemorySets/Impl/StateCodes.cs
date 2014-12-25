
namespace Anycmd.Engine.Host.Edi.MemorySets.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// 状态码上下文
    /// </summary>
    public sealed class StateCodes : IStateCodes
    {
        public static readonly StateCodes Empty = new StateCodes();

        private static readonly List<StateCode> List = new List<StateCode>();
        private static bool _initialized = false;
        private static readonly object Locker = new object();

        /// <summary>
        /// 
        /// </summary>
        internal static void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                List.Clear();
                var stateCodeEnumType = typeof(Status);
                var members = stateCodeEnumType.GetFields();
                var list = new List<StateCode>();
                var ok = new StateCode((int)Status.Ok, "Ok", "成功");
                list.Add(ok);

                // 通过反射构建状态码对象
                foreach (var item in members)
                {
                    if (item.DeclaringType == stateCodeEnumType)
                    {
                        var value = (Status)item.GetValue(Status.Ok);
                        if (value != Status.Ok)
                        {
                            var attrs = item.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
                            var description = attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : item.Name;
                            var reasonPhrase = value.ToString();
                            var entity = new StateCode((int)value, reasonPhrase, description);
                            list.Add(entity);
                        }
                    }
                }
                foreach (var item in list.OrderBy(a => a.Code))
                {
                    List.Add(item);
                }
                _initialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<StateCode> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return List.GetEnumerator();
        }
    }
}
