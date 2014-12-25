
namespace Anycmd.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示字典数据读取器。它在原生字典的基础上添加了容器对象的引用。
    /// </summary>
    public sealed class DicReader : Dictionary<string, object>
    {
        public DicReader(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this.Host = host;
        }

        /// <summary>
        /// 容易对象引用。
        /// </summary>
        public IAcDomain Host { get; private set; }
    }
}
