
namespace Anycmd.Commands
{
    using System;
    using Util;

    /// <summary>
    /// 表示命令的基类。
    /// </summary>
    [Serializable]
    public class Command : ICommand
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>Command</c> 类型的对象。
        /// </summary>
        public Command()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// 初始化一个 <c>Command</c> 类型的对象。
        /// </summary>
        /// <param name="id">命令标识。</param>
        public Command(Guid id)
        {
            this.Id = id;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the hash code for current command object.
        /// </summary>
        /// <returns>The calculated hash code for the current command object.</returns>
        public override int GetHashCode()
        {
            return ReflectionHelper.GetHashCode(this.Id.GetHashCode());
        }

        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value indicating whether this instance is equal to a specified
        /// object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of the <see cref="Anycmd.Commands.ICommand"/> type and equals the value of this
        /// instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as Command;
            if ((object)other == (object)null)
                return false;
            return this.Id == other.Id;
        }
        #endregion

        #region IEntity Members
        /// <summary>
        /// 读取命令的标识。
        /// </summary>
        public virtual Guid Id
        {
            get;
            private set;
        }

        #endregion
    }
}
