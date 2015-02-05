
namespace Anycmd.Document
{
    using System.Collections.Generic;
    using System.Linq;

    public class BsonObject : BsonValue
    {
        public BsonObject()
            : base(new Dictionary<string, object>())
        {
        }

        public BsonObject(Dictionary<string, object> obj)
            : base(obj)
        {
        }

        public new Dictionary<string, object> RawValue
        {
            get
            {
                return (Dictionary<string, object>)base.RawValue;
            }
        }

        /// <summary>
        /// Add fields in fluent api
        /// </summary>
        public BsonObject Add(string key, object value)
        {
            this[key] = new BsonValue(value);
            return this;
        }

        /// <summary>
        /// Returns all object keys
        /// </summary>
        public string[] Keys { get { return this.RawValue.Keys.ToArray(); } }

        /// <summary>
        /// Check if this object has a specific key
        /// </summary>
        public bool HasKey(string key)
        {
            return this.RawValue.ContainsKey(key);
        }

        /// <summary>
        /// Remove a specific key on object
        /// </summary>
        public bool RemoveKey(string key)
        {
            return this.RawValue.Remove(key);
        }
    }
}
