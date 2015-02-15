
namespace Anycmd.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示字典数据读取器。它在原生字典的基础上添加了容器对象的引用。
    /// </summary>
    public sealed class DicReader : Dictionary<string, object>
    {
        public DicReader(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            this.AcDomain = acDomain;
        }

        /// <summary>
        /// 容器对象引用。
        /// </summary>
        public IAcDomain AcDomain { get; private set; }
    }
}
