
namespace Anycmd.Specifications
{
    /// <summary>
    /// 表示组合规格的基类。
    /// </summary>
    /// <typeparam name="T">规格被应用到的对象的类型。</typeparam>
    public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
    {
        #region Private Fields
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>CompositeSpecification&lt;T&gt;</c> 类型的参数。
        /// </summary>
        /// <param name="left">左规格。</param>
        /// <param name="right">由规格。</param>
        protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this._left = left;
            this._right = right;
        }
        #endregion

        #region ICompositeSpecification Members
        /// <summary>
        /// 读取左规格。
        /// </summary>
        public ISpecification<T> Left
        {
            get { return this._left; }
        }

        /// <summary>
        /// 读取右规格。
        /// </summary>
        public ISpecification<T> Right
        {
            get { return this._right; }
        }
        #endregion
    }
}
