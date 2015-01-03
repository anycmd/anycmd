
namespace Anycmd.Engine
{
    using System;

    public abstract class StateObject<T> : IStateObject, IEquatable<T> where T : class
    {
        // ReSharper disable once InconsistentNaming
        protected readonly Guid _id = Guid.Empty;

        protected StateObject(Guid id)
        {
            this._id = id;
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        protected abstract bool DoEquals(T other);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is T))
            {
                return false;
            }

            return this.Equals((T)obj);
        }

        public static bool operator ==(StateObject<T> a, StateObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            return !ReferenceEquals(a, null) && a.Equals(b);
        }

        public static bool operator !=(StateObject<T> a, StateObject<T> b)
        {
            return !(a == b);
        }

        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || DoEquals(other);
        }
    }
}
