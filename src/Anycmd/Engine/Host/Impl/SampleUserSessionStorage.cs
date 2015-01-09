
namespace Anycmd.Engine.Host.Impl
{
    using System.Collections.Generic;

    public class SampleUserSessionStorage : IUserSessionStorage
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void SetData(string key, object data)
        {
            _data.Add(key, data);
        }

        public object GetData(string key)
        {
            return _data[key];
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}
