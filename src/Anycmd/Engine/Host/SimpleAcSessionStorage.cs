
namespace Anycmd.Engine.Host
{
    using System;
    using System.Collections.Concurrent;

    public sealed class SimpleAcSessionStorage : IAcSessionStorage
    {
        private readonly ConcurrentDictionary<string, object> _dic = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void SetData(string key, object data)
        {
            object value;
            if (_dic.TryGetValue(key, out value))
            {
                value = data;
            }
            else
            {
                _dic.GetOrAdd(key, data);   
            }
        }

        public object GetData(string key)
        {
            object value;
            return _dic.TryGetValue(key, out value) ? value : null;
        }

        public void Clear()
        {
            _dic.Clear();
        }
    }
}
