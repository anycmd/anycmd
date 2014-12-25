
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Model;
    using System;

    public sealed class EntityTypeMap
    {
        public static readonly EntityTypeMap Empty = new EntityTypeMap
        {
            ClrType = typeof(EntityObject),
            Code = string.Empty,
            Codespace = string.Empty
        };

        private EntityTypeMap() { }

        public static EntityTypeMap Create(Type clrType, string codespace, string code)
        {
            if (clrType == null)
            {
                throw new ArgumentNullException("clrType");
            }
            if (string.IsNullOrEmpty(codespace))
            {
                throw new ArgumentNullException("codespace");
            }
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            return new EntityTypeMap
            {
                ClrType = clrType,
                Codespace = codespace,
                Code = code
            };
        }

        public static EntityTypeMap Create<T>(string codespace, string code)
        {
            return Create(typeof(T), codespace, code);
        }

        public static EntityTypeMap Create<T>(string codespace)
        {
            return Create<T>(codespace, typeof(T).Name);
        }

        /// <summary>
        /// 一个CLR类型至多映射为一种实体类型
        /// </summary>
        public Type ClrType { get; private set; }
        /// <summary>
        /// 实体类型编码空间
        /// </summary>
        public string Codespace { get; private set; }
        /// <summary>
        /// 实体类型编码
        /// </summary>
        public string Code { get; private set; }

        public override int GetHashCode()
        {
            // 一个CLR类型至多映射为一种实体类型
            return ClrType.GetHashCode();
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
            if (!(obj is EntityTypeMap))
            {
                return false;
            }
            var left = this;
            var right = (EntityTypeMap)obj;

            return
                left.ClrType == right.ClrType &&
                string.Equals(left.Code + left.Codespace, right.Code + right.Codespace, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(EntityTypeMap a, EntityTypeMap b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(EntityTypeMap a, EntityTypeMap b)
        {
            return !(a == b);
        }
    }
}
