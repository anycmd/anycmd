
namespace Anycmd.Document
{
    using System.Collections.Generic;

    public class BsonArray : BsonValue, IEnumerable<BsonValue>
    {
        public BsonArray()
            : base(new List<object>())
        {
        }

        public BsonArray(List<object> array)
            : base(array)
        {
        }

        public void Add(BsonValue value)
        {
            this.RawValue.Add(value == null ? null : value.RawValue);
        }

        public void Remove(int index)
        {
            this.RawValue.RemoveAt(index);
        }

        public int Length
        {
            get
            {
                return this.RawValue.Count;
            }
        }

        public new List<object> RawValue
        {
            get
            {
                return (List<object>)base.RawValue;
            }
        }

        public IEnumerator<BsonValue> GetEnumerator()
        {
            foreach (var value in this.RawValue)
            {
                yield return new BsonValue(value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
