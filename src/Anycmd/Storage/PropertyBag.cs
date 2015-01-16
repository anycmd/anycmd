
namespace Anycmd.Storage
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 属性袋。表示一个属性名称值对映射列表。
    /// </summary>
    public class PropertyBag : IEnumerable<KeyValuePair<string, object>>
    {
        #region Private Fields
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
        #endregion

        #region Public Static Fields
        /// <summary>
        /// 用于反射获取给定对象的属性的绑定标记。
        /// </summary>
        public static readonly BindingFlags PropertyBagBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>PropertyBag</c> 类型的对象。
        /// </summary>
        public PropertyBag()
        {
        }

        /// <summary>
        /// 初始化一个 <c>PropertyBag</c> 类型的对象并用给定的对象初始化袋的内容。
        /// </summary>
        /// <param name="target">给定的用来初始化属性袋的内容的对象。</param>
        public PropertyBag(object target)
        {
            target
                .GetType()
                .GetProperties(PropertyBagBindingFlags)
                .ToList()
                .ForEach(pi => _propertyValues.Add(pi.Name, pi.GetValue(target, null)));
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置给定属性名的属性值。
        /// </summary>
        /// <param name="idx">属性名</param>
        /// <returns>属性值。</returns>
        public object this[string idx]
        {
            get
            {
                return _propertyValues[idx];
            }
            set
            {
                _propertyValues[idx] = value;
            }
        }

        /// <summary>
        /// 读取属性袋中属性的个数。
        /// </summary>
        public int Count
        {
            get { return _propertyValues.Count; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 清空属性袋。
        /// </summary>
        public void Clear()
        {
            _propertyValues.Clear();
        }

        /// <summary>
        /// 添加一个属性到属性袋中。
        /// </summary>
        /// <param name="propertyName">要添加的属性的名字。</param>
        /// <param name="propertyValue">属性的值。</param>
        /// <returns>当前属性袋。</returns>
        public PropertyBag Add(string propertyName, object propertyValue)
        {
            _propertyValues.Add(propertyName, propertyValue);
            return this;
        }

        /// <summary>
        /// Adds a property to property bag, to be used as the sort field.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="propertyName">The name of the property to be added.</param>
        /// <returns>The instance with the added sort field.</returns>
        public PropertyBag AddSort<T>(string propertyName)
        {
            _propertyValues.Add(propertyName, default(T));
            return this;
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> value which represents the current property bag.
        /// </summary>
        /// <returns>The <see cref="System.String"/> value which represents the current property bag.</returns>
        public override string ToString()
        {
            return string.Format("[{0}]", string.Join(", ", _propertyValues.Keys.Select(p => p)));
        }
        #endregion

        #region IEnumerator<KeyValuePair<string, object>> Members
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _propertyValues.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _propertyValues.GetEnumerator();
        }
        #endregion
    }
}
